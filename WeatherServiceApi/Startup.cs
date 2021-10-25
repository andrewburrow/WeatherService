using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiMWIdPlatform.Authentication;
using WebApiMWIdPlatform.Extensions;

namespace WebApiMWIdPlatform {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services
				.AddAuthentication(options => {
					options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddApiKeySupport(options => { })
				.AddMicrosoftIdentityWebApi(Configuration);

			services.AddControllers();

			string azInstance = Configuration.GetSection("AzureAd").GetValue<string>("Instance");
			string azTenantId = Configuration.GetSection("AzureAd").GetValue<string>("TenantId");
			string azClientId = Configuration.GetSection("AzureAd").GetValue<string>("ClientId");
			string azScopeUrl = $"api://{azClientId}/orders.osapi";

			services.ConfigureSwagger();

			services.Configure<ApiKeyAccounts>(Configuration.GetSection(ApiKeyAccounts.ConfigSection));

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather Service V1");
				c.DocumentTitle = "SwaggerUITitle";
				c.DocExpansion(DocExpansion.None);
				c.RoutePrefix = string.Empty;
			});

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
			});
		}
	}
}
