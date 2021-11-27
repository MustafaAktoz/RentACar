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
    public class RentalsController : ControllerBase
    {
        IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPost("add")]
        public IActionResult Add(RentDto rentDto)
        {
            var result=_rentalService.Add(rentDto.Rental,rentDto.Payment);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);            
        }
        [HttpPost("update")]
        public IActionResult Update(Rental rental)
        {
            var result = _rentalService.Update(rental);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _rentalService.GetById(id);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("getrentaldetails")]
        public IActionResult GetRentalDetails()
        {
            var result = _rentalService.GetRentalDetails();
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("rulesforadd")]
        public IActionResult RulesForAdd(Rental rental)
        {
            var result = _rentalService.RulesForAdd(rental);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
