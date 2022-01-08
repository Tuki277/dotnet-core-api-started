using APIStarted;
using APIStarted.Models;
using APIStarted.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace APIStarted.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentsService _departmentService;

        public DepartmentController(DepartmentsService departmentsService)
        {
            _departmentService = departmentsService;
        }

        [HttpGet]
        public ActionResult<JSONMessage<Departments>> Get()
        {
            JSONMessage<Departments> json = new JSONMessage<Departments>();
            try
            {
                json.Message = "Query Success";
                json.Code = true;
                json.Data = _departmentService.Get();
            }
            catch (System.Exception)
            {
                json.Message = "Query Fail";
                json.Code = false;
            }
            return json;
        }

        [HttpGet("{id:length(24)}", Name = "GetDepartment")]
        public ActionResult<JSONMessage<Departments>> Get(string id)
        {
            JSONMessage<Departments> json = new JSONMessage<Departments>();
            try
            {
                json.Message = "Query Success";
                json.Code = true;
                json.Data = _departmentService.Get(id);
            }
            catch (System.Exception)
            {
                json.Message = "Query Fail";
                json.Code = false;
            }
            return json;
        }

        [HttpPost]
        public ActionResult<Departments> Create(Departments departments)
        {
            _departmentService.Create(departments);

            return CreatedAtRoute("GetDepartment", new { id = departments.Id.ToString() }, departments);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Departments departmentIn)
        {
            var category = _departmentService.Get(id);

            if (category == null)
            {
                return NotFound();
            }

            _departmentService.Update(id, departmentIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var departmentsQuery = _departmentService.GetToRemove(id);

            if (departmentsQuery == null)
            {
                return NotFound();
            }

            _departmentService.Remove(departmentsQuery.Id);

            return NoContent();
        }
    }
}