using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TeknoJobs.Application.DTOs;
using TeknoJobs.Domain.Entities;
using TeknoJobs.Domain.Interfaces;

namespace TeknoJobs.Controllers
{
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentsRequestDto request)
        {
            try
            {
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

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDepartments()
        {
            var department = await _unitOfWork.Departments.GetAllAsync();

            if (department == null)
                return NotFound();

            var dto = _mapper.Map<IEnumerable<DepartmentsResponseDto>>(department);
            return Ok(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentsRequestDto request, int id)
        {
            try
            {
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
