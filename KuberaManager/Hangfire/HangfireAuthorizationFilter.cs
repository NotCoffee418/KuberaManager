using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Hangfire
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Get user role claims
            var userRoleClaims = httpContext.User.Claims
                .Where(x => x.Type == System.Security.Claims.ClaimTypes.Role)
                .ToList();

            // If userRoleClaims list contains admin
            bool userIsAdmin = userRoleClaims
                .Where(x => x.Value == "Admin")
                .Count() > 0;

            // If authenticated & authorized
            return httpContext.User.Identity.IsAuthenticated && userIsAdmin;
        }
    }
}
