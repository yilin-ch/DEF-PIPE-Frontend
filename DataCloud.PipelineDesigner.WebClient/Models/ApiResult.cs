using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.WebClient.Models
{
    public class ApiResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }        
    }

    public class ApiResult<T> : ApiResult
    {
        public T Data;
    }
}
