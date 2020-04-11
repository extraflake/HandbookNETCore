using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLIENT.ViewModels
{
    public class DepartmentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DepartmentJson
    {
        [JsonProperty("data")]
        public IList<DepartmentVM> data { get; set; }
    }
}
