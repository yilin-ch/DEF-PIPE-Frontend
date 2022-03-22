using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.Services;
using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.WebClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        ITemplateService templateService;

        public TemplatesController()
        {
            //TODO: Update this to use dependency injection
            templateService = new TemplateService();
        }
        [HttpPost]
        public async Task<ApiResult<CanvasShapeTemplate>> AddOrUpdateTemplateAsync([FromBody] CanvasShapeTemplate template)
        {
            try
            {
                await templateService.AddOrUpdateTemplateAsync(template);

                return ApiHelper.CreateSuccessResult(template);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<CanvasShapeTemplate>(e.Message);
            }
        }

        [HttpGet]
        public async Task<ApiResult<List<CanvasShapeTemplate>>> GetAvailableTemplates()
        {
            try
            {
                var templates = await templateService.GetTemplatesAsync();
                return ApiHelper.CreateSuccessResult(templates);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<List<CanvasShapeTemplate>>(e.Message);
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
