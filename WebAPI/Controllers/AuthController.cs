using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserForRegisterDto register)
        {
            var user = _authService.Register(register);
            if (!user.Success) return BadRequest(user.Message);

            var token = _authService.CreateAccessToken(user.Data);
            if (!token.Success) return BadRequest(token.Message);

            return Ok(token);
        }

        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto login)
        {
            var user = _authService.Login(login);
            if (!user.Success) return BadRequest(user.Message);

            var token = _authService.CreateAccessToken(user.Data);
            if (!token.Success) return BadRequest(token.Message);

            return Ok(token);
        }
    }
}
