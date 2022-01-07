using APIStarted;
using APIStarted.Models;
using APIStarted.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;

namespace APIStarted.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     public class LoginControllers : ControllerBase
    {
        private readonly MembersService _membersService;
        private IConfiguration _config;
        private SymmetricSecurityKey _key;

        public LoginControllers(MembersService membersService, IConfiguration config)
        {
            _membersService = membersService;
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        }

        [HttpPost]
        public ActionResult<JSONMessage<Account>> Login(Account account)
        {
            JSONMessage<Account> json = new JSONMessage<Account>();
            ActionResult res = Unauthorized();
            try
            {
                if (_membersService.Login(account) == true)
                {
                    var token = GenerateJSONWebToken(account);
                    json.Message = "Login Success";
                    json.Code = true;
                    json.Token = token;
                }
                else
                {
                    json.Message = "Login Fail";
                    json.Code = true;
                    json.Token = res.ToString();
                }
            }
            catch (System.Exception ex)
            {
                json.Message = ex.Message;
                json.Code = false;
            }
            return json;
        }

        private string GenerateJSONWebToken (Account account)
        {
            var securityKey = _key;
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var id = _membersService.FindAccount(account.Username);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Username),
                new Claim(JwtRegisteredClaimNames.Sub, account.Password),
                new Claim(JwtRegisteredClaimNames.NameId, id[0].Id),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }

        private string DecodeToken (string token)
        {
            // var handler = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters() {
            //     IssuerSigningKey = _key,
            //     ValidIssuer =  _config["Jwt:Issuer"],
            //     ValidateIssuer = true,
            //     ValidAudience =  _config["Jwt:Issuer"],
            //     ValidateAudience = true
            // }, out SecurityToken sToken);

            // var stream = token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            var jti = tokenS.Claims.First(claim => claim.Type == "nameid").Value;
            return jti;
        }

        [Authorize]
        [HttpPost("Post")]
        public string Post()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var userName = claim[0].Value;
            return "Welcome to " + userName;
        }

        [Authorize]
        [HttpGet("GetValue")]
        // public ActionResult<IEnumerable<string>> Get() 
        public string Get() 
        {
            object obj = new object();
            // var a = DecodeToken("").Claims.First(x => x.Type == ClaimTypes.Name).Value;
            var accessToken = Request.Headers[HeaderNames.Authorization];
            var accessTokenBearer = accessToken.ToString().Replace("Bearer ", string.Empty);
            var b = HttpContext.User.Identity;
            var a = DecodeToken(accessTokenBearer);
            // obj = a;
            // return new string[] { "Value 1", "Value 2", "Value 3" };
            // var data = obj.
            var data = a;
            obj = a;
            return obj.ToString();
        }
    }
}