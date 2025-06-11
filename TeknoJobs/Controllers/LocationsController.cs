using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknoJobs.Application.DTOs;
using TeknoJobs.Domain.Entities;
using TeknoJobs.Domain.Interfaces;

namespace TeknoJobs.Controllers
{
    [ApiController]
    [Route("api/v1/locations")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]

    public class LocationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LocationsController> _logger;
        private readonly IMapper _mapper;

        public LocationsController(IUnitOfWork unitOfWork, ILogger<LocationsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        #region Locations CRUD Operations

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLocation([FromBody] LocationsRequestDto request)
        {
            try
            {
                var location = _mapper.Map<Locations>(request);

                await _unitOfWork.Locations.AddAsync(location);
                await _unitOfWork.SaveChangesAsync();

                var locationUrl = $"{Request.Scheme}://{Request.Host}/api/v1/locations/{location.Id}";

                return Created(locationUrl, new { locationUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating location");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllLocations()
        {
            var location = await _unitOfWork.Locations.GetAllAsync();

            if (location == null)
                return NotFound();

            var dto = _mapper.Map<IEnumerable<LocationsResponseDto>>(location);
            return Ok(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLocation([FromBody] LocationsRequestDto request, int id)
        {
            try
            {
                var locationFromDb = await _unitOfWork.Locations.GetAsync(l => l.Id == id);
                if (locationFromDb == null)
                    return NotFound();

                _mapper.Map(request, locationFromDb); // map onto existing entity
                _unitOfWork.Locations.Update(locationFromDb);
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
