using Business.Abstract;
using Entities.Concrete;
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
    public class ColorsController : ControllerBase
    {
        IColorService _colorService;

        public ColorsController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpPost("add")]
        public IActionResult Add(Color color)
        {
            var result=_colorService.Add(color);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("update")]
        public IActionResult Update(Color color)
        {
            var result = _colorService.Update(color);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("delete")]
        public IActionResult Delete(Color color)
        {
            var result = _colorService.Delete(color);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _colorService.GetAll();
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _colorService.GetById(id);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
