using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarSystem.Data.DTOs;
using SolarSystem.Data.Entities;
using SolarSystem.Services.AuthWithJwt;

namespace SolarSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountsController> _logger;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<User> userManager, IAuthManager authManager,
            ILogger<AccountsController> logger, IMapper mapper)
        {
            _userManager = userManager;
            _authManager = authManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost, Route("register")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] RegisterDTO request)
        {
            if (request is null || !ModelState.IsValid)
            {
                _logger.LogError("Invalid registration attempt. Please check at {MethodName} : {@Request}",
                    nameof(Register), request);
                return BadRequest("Invalid registration attempt. Please try again!");
            }

            var user = _mapper.Map<User>(request);
            
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                if (request.Roles is not null)
                {
                    var resultRole = await _userManager.AddToRolesAsync(user, request.Roles);
                    return resultRole.Succeeded ? NoContent() : BadRequest("User registration attempt failed.");
                }
            }

            return BadRequest("User registration attempt failed.");
        }

        [HttpPost, Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Login([FromBody] SignInDTO request)
        {
            if (request is null || !ModelState.IsValid)
            {
                _logger.LogError("Invalid login attempt. Please check at {MethodName} : {@Request}", nameof(Login),
                    request);
                return BadRequest("Invalid login attempt. Please try again!");
            }

            return await _authManager.ValidateUserAsync(request) ?
                Accepted(new {Token = await _authManager.CreateTokenAsync()})
                :
                Unauthorized("Email or Password is invalid. Please try again.");
        }
    }
}