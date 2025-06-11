using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknoJobs.Application.DTOs;
using TeknoJobs.Domain.Entities;
using TeknoJobs.Domain.Interfaces;

namespace TeknoJobs.Controllers
{
    /// <summary>
    /// Provides endpoints for managing job-related operations, including creating, updating, retrieving, and listing
    /// jobs.
    /// </summary>
    /// <remarks>This controller handles CRUD operations for jobs, including validation of input data and
    /// interaction with the underlying data store. It requires authorization and is part of the API version 1
    /// group.</remarks>
    [ApiController]
    [Route("api/v1/jobs")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<JobsController> _logger;
        private readonly IMapper _mapper;
        public JobsController(IUnitOfWork unitOfWork, ILogger<JobsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        #region Jobs CRUD Operations
        /// <summary>
        /// Creates a new job entry based on the provided job details.
        /// </summary>
        /// <remarks>This method validates the input request to ensure all required fields are provided
        /// and valid. It checks the existence of the specified location and department before creating the job. If
        /// successful, the method generates a unique job code, saves the job to the database, and returns a 201 Created
        /// response with the URL of the newly created job.</remarks>
        /// <param name="request">The job details provided by the client. Must include a non-null title, description, valid LocationId, and
        /// valid DepartmentId.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation: <list type="bullet">
        /// <item><description>201 Created: If the job is successfully created.</description></item>
        /// <item><description>400 Bad Request: If the request is invalid (e.g., missing required fields or invalid
        /// IDs).</description></item> <item><description>500 Internal Server Error: If an unexpected error occurs
        /// during processing.</description></item> </list></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateJob([FromBody] JobsRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Job request cannot be null.");
                }
                if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description))
                {
                    return BadRequest("Job title and description are required.");
                }
                if (request.LocationId <= 0 || request.DepartmentId <= 0)
                {
                    return BadRequest("Valid LocationId and DepartmentId are required.");
                }

                var locationFromDb = await _unitOfWork.Locations.AnyAsync(l => l.Id == request.LocationId);
                if (!locationFromDb)
                {
                    return BadRequest("Invalid LocationId provided.");
                }

                var departmentFromDb = await _unitOfWork.Departments.AnyAsync(d => d.Id == request.DepartmentId);
                if (!departmentFromDb)
                {
                    return BadRequest("Invalid DepartmentId provided.");
                }

                //we first map the incoming request dto to the entity
                var job = _mapper.Map<Jobs>(request);

                // set the PostedDate to now and ClosingDate to PostedDate
                job.PostedDate = job.ClosingDate;

                //here we will generate the unique Job code for every new entry
                job.Code = await _unitOfWork.Jobs.GenerateJobCodeAsync();

                await _unitOfWork.Jobs.AddAsync(job);
                await _unitOfWork.SaveChangesAsync();

                var jobUrl = $"{Request.Scheme}://{Request.Host}/api/v1/jobs/{job.Id}";
                return Created(jobUrl, new { jobUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating job");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        
        /// <summary>
        /// Retrieves a paginated list of jobs based on the specified filtering criteria.
        /// </summary>
        /// <remarks>This method supports filtering by job title, location, and department, as well as
        /// pagination. The results are ordered by the job's posted date in descending order.</remarks>
        /// <param name="request">The request object containing filtering and pagination parameters. <para> <see
        /// cref="JobListRequestDto.PageNo"/> and <see cref="JobListRequestDto.PageSize"/> must be greater than zero.
        /// </para> <para> Optional filters include <see cref="JobListRequestDto.Q"/> for job title search,  <see
        /// cref="JobListRequestDto.LocationId"/> for location filtering, and  <see
        /// cref="JobListRequestDto.DepartmentId"/> for department filtering. </para></param>
        /// <returns>An <see cref="IActionResult"/> containing a <see cref="JobListResponseDto"/> object. The response includes
        /// the total number of jobs matching the criteria and a paginated list of job details.</returns>
        [HttpPost("list")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobListResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListJobs([FromBody] JobListRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Job list request cannot be null.");
                }
                if (request.PageNo <= 0 || request.PageSize <= 0)
                {
                    return BadRequest("PageNo and PageSize must be greater than zero.");
                }
                var jobList = await _unitOfWork.Jobs.GetAllAsync(
                    include: j => j
                    .Include(j => j.Locations)
                    .Include(j => j.Department)
                    );

                // Search Filters
                if (!string.IsNullOrWhiteSpace(request.Q))
                    jobList = jobList.Where(j => j.Title.Contains(request.Q));

                if (request.LocationId.HasValue && request.LocationId.Value != 0)
                    jobList = jobList.Where(j => j.LocationId == request.LocationId.Value);

                if (request.DepartmentId.HasValue && request.DepartmentId.Value != 0)
                    jobList = jobList.Where(j => j.DepartmentId == request.DepartmentId.Value);

                var total = jobList.Count();

                var jobs = jobList
                    .OrderByDescending(j => j.PostedDate)
                    .Skip((request.PageNo - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(j => new JobListItemDto
                    {
                        Id = j.Id,
                        Code = j.Code,
                        Title = j.Title,
                        Location = j.Locations.Title,
                        Department = j.Department.Title,
                        PostedDate = j.PostedDate,
                        ClosingDate = j.ClosingDate
                    }).ToList();

                return Ok(new JobListResponseDto
                {
                    Total = total,
                    Data = jobs
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while listing jobs.");
                return StatusCode(500, "An internal error occurred while processing your request.");
            }
        }
        /// <summary>
        /// Updates an existing job with the specified ID using the provided data.
        /// </summary>
        /// <remarks>This method validates the existence of the job, location, and department before
        /// applying updates. If the validation fails, appropriate error responses are returned.</remarks>
        /// <param name="request">The data transfer object containing the updated job details. Must include valid  <see
        /// cref="JobsRequestDto.LocationId"/> and <see cref="JobsRequestDto.DepartmentId"/> values.</param>
        /// <param name="id">The unique identifier of the job to be updated.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation: <list type="bullet">
        /// <item><description><see cref="StatusCodes.Status200OK"/> if the job was successfully
        /// updated.</description></item> <item><description><see cref="StatusCodes.Status404NotFound"/> if no job with
        /// the specified ID exists.</description></item> <item><description><see
        /// cref="StatusCodes.Status400BadRequest"/> if the provided <paramref name="request"/> contains invalid
        /// data.</description></item> <item><description><see cref="StatusCodes.Status500InternalServerError"/> if an
        /// unexpected error occurs during the update process.</description></item> </list></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateJobs([FromBody] JobsRequestDto request, int id)
        {
            try
            {
                //validation checks to make sure our data is consistent
                if (request == null)
                {
                    return BadRequest("Job request cannot be null.");
                }
                if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description))
                {
                    return BadRequest("Job title and description are required.");
                }
                if (request.LocationId <= 0 || request.DepartmentId <= 0)
                {
                    return BadRequest("Valid LocationId and DepartmentId are required.");
                }

                //fetching the job from the database to update
                var jobFromDb = await _unitOfWork.Jobs.GetAsync(j => j.Id == id);
                if (jobFromDb == null)
                    return NotFound();

                //verifying if the new locationId is existing in the db
                var locationFromDb = await _unitOfWork.Locations.AnyAsync(l => l.Id == request.LocationId);
                if (!locationFromDb)
                {
                    return BadRequest("Invalid LocationId provided.");
                }

                //verifying if the new departmentId is existing in the db
                var departmentFromDb = await _unitOfWork.Departments.AnyAsync(d => d.Id == request.DepartmentId);
                if (!departmentFromDb)
                {
                    return BadRequest("Invalid DepartmentId provided.");
                }

                // Map updated values onto existing entity
                _mapper.Map(request, jobFromDb);

                jobFromDb.PostedDate = DateTime.Now;

                _unitOfWork.Jobs.Update(jobFromDb);
                await _unitOfWork.SaveChangesAsync();

                return Ok("OK");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating job with ID {Id}", id);
                return StatusCode(500, "An internal error occurred.");
            }
        }
        /// <summary>
        /// Retrieves detailed information about a job based on its unique identifier.
        /// </summary>
        /// <remarks>This method performs a lookup for a job by its ID and includes related data such as
        /// locations and department. If the job is found, the response includes a mapped DTO containing the job's
        /// details.</remarks>
        /// <param name="id">The unique identifier of the job to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing a <see cref="JobDetailsResponseDto"/> with detailed job
        /// information  if the job is found, or a 404 Not Found response if the job does not exist.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobDetailsResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetJobDetails(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid job ID provided.");
                var job = await _unitOfWork.Jobs.GetAsync(
                    j => j.Id == id,
                    include: query => query
                        .Include(j => j.Locations)
                        .Include(j => j.Department)
                );

                if (job == null)
                    return NotFound();

                var dto = _mapper.Map<JobDetailsResponseDto>(job);
                dto.Location = _mapper.Map<LocationDto>(job.Locations);
                dto.Department = _mapper.Map<DepartmentDto>(job.Department);

                return Ok(dto);

            }
            catch (Exception)
            {
                _logger.LogError("An error occurred while retrieving job details for ID {Id}", id);
                return StatusCode(500, "An internal error occurred while processing your request.");
            }
        }

        #endregion Jobs CRUD Operations
    }
}
