using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("token")]
        public ActionResult GetToken()
        {
            string securityKey = "this_is_security_key_longest_i_have_ever_been_written_before";

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: "bootcampresourcemanagement",
                audience: "readers",
                expires: DateTime.Now.AddYears(1000),
                signingCredentials: signingCredentials);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
