using APIStarted;
using APIStarted.Models;
using APIStarted.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace APIStarted.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly MembersService _membersService;

        public MembersController(MembersService membersService)
        {
            _membersService = membersService;
        }

        [HttpGet]
        public ActionResult<JSONMessage<Members>> Get()
        {
            JSONMessage<Members> json = new JSONMessage<Members>();
            try
            {
                json.Message = "Query Success";
                json.Code = true;
                json.Data = _membersService.Get();
            }
            catch (System.Exception ex)
            {
                json.Message = ex.Message;
                json.Code = false;
            }
            return json;
        }

        [HttpGet("{id:length(24)}", Name = "GetMembers")]
        public ActionResult<JSONMessage<Members>> Get(string id)
        {
            JSONMessage<Members> json = new JSONMessage<Members>();
            try
            {
                json.Message = "Query Success";
                json.Code = true;
                json.Data = _membersService.Get(id);
            }
            catch (System.Exception ex)
            {
                json.Message = ex.Message;
                json.Code = false;
            }
            return json;
        }

        [HttpPost]
        public ActionResult<Members> Create(Members members)
        {
            _membersService.Create(members);

            return CreatedAtRoute("GetMembers", new { id = members.Id.ToString() }, members);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Members membersIn)
        {
            var membersQuery = _membersService.Get(id);

            if (membersQuery == null)
            {
                return NotFound();
            }

            _membersService.Update(id, membersIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var membersQuery = _membersService.GetToRemove(id);

            if (membersQuery == null)
            {
                return NotFound();
            }

            _membersService.Remove(membersQuery.Id);

            return NoContent();
        }
    }
}