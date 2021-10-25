using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiMWIdPlatform.Authentication;

namespace WebApiMWIdPlatform.Filters {
	public class AuthorizeCheckOperationFilter : IOperationFilter {
		public void Apply(OpenApiOperation operation, OperationFilterContext context) {
			context.ApiDescription.TryGetMethodInfo(out var methodInfo);

			if (methodInfo == null)
				return;

			var hasAuthorizeAttribute = false;

			if (methodInfo.MemberType == MemberTypes.Method) {
				// NOTE: Check the controller itself has Authorize attribute
				hasAuthorizeAttribute = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeRolesAttribute>().Any();

				// NOTE: Controller has Authorize attribute, so check the endpoint itself.
				//       Take into account the allow anonymous attribute
				if (hasAuthorizeAttribute)
					hasAuthorizeAttribute = !methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
				else
					hasAuthorizeAttribute = methodInfo.GetCustomAttributes(true).OfType<AuthorizeRolesAttribute>().Any();
			}

			if (!hasAuthorizeAttribute)
				return;

			operation.Security = new List<OpenApiSecurityRequirement> {
				  new OpenApiSecurityRequirement {
					  {
						  new OpenApiSecurityScheme {
							  Reference = new OpenApiReference {
								  Type = ReferenceType.SecurityScheme,
								  Id = ApiKeyAuthenticationOptions.AuthenticationScheme
							  },
							  Name = ApiKeyAuthenticationOptions.ApiKeyHeaderName,
							  In = ParameterLocation.Header
						  },
						  new List<string>()
					  }
				  }
			  };
		}
	}
}
