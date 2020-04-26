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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CLIENT.Controllers
{
    public class AccountsController : Controller
    {
        int statusCode = 400;

        public IActionResult Index()
        {
            string token = HttpContext.Session.GetString("UserToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                var readToken = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.FirstOrDefault(x => x.Type.Equals("role")).Value;
                if (readToken.Equals("Admin"))
                {
                    return RedirectToAction("Index", "Departments", null, null);
                }
            }
            return View();
        }

        public JsonResult Login(LoginVM loginVM)
        {
            string token = null;
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44370/api/")
            };
            var myContent = JsonConvert.SerializeObject(loginVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var responseTask = client.PostAsync("Accounts/Login/", byteContent).Result;
            if (responseTask.IsSuccessStatusCode)
            {
                token = responseTask.Content.ReadAsStringAsync().Result.ToString();
                HttpContext.Session.SetString("UserToken", token);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    statusCode = 200;
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return Json(statusCode);
        }

        public IActionResult Unauthorize()
        {
            return View();
        }
    }
}