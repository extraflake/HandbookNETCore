using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CLIENT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CLIENT.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult LoadData()
        {
            DepartmentJson departmentVM = null;
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44370/api/")
            };
            //client.DefaultRequestHeaders.Add("Authorization", "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjMzMTQzNDAzMDgyLCJpc3MiOiJib290Y2FtcHJlc291cmNlbWFuYWdlbWVudCIsImF1ZCI6InJlYWRlcnMifQ.DCIMFDIIFIYK41ORntIMyaJxHL093cPWDK6JhUN2vew");
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
            //client.DefaultRequestHeaders.Add("Authorization", "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjMzMTQzNDAzMDgyLCJpc3MiOiJib290Y2FtcHJlc291cmNlbWFuYWdlbWVudCIsImF1ZCI6InJlYWRlcnMifQ.DCIMFDIIFIYK41ORntIMyaJxHL093cPWDK6JhUN2vew");
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