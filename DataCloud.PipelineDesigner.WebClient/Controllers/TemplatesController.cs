using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.Repositories.Models;
using DataCloud.PipelineDesigner.Repositories.Services;
using DataCloud.PipelineDesigner.Services;
using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.WebClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.WebClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplatesController : ControllerBase
    {
        Services.Interfaces.ITemplateService templateService;

        public TemplatesController(ITemplateService services)
        {
            //TODO: Update this to use dependency injection
            templateService = services;
        }
        [HttpPost]
        public async Task<ApiResult<Template>> AddOrUpdateTemplateAsync([FromBody] Template template)
        {
            try
            {
                await templateService.AddOrUpdateTemplateAsync(template);

                return ApiHelper.CreateSuccessResult(template);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<Template>(e.Message);
            }
        }

        [HttpGet]
        public async Task<ApiResult<List<Template>>> GetAvailableTemplates()
        {
            try
            {
                var templates = await templateService.GetTemplatesAsync();
                return ApiHelper.CreateSuccessResult(templates);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<List<Template>>(e.Message);
            }
        }

        [HttpGet("{user}")]
        public async Task<ApiResult<List<Template>>> GetAvailableTemplates(String user)
        {
            try
            {
                User userDB = await templateService.GetTemplatesAsync(user);
                return ApiHelper.CreateSuccessResult(userDB.Templates);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<List<Template>>(e.Message);
            }
        }

        [HttpPost("{user}")]
        public async Task<ApiResult<Template>> AddOrUpdateTemplateAsync([FromBody] Template template, string user)
        {
            try
            {
                await templateService.AddOrUpdateTemplateAsync(template, user);

                return ApiHelper.CreateSuccessResult(template);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<Template>(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ApiResult<bool>> DeleteTemplate(String id)
        {
            try
            {

                await templateService.DeleteTemplate(id);

                return ApiHelper.CreateSuccessResult(true);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<bool>(e.Message);
            }
        }
    }
}
