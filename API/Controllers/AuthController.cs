using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using API.ViewModels;
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
        public string GetToken(Account account)
        {
            string securityKey = "this_is_security_key_longest_i_have_ever_been_written_before";

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim("id", account.Id));
            claims.Add(new Claim("name", account.Name));
            claims.Add(new Claim("email", account.Email));
            claims.Add(new Claim("role", account.Role));

            var token = new JwtSecurityToken(
                issuer: "https://localhost:44370/",
                audience: "https://localhost:44370/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: signingCredentials);

            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt_token;
        }
    }
}
