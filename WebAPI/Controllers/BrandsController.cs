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
    public class BrandsController : ControllerBase
    {
        IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost("add")]
        public IActionResult Add(Brand brand)
        {
            var result=_brandService.Add(brand);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("update")]
        public IActionResult Update(Brand brand)
        {
            var result = _brandService.Update(brand);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("delete")]
        public IActionResult Delete(Brand brand)
        {
            var result = _brandService.Delete(brand);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _brandService.GetAll();
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _brandService.GetById(id);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
