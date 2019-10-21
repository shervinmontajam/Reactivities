using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistence;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {

    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;

        public IsHostRequirementHandler(IHttpContextAccessor httpContextAccessor, DataContext dataContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            if (context.Resource is AuthorizationFilterContext authorizationFilterContext)
            {
                var currentUsername = _httpContextAccessor.HttpContext.User?.Claims?.SingleOrDefault(u=>u.Value == ClaimTypes.NameIdentifier)?.Value;
                var activityId = Guid.Parse(authorizationFilterContext.RouteData.Values["id"].ToString());
                var activity = _dataContext.Activities.FindAsync(activityId).Result;
                var host = activity.UserActivities.SingleOrDefault(a => a.IsHost);
                if(host?.AppUser.UserName == currentUsername)
                    context.Succeed(requirement);
            }
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}
