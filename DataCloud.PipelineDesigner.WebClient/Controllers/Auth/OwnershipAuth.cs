using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.WebClient.Controllers.Auth
{
    public class OwnershipAuthHandler : AuthorizationHandler<OwnershipRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnershipRequirement requirement)
        {

            if (context.User.Identity.Name == (context.Resource as string))
            {
                context.Succeed(requirement);
            }


            return Task.CompletedTask;
        }

    }

    public class OwnershipRequirement : IAuthorizationRequirement { }
}
