using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using WebApiMWIdPlatform.Authentication;
using WebApiMWIdPlatform.Filters;

namespace WebApiMWIdPlatform.Extensions {
	public static class ConfigureSwaggerExtension {
		public static void ConfigureSwagger(this IServiceCollection services) {
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo {
					Version = "v1",
					Title = "WeatherService.Api",
					Description = "Weather Service Api"
				});
				c.OperationFilter<AuthorizeCheckOperationFilter>();
				c.AddSecurityDefinition(ApiKeyAuthenticationOptions.AuthenticationScheme, new OpenApiSecurityScheme {
					Description = $"Api key needed to access the endpoints.",
					In = ParameterLocation.Header,
					Name = ApiKeyAuthenticationOptions.ApiKeyHeaderName,
					Type = SecuritySchemeType.ApiKey
				});
			});
		}

	}
}