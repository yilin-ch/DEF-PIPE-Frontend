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
    [Route("api/templates")]
    [ApiController]
    public class TemplatesController : ControllerBase
    {
        Services.Interfaces.ITemplateService templateService;

        public TemplatesController(ITemplateService services)
        {
            //TODO: Update this to use dependency injection
            templateService = services;
        }

        /// <summary>
        /// Add/Update a template
        /// </summary>
        /// <response code="200">New template</response>
        [HttpPost]
        public async Task<ApiResult<Template>> AddOrUpdateTemplateAsync([FromBody] Template template)
        {
            try
            {
                Console.WriteLine("write a template");
                await templateService.AddOrUpdateTemplateAsync(template);

                return ApiHelper.CreateSuccessResult(template);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<Template>(e.Message);
            }
        }

        /// <summary>
        /// Get Available template
        /// </summary>
        /// <response code="200">List of template</response>
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

        /// <summary>
        /// Delete a template
        /// </summary>
        /// <response code="200">Success</response>
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
