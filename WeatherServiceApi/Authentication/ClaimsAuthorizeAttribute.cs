using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMWIdPlatform.Authentication {
	public sealed class ClaimsAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        private readonly string[] _userRoles;

        public ClaimsAuthorizeAttribute(params string[] roles)
        {
			_userRoles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;

            if (!isAuthenticated)
            {
                context.Result = new UnauthorizedObjectResult("Access denied.");
                return;
            }
            
            var isAuthorized = false;

            if (_userRoles.Length == 0)
                isAuthorized = true;
            else
                isAuthorized = _userRoles.Any(r =>  context.HttpContext.User.IsInRole(r));

            if (!isAuthorized)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}