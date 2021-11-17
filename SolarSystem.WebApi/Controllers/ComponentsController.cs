using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarSystem.Data.DTOs;
using SolarSystem.Data.Entities;
using SolarSystem.Repository.IRepository;

namespace SolarSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ComponentsController> _logger;
        private readonly IMapper _mapper;

        public ComponentsController(IUnitOfWork unitOfWork, ILogger<ComponentsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Components
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#nullable enable
        public async Task<ActionResult<IEnumerable<ComponentDTO>>> Get([FromQuery] PaginationParam? request)
        {
            if (request is null)
                request = new();
            else if (request.PageNumber < 1 || request.PageSize < 1)
            {
                _logger.LogError($"Invalid request in {nameof(Get)}");
                return BadRequest($"Invalid Page Size or Page Number. Please try again.");
            }

            var components = await _unitOfWork.Components.GetAllAsync(request);

            if (components is null)
            {
                _logger.LogError("No data in components in {MethodName}", nameof(Get));
                return NotFound("There is no data at this moment");
            }

            var componentDtos = _mapper.Map<IEnumerable<ComponentDTO>>(components);

            return Ok(componentDtos);
        }
#nullable disable

        // GET: api/Components/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ComponentDetailDTO>> Get(int id)
        {
            if (id < 1)
            {
                _logger.LogError("Invalid request in {MethodName}: {Id}", nameof(Get), id);
                return BadRequest("Invalid id. Please try again!");
            }

            var component = await _unitOfWork.Components.GetAsync(c => c.Id == id, new List<string>() {"Bodies"});

            if (component is null)
            {
                _logger.LogError("No region with the provided ID in {MethodName}: {Id}", nameof(Get), id);
                return NotFound($"There is no region with the request id of {id}");
            }

            var componentDetailDto = _mapper.Map<ComponentDetailDTO>(component);

            return Ok(componentDetailDto);
        }

        // POST: api/Components
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ComponentDTO>> Post([FromBody] CreateComponentDTO request)
        {
            if (request is null || !ModelState.IsValid)
            {
                _logger.LogError("Invalid request in {MethodName}: {@Request}", nameof(Post), request);
                return BadRequest("Invalid request. Please try again!");
            }

            var component = _mapper.Map<Component>(request);
            component.CreatedAt = DateTime.Now;
            component.UpdatedAt = DateTime.Now;

            await _unitOfWork.Components.CreateAsync(component);
            await _unitOfWork.SaveAsync();

            var componentDto = _mapper.Map<ComponentDTO>(component);

            return CreatedAtAction(nameof(Get), new {id = componentDto.Id}, componentDto);
        }

        // PUT: api/Components/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateComponentDTO request)
        {
            if (id < 1)
            {
                _logger.LogError("Invalid request in {MethodName}: {Id}", nameof(Put), id);
                return BadRequest("Invalid id. Please try again!");
            }

            if (request is null || !ModelState.IsValid)
            {
                _logger.LogError("Invalid request in {MethodName}: {@Request}", nameof(Put), request);
                return BadRequest("Invalid request. Please try again!");
            }

            var component = await _unitOfWork.Components.GetAsync(r => r.Id == id);

            if (component is null)
            {
                _logger.LogError("No region with the provided ID in {MethodName}: {Id}", nameof(Put), id);
                return NotFound($"There is no region with the request id of {id}");
            }

            var updatedComponent = _mapper.Map(request, component);

            _unitOfWork.Components.Update(updatedComponent);

            if (request.BodiesId is not null)
            {
                foreach (var bodyId in request.BodiesId)
                {
                    if (bodyId < 1)
                    {
                        _logger.LogError("No body with the provided ID in {MethodName}: {Id}", nameof(Post), id);
                        return NotFound($"There is no region with the request id of {id}");
                    }

                    var body = await _unitOfWork.Bodies.GetAsync(b => b.Id == bodyId);

                    if (body is null)
                    {
                        _logger.LogError("No body with the provided ID in {Method}: {Id}", nameof(Put), bodyId);
                        return NotFound($"There is no body with the request id of {bodyId}");
                    }

                    body.ComponentId = id;
                    _unitOfWork.Bodies.Update(body);
                }
            }

            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/Components/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id < 1)
            {
                _logger.LogError("Invalid request in {MethodName}: {Id}", nameof(Delete), id);
                return BadRequest("Invalid id. Please try again!");
            }

            var component = await _unitOfWork.Components.GetAsync(r => r.Id == id);

            if (component is null)
            {
                _logger.LogError("No region with the provided ID in {MethodName}: {Id}", nameof(Delete), id);
                return NotFound($"There is no region with the request id of {id}");
            }

            await _unitOfWork.Components.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}