using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarSystem.Data.DTOs;
using SolarSystem.Data.Entities;
using SolarSystem.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegionsController> _logger;
        private readonly IMapper _mapper;

        public RegionsController(IUnitOfWork unitOfWork, ILogger<RegionsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/<RegionsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<RegionDTO>>> Get()
        {
            var regions = await _unitOfWork.Regions.GetAllAsync();

            if (regions is null) NoData();

            var result = _mapper.Map<IEnumerable<RegionDTO>>(regions);

            return Ok(result);
        }

        // GET api/<RegionsController>/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegionDetailDTO>> Get(int id)
        {
            if(id < 1) InvalidId(id, "Get");

            var region = await _unitOfWork.Regions.GetAsync(r => r.Id == id, includes: new List<string> { "Bodies" });

            if (region is null) NoRegionFound(id);

            var result = _mapper.Map<RegionDetailDTO>(region);

            return Ok(result);
        }

        // POST api/<RegionsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegionDTO>> Post([FromBody] CreateRegionDTO request)
        {
            if (request is null || !ModelState.IsValid)
            {
                _logger.LogError("Invalid request in {MethodName}: {@Request}", nameof(Post), request);
                return BadRequest("Invalid request. Please try again!");
            }

            var region = _mapper.Map<Region>(request);

            region.CreatedAt = DateTime.Now;
            region.UpdatedAt = DateTime.Now;

            await _unitOfWork.Regions.CreateAsync(region);
            await _unitOfWork.SaveAsync();

            var regionDto = _mapper.Map<RegionDTO>(region);

            return CreatedAtAction(nameof(Get), new { id = regionDto.Id }, regionDto);
        }

        // PUT api/<RegionsController>/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateRegionDTO request)
        {
            if (id < 1) InvalidId(id, "Put");

            if (request is null || !ModelState.IsValid)
            {
                _logger.LogError("Invalid request in {MethodName}: {@Request}", nameof(Put), request);
                return BadRequest("Invalid request. Please try again!");
            }

            var region = await _unitOfWork.Regions.GetAsync(r => r.Id == id);

            if (region is null) NoRegionFound(id);

            var updatedRegion = _mapper.Map(request, region);

            _unitOfWork.Regions.Update(updatedRegion);

            if (request.BodiesId is not null)
            {
                foreach (var bodyId in request.BodiesId)
                {
                    if (bodyId < 1)
                    {
                        _logger.LogError($"Invalid request in {nameof(Put)}: {request.BodiesId}");
                        return BadRequest("Invalid body id. Please try again!");
                    }

                    var body = await _unitOfWork.Bodies.GetAsync(b => b.Id == bodyId);

                    if (body is null)
                    {
                        _logger.LogError("No body with the provided ID in {Method}: {Id}", nameof(Put), bodyId);
                        return NotFound($"There is no body with the request id of {bodyId}");
                    }

                    body.RegionId = id;
                    _unitOfWork.Bodies.Update(body);
                }
            }

            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE api/<RegionsController>/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id < 1) InvalidId(id, "Delete");

            var region = await _unitOfWork.Regions.GetAsync(r => r.Id == id);

            if (region is null) NoRegionFound(id);

            await _unitOfWork.Regions.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
        
        private ActionResult<IEnumerable<RegionDTO>> NoData()
        {
            _logger.LogError("No data in regions in {MethodName}", nameof(Get));
            return NotFound("There is no data at this moment");
        }
        
        private ActionResult<RegionDetailDTO> NoRegionFound(int id)
        {
            _logger.LogError("No region with the provided ID in {MethodName}: {Id}", nameof(Get), id);
            return NotFound($"There is no region with the request id of {id}");
        }

        private ActionResult<RegionDetailDTO> InvalidId(int id, string methodName)
        {
            _logger.LogError("Invalid request in {MethodName}: {Id}", nameof(methodName), id);
            return BadRequest("Invalid id. Please try again!");
        }
    }
}
