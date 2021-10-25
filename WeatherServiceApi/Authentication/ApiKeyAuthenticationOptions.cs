using Microsoft.AspNetCore.Authentication;

namespace WebApiMWIdPlatform.Authentication {
	public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions {
		public const string AuthenticationScheme = "API Key";
		public const string ApiKeyHeaderName = "X-Api-Key";
	}
}

