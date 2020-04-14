using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CLIENT.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Login(LoginVM loginVM)
        {
            AccountVM accountVM = null;
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44370/api/")
            };
            //client.DefaultRequestHeaders.Add("Authorization", "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjMzMTQzNDAzMDgyLCJpc3MiOiJib290Y2FtcHJlc291cmNlbWFuYWdlbWVudCIsImF1ZCI6InJlYWRlcnMifQ.DCIMFDIIFIYK41ORntIMyaJxHL093cPWDK6JhUN2vew");
            var myContent = JsonConvert.SerializeObject(loginVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var responseTask = client.PostAsync("Accounts/Login/", byteContent).Result;
            if (responseTask.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(responseTask.Content.ReadAsStringAsync().Result).ToString();
                accountVM = JsonConvert.DeserializeObject<AccountVM>(json);
                GenerateToken(accountVM);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return Json(new { result = accountVM });
        }

        public string GenerateToken(AccountVM accountVM)
        {
            string key = "this_is_security_key_longest_i_have_ever_been_written_before";   
            var issuer = "http://bootcamponline.com/"; 
            var audience = "http://bootcamponline.com/";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
   
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim("id", accountVM.Id));
            claims.Add(new Claim("name", accountVM.Name));
            claims.Add(new Claim("email", accountVM.Email));
            claims.Add(new Claim("role", accountVM.Role));
 
            var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.Now.AddYears(1), signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }
    }
}