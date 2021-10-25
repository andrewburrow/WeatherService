using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApiMWIdPlatform.Authentication {
	public class AuthorizeRolesAttribute : AuthorizeAttribute {
		public AuthorizeRolesAttribute(params string[] roles) : base() {
			Roles = string.Join(",", roles);

			var authSchemes = string.Join(",", new string[] { JwtBearerDefaults.AuthenticationScheme
					,ApiKeyAuthenticationOptions.AuthenticationScheme });
			AuthenticationSchemes = authSchemes;
		}
	}
}