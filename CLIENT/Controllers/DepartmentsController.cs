using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using API.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CLIENT.Controllers
{
    public class DepartmentsController : Controller
    {
public IActionResult Index()
        {
            string token = HttpContext.Session.GetString("UserToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                var readToken = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.FirstOrDefault(x => x.Type.Equals("role")).Value;
                if (readToken.Equals("Admin"))
                {
                    return View();
                }
            }
            return RedirectToAction("Unauthorize", "Accounts", null, null);
        }

        public JsonResult LoadData()
        {
            DepartmentJson departmentVM = null;
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44370/api/")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("UserToken").ToString());
            var responseTask = client.GetAsync("Departments");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                departmentVM = JsonConvert.DeserializeObject<DepartmentJson>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return Json(departmentVM);
        }

        public JsonResult LoadDataById(int id)
        {
            DepartmentVM departmentVM = null;
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44370/api/")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("UserToken").ToString());
            var responseTask = client.GetAsync("Departments/" + id);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                departmentVM = JsonConvert.DeserializeObject<DepartmentVM>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return Json(departmentVM);
        }

        public JsonResult DeleteData(int id)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44370/api/")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("UserToken").ToString());
            var result = client.DeleteAsync("Departments/" + id).Result;
            return Json(result);
        }

        public JsonResult SubmitData(DepartmentVM departmentVM)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44370/api/")
            };
            var myContent = JsonConvert.SerializeObject(departmentVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("UserToken").ToString());
            if (departmentVM.Id.Equals(0))
            {
                var result = client.PostAsync("Departments/", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("Departments/" + departmentVM.Id, byteContent).Result;
                return Json(result);
            }
        }
    }
}