using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace WebApiMWIdPlatform.Authentication {
	public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions> {
        private readonly ApiKeyAccounts _apiKeyAccounts;
        private readonly ILogger<ApiKeyAuthenticationHandler> _logger;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOptions<ApiKeyAccounts> apiKeyAccounts) : base(options, logger, encoder, clock) {
            _apiKeyAccounts = apiKeyAccounts.Value;
            _logger = logger.CreateLogger<ApiKeyAuthenticationHandler>();

        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            if (!Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.ApiKeyHeaderName, out var apiKeyHeaderValues)) {

                return AuthenticateResult.NoResult();
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(providedApiKey)) {
                return AuthenticateResult.NoResult();
            }

            KeyValuePair<string, ApiKeyAccount> apiKeyAccount = _apiKeyAccounts.FirstOrDefault(a => $"{a.Key}:{a.Value.GatewayKey}" == providedApiKey);

            //Make sure that the route account and the account key match
            if (apiKeyAccount.Key == null) {
                return AuthenticateResult.Fail("Invalid API Key provided.");
            }

            var claims = apiKeyAccount.Value.Roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

            var identity = new ClaimsIdentity(claims, ApiKeyAuthenticationOptions.AuthenticationScheme);
            var identities = new List<ClaimsIdentity> { identity };
            var principal = new ClaimsPrincipal(identities);
            var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationOptions.AuthenticationScheme);

            return AuthenticateResult.Success(ticket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties) {
            Response.StatusCode = 401;
            _logger.LogError($"HandleChallengeAsync rejected an authentication attempt because an invalid API key was supplied. Response HttpStatusCode is {Response.StatusCode}");
            await Response.WriteAsync("Authentication error.  Invalid credentials supplied.");
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties) {
            Response.StatusCode = 403;
            _logger.LogError($"HandleForbiddenAsync rejected an authorization attempt because an incorrect claim was used. Response HttpStatusCode is {Response.StatusCode}");
            await Response.WriteAsync("Authorisation error.  Incorrect claim was used.");
        }
    }
}
