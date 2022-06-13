using DataCloud.PipelineDesigner.Repositories.Models;
using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.WebClient.Models;
using DataCloud.PipelineDesigner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DataCloud.PipelineDesigner.WebClient.Controllers
{
    [Authorize]
    [Route("api/repo")]
    [ApiController]
    public class RepoController : ControllerBase
    {
        Services.Interfaces.IUserService userService;
        Services.Interfaces.IPublicRepoService pRepoService;
        private readonly IAuthorizationService authorizationService;
        IDSLService dslService;
        public RepoController(IUserService uService, IPublicRepoService prService, IAuthorizationService authService)
        {
            userService = uService;
            pRepoService = prService;
            authorizationService= authService;
            dslService = new DSLService();
        }

        /// <summary>
        /// Search for puclic workflow
        /// </summary>
        /// <response code="200">List of public workflow matching the query</response>
        [HttpGet("s")]
        public async Task<ApiResult<List<PublicRepo>>> SearchRepo([FromQuery] string query)
        {
            try
            {
                List<PublicRepo> prs = await pRepoService.Search(query);
                return ApiHelper.CreateSuccessResult(prs);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<List<PublicRepo>>(e.Message);
            }
        }

        /// <summary>
        /// Get a specific workflow
        /// </summary>
        /// <response code="200">List of public repo matching the query</response>
        [HttpGet]
        public async Task<ApiResult<Template>> ImportRepo([FromQuery] string user, [FromQuery] string workflowName)
        {

            try
            {
                Template prs = await pRepoService.GetPublicRepo(user, workflowName);
                return ApiHelper.CreateSuccessResult(prs);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<Template>(e.Message);
            }
        }

        /// <summary>
        /// Get user workflows
        /// </summary>
        /// <response code="200">List workflow in the user's repository</response>
        [HttpGet("{user}")]
        public async Task<ApiResult<List<Template>>> GetAvailableRepo(String user)
        {

            var authorizationResult = await authorizationService.AuthorizeAsync(User, user, "OwnershipPolicy");

            if (!authorizationResult.Succeeded)
            {
                return ApiHelper.CreateFailedResult< List<Template>>("Forbiden");
            }
            try
            {
                User userDB = await userService.GetRepoAsync(user);
                return ApiHelper.CreateSuccessResult(userDB.Templates);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<List<Template>>(e.Message);
            }
        }



        /// <summary>
        /// Add/update workflow
        /// </summary>
        /// <response code="200">New workflow</response>
        [HttpPost("{user}")]
        public async Task<ApiResult<Template>> AddOrUpdateRepoAsync([FromBody] Template template, string user)
        {
            try
            {
                var result = await userService.UpdateRepoAsync(template, user);


                // Probably a better way to do nested upsert
                if (result.ModifiedCount < 1)
                {
                    await userService.AddRepoAsync(template, user);
                }

                if (template.Public)
                {
                    await pRepoService.AddRepo(user, template.Name);
                }
                else
                {
                    await pRepoService.RemoveRepo(user, template.Name);
                }

                return ApiHelper.CreateSuccessResult(template);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<Template>(e.Message);
            }
        }


        /// <summary>
        /// Delete workflow
        /// </summary>
        /// <response code="200">Success</response>
        [HttpDelete("{user}/{id}")]
        public async Task<ApiResult<bool>> DeleteTemplate(String user, String id)
        {
            try
            {

                var result = await userService.DeleteTemplate(user, id);

                return ApiHelper.CreateSuccessResult(true);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<bool>(e.Message);
            }
        }


        /// <summary>
        /// Import DSL
        /// </summary>
        /// <remarks>
        /// Swagger doesn't allow a correct formating (newline, tab), try the request on postman
        /// -> = tab (\t)
        /// 
        /// Pipeline name {\
        ///->   ->  steps:\
        ///->   ->  -	Extract\
        ///->   ->  implementation: docker\
        ///->   ->  image: def/def-extract\
        ///->   ->  environmentParameters: {\
        ///->   ->  ->  pwd = xxx\
        ///->   ->  }\
        ///->   ->  resourceProvider: External\
        ///->   ->  -	Filter\
        ///->   ->  implementation: docker\
        ///->   ->  image: def/def-filter\
        ///->   ->  environmentParameters: {\
        ///->   ->  ->  pwd = xxx\
        ///->   ->  }\
        ///->   ->  resourceProvider: External\
        ///}
        /// </remarks>
        /// <response code="200"></response>
        [HttpPost("{user}/import")]
        public async Task<ApiResult<bool>> ImportDsl([FromForm] string dsl, String user)
        {
            try
            {
                var d = dslService.DeserializeDsl(dsl);
                var r = CanvasService.TransformDslToCanvas(d);

                var template = new Template { Name = d.Pipeline.Name, Category = "Imported", Id = Guid.NewGuid().ToString(), CanvasTemplate = r };

                var result = await userService.AddRepoAsync(template, user);

                return ApiHelper.CreateSuccessResult(true);
            }
            catch (Exception e)
            {
                return ApiHelper.CreateFailedResult<bool>(e.Message);
            }
        }
    }
}
