using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknoJobs.Application.DTOs;
using TeknoJobs.Domain.Entities;
using TeknoJobs.Domain.Interfaces;

namespace TeknoJobs.Controllers
{
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

                var job = _mapper.Map<Jobs>(request);

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
        [HttpPost("list")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobListResponseDto))]
        public async Task<IActionResult> ListJobs([FromBody] JobListRequestDto request)
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

            // Filter
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
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateJobs([FromBody] JobsRequestDto request, int id)
        {
            try
            {
                var jobFromDb = await _unitOfWork.Jobs.GetAsync(j => j.Id == id);
                if (jobFromDb == null)
                    return NotFound();

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
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobDetailsResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetJobDetails(int id)
        {
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

        #endregion Jobs CRUD Operations
    }
}
