using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace DataCloud.PipelineDesigner.WebClient
{
    public class SwaggerAuthOperationFilter : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerAuth = context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(filter => filter is AuthorizeAttribute);
            var filterDescriptor = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var endPointAuth = filterDescriptor.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
            var allowAnonymous = filterDescriptor.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);

            if ((endPointAuth || controllerAuth) && !allowAnonymous)
            {

                operation.Responses.Add("401", new OpenApiResponse
                {
                    Description = "Unauthorized"
                });
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id =  "Bearer"
                              },
                              Scheme =  JwtBearerDefaults.AuthenticationScheme,

                          },
                         new string[] {}
                    }
                });
            }
        }
    }
}
