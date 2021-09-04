using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
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
    public class CarImagesController : ControllerBase
    {
        ICarImageService _carImageService;

        public CarImagesController(ICarImageService carImageService)
        {
            _carImageService = carImageService;
        }

        [HttpPost("add")]
        public IActionResult Add(int carId, [FromForm] IFormFile file)
        {
            var result = _carImageService.Add(carId, file);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("delete")]
        public IActionResult Delete(int id)
        {
            var result = _carImageService.Delete(id);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("update")]
        public IActionResult Update(int id,[FromForm]IFormFile file)
        {
            var result = _carImageService.Update(id,file);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("getbycarid")]
        public IActionResult GetByCarId(int carId)
        {
            var result = _carImageService.GetByCarId(carId);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
