using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getuserdtobyid")]
        public IActionResult GetUserDtoById(int id)
        {
            var result = _userService.GetUserDtoById(id);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
