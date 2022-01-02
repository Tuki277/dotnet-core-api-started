using APIStarted;
using APIStarted.Models;
using APIStarted.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace APIStarted.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     public class LoginControllers : ControllerBase
    {
        private readonly MembersService _membersService;

        public LoginControllers(MembersService membersService)
        {
            _membersService = membersService;
        }

        [HttpPost]
        public ActionResult<JSONMessage<Account>> Login(Account account)
        {
            JSONMessage<Account> json = new JSONMessage<Account>();
            try
            {
                if (_membersService.Login(account) == true)
                {
                    json.Message = "Login Success";
                    json.Code = true;
                }
                else 
                {
                    json.Message = "Login Fail";
                    json.Code = true;
                }
            }
            catch (System.Exception ex)
            {
                json.Message = ex.Message;
                json.Code = false;
            }
            return json;
        }
    }
}