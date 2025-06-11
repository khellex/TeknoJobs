using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknoJobs.Application.DTOs;
using TeknoJobs.Domain.Entities;
using TeknoJobs.Domain.Interfaces;

namespace TeknoJobs.Controllers
{
    /// <summary>
    /// Provides endpoints for managing location entities, including creating, retrieving, and updating locations.
    /// </summary>
    /// <remarks>This controller handles CRUD operations for location entities. It includes methods for
    /// creating new locations, retrieving all locations, and updating existing locations. Each endpoint is secured with
    /// authorization and adheres to RESTful API conventions.</remarks>
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

        /// <summary>
        /// Creates a new location based on the provided request data.
        /// </summary>
        /// <remarks>This method maps the provided request data to a location entity, saves it to the
        /// database, and generates a URL for the created location. Ensure that the <paramref name="request"/> parameter
        /// contains valid data to avoid a 400 Bad Request response.</remarks>
        /// <param name="request">The data for the location to be created. This must include all required fields for a valid location.</param>
        /// <returns>A response indicating the result of the operation. If successful, returns a 201 Created status with the URL
        /// of the created location. If the request is invalid, returns a 400 Bad Request status. If an internal error
        /// occurs, returns a 500 Internal Server Error status.</returns>
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

        /// <summary>
        /// Retrieves all locations from the data source.
        /// </summary>
        /// <remarks>This method fetches all location entities, maps them to a collection of response
        /// DTOs, and returns them in the response. If no locations are found, a 200 Ok status is
        /// returned with an empty list.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of <see cref="LocationsResponseDto"/> objects with a
        /// 200 OK status if locations are found, or a 404 Not Found status if no locations exist.</returns>
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

        /// <summary>
        /// Updates the details of an existing location.
        /// </summary>
        /// <remarks>This method retrieves the location entity from the database using the specified
        /// <paramref name="id"/>.  If the location exists, the method updates its details using the provided <paramref
        /// name="request"/> object  and saves the changes to the database. If the location does not exist, a 404
        /// response is returned.</remarks>
        /// <param name="request">The updated location details provided in the request body.</param>
        /// <param name="id">The unique identifier of the location to be updated.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns <see
        /// cref="StatusCodes.Status200OK"/> if the update is successful,  <see cref="StatusCodes.Status404NotFound"/>
        /// if the location with the specified <paramref name="id"/> is not found,  or <see
        /// cref="StatusCodes.Status500InternalServerError"/> if an unexpected error occurs.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLocation([FromBody] LocationsRequestDto request, int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid location ID.");
                if (request == null)
                {
                    return BadRequest("Location request cannot be null");
                }

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
