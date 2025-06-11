using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TeknoJobs.Application.DTOs;
using TeknoJobs.Domain.Entities;
using TeknoJobs.Domain.Interfaces;

namespace TeknoJobs.Controllers
{
    /// <summary>
    /// Provides endpoints for managing department entities, including creating, retrieving, and updating departments.
    /// </summary>
    /// <remarks>This controller handles CRUD operations for department entities. It includes methods for
    /// creating new departments, retrieving all departments, and updating existing departments. All endpoints require
    /// authorization and are exposed under the route "api/v1/departments".</remarks>
    [ApiController]
    [Route("api/v1/departments")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DepartmentsController> _logger;
        private readonly IMapper _mapper;

        public DepartmentsController(IUnitOfWork unitOfWork, ILogger<DepartmentsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        #region Department CRUD Operations
        /// <summary>
        /// Creates a new department based on the provided request data.
        /// </summary>
        /// <remarks>This method maps the provided request data to a department entity, adds it to the
        /// database,  and commits the changes. The response includes the location URL of the created
        /// department.</remarks>
        /// <param name="request">The data for the department to be created. This must include all required fields for department creation.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns a 201 Created response with
        /// the location URL of the newly created department  if successful, a 400 Bad Request response if the input is
        /// invalid, or a 500 Internal  Server Error response if an unexpected error occurs.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentsRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Department request cannot be null.");
                }
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return BadRequest("Department title is required.");
                }
                var department = _mapper.Map<Departments>(request);

                await _unitOfWork.Departments.AddAsync(department);
                await _unitOfWork.SaveChangesAsync();

                var locationUrl = $"{Request.Scheme}://{Request.Host}/api/v1/departments/{department.Id}";

                return Created(locationUrl, new { locationUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating department");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Retrieves all departments from the data source.
        /// </summary>
        /// <remarks>This method returns a list of departments as a collection of DTOs. If no departments
        /// are found,  a 200 Ok response is returned with an empty list. The response is formatted as JSON.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a 200 OK response with a collection of <see
        /// cref="DepartmentsResponseDto"/> objects representing the departments, or a 404 Not Found response if no
        /// departments exist.</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departmentList = await _unitOfWork.Departments.GetAllAsync();

            if (departmentList == null)
                return NotFound();

            var dto = _mapper.Map<IEnumerable<DepartmentsResponseDto>>(departmentList);
            return Ok(dto);
        }

        /// <summary>
        /// Updates an existing department with the specified ID using the provided data.
        /// </summary>
        /// <remarks>This method retrieves the department by its ID, updates its properties using the
        /// provided data,  and saves the changes to the database. If the department does not exist, a 404 response is
        /// returned.</remarks>
        /// <param name="request">The data transfer object containing the updated department information.</param>
        /// <param name="id">The unique identifier of the department to update.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns <see
        /// cref="StatusCodes.Status200OK"/> if the update is successful,  <see cref="StatusCodes.Status404NotFound"/>
        /// if the department with the specified ID is not found,  or <see
        /// cref="StatusCodes.Status500InternalServerError"/> if an unexpected error occurs.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentsRequestDto request, int id)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Department request cannot be null.");
                }
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return BadRequest("Department title is required.");
                }

                var departmentFromDb = await _unitOfWork.Departments.GetAsync(d => d.Id == id);
                if (departmentFromDb == null)
                    return NotFound();

                _mapper.Map(request, departmentFromDb); // map onto existing entity

                _unitOfWork.Departments.Update(departmentFromDb);
                await _unitOfWork.SaveChangesAsync();

                return Ok("OK");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating department");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        #endregion
    }
}
