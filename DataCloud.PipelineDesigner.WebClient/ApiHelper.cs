using DataCloud.PipelineDesigner.WebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.WebClient
{
    public class ApiHelper
    {
        public static ApiResult CreateSuccessResult()
        {
            return new ApiResult { Success = true};
        }

        public static ApiResult CreateFailedResult(string errorMsg)
        {
            return new ApiResult{ Success = false, ErrorMessage = errorMsg };
        }

        public static ApiResult<T> CreateSuccessResult<T>(T data)
        {
            return new ApiResult<T> { Success = true, Data = data };
        }

        public static ApiResult<T> CreateFailedResult<T>(string errorMsg)
        {
            return new ApiResult<T> { Success = false, ErrorMessage = errorMsg };
        }
    }
}
