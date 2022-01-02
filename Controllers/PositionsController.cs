using APIStarted;
using APIStarted.Models;
using APIStarted.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LearnAPiDotNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly PositionsService _positionsService;

        public PositionsController(PositionsService positionsService)
        {
            _positionsService = positionsService;
        }

        [HttpGet]
        public ActionResult<JSONMessage<Positions>> Get()
        {
            JSONMessage<Positions> json = new JSONMessage<Positions>();
            try
            {
                json.Message = "Query Success";
                json.Code = true;
                json.Data = _positionsService.Get();
            }
            catch (System.Exception)
            {
                json.Message = "Query Fail";
                json.Code = false;
            }
            return json;
        }

        [HttpGet("{id:length(24)}", Name = "GetPosition")]
        public ActionResult<JSONMessage<Positions>> Get(string id)
        {
            JSONMessage<Positions> json = new JSONMessage<Positions>();
            try
            {
                json.Message = "Query Success";
                json.Code = true;
                json.Data = _positionsService.Get(id);
            }
            catch (System.Exception)
            {
                json.Message = "Query Fail";
                json.Code = false;
            }
            return json;
        }

        [HttpPost]
        public ActionResult<Positions> Create(Positions Positions)
        {
            _positionsService.Create(Positions);

            return CreatedAtRoute("GetPosition", new { id = Positions.Id.ToString() }, Positions);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Positions departmentIn)
        {
            var category = _positionsService.Get(id);

            if (category == null)
            {
                return NotFound();
            }

            _positionsService.Update(id, departmentIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var PositionsQuery = _positionsService.GetToRemove(id);

            if (PositionsQuery == null)
            {
                return NotFound();
            }

            _positionsService.Remove(PositionsQuery.Id);

            return NoContent();
        }
    }
}