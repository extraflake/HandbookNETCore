using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Interface
{
    public interface IService<T> where T : class
    {
        List<IActionResult> Get();
        IActionResult Get(int id);
        IActionResult Post(T entity);
        IActionResult Put(T entity);
        IActionResult Delete(int id);
    }
}
