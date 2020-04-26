using API.Bases;
using API.Models;
using API.Repositories.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : BaseController<Department, DepartmentRepository>
    {
        public DepartmentsController(DepartmentRepository departmentRepository) : base(departmentRepository)
        {

        }
    }
}