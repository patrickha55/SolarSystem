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
    public class BodiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BodiesController> _logger;
        private readonly IMapper _mapper;

        public BodiesController(IUnitOfWork unitOfWork, ILogger<BodiesController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Bodies
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#nullable enable
        public async Task<ActionResult<IEnumerable<BodyDTO>>> Get([FromQuery] PaginationParam? request)
        {
            if (request is null)
                request = new();
            else if (request.PageNumber < 1 || request.PageSize < 1)
            {
                _logger.LogError($"Invalid request in {nameof(Get)}");
                return BadRequest($"Invalid Page Size or Page Number. Please try again.");
            }

            var bodies = await _unitOfWork.Bodies.GetAllAsync(request);

            if (bodies is null)
            {
                _logger.LogError("No data in bodies in {MethodName}", nameof(Get));
                return NotFound("There is no data at this moment");
            }

            var bodiesDto = _mapper.Map<IEnumerable<BodyDTO>>(bodies);

            return Ok(bodiesDto);
        }
#nullable disable

        // GET: api/Bodies/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BodyDetailDTO>> Get(int id)
        {
            if (id < 1)
            {
                _logger.LogError("Invalid request in {MethodName}: {Id}", nameof(Get), id);
                return BadRequest("Invalid id. Please try again!");
            }

            var body = await _unitOfWork.Bodies.GetAsync(c => c.Id == id, new List<string>() {"Component", "Region"});

            if (body is null)
            {
                _logger.LogError("No body with the provided ID in {MethodName}: {Id}", nameof(Get), id);
                return NotFound($"There is no body with the request id of {id}");
            }

            var bodyDetailDto = _mapper.Map<BodyDetailDTO>(body);

            return Ok(bodyDetailDto);
        }

        // POST: api/Bodies
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BodyDTO>> Post([FromBody] ManageBodyDTO request)
        {
            if (request is null || !ModelState.IsValid)
            {
                _logger.LogError("Invalid request in {MethodName}: {@Request}", nameof(Post), request);
                return BadRequest("Invalid request. Please try again!");
            }

            var body = _mapper.Map<Body>(request);
            body.CreatedAt = DateTime.Now;
            body.UpdatedAt = DateTime.Now;

            await _unitOfWork.Bodies.CreateAsync(body);
            await _unitOfWork.SaveAsync();

            var bodyDto = _mapper.Map<BodyDTO>(body);

            return CreatedAtAction(nameof(Get), new {id = bodyDto.Id}, bodyDto);
        }

        // PUT: api/Bodies/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] ManageBodyDTO request)
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

            var body = await _unitOfWork.Bodies.GetAsync(r => r.Id == id);

            if (body is null)
            {
                _logger.LogError("No body with the provided ID in {MethodName}: {Id}", nameof(Put), id);
                return NotFound($"There is no body with the request id of {id}");
            }

            var updatedBody = _mapper.Map(request, body);

            _unitOfWork.Bodies.Update(updatedBody);

            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/Bodies/5
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

            var body = await _unitOfWork.Bodies.GetAsync(r => r.Id == id);

            if (body is null)
            {
                _logger.LogError("No body with the provided ID in {MethodName}: {Id}", nameof(Delete), id);
                return NotFound($"There is no body with the request id of {id}");
            }

            await _unitOfWork.Bodies.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
