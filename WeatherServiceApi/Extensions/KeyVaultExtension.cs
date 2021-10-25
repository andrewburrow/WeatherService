using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System;

namespace WebApiMWIdPlatform.Extensions {
	public static class KeyVaultExtension
	{
		private const string KeyVaultNameConfigFieldName = "KeyVaultName";

		public static void ConfigureKeyVault(this IConfigurationBuilder configurationBuilder)
		{
			var builtConfig = configurationBuilder.Build();
			var keyVaultUri = new Uri($"https://{builtConfig[KeyVaultNameConfigFieldName]}.vault.azure.net/");
			var secretClient = new SecretClient(keyVaultUri, new DefaultAzureCredential());
			configurationBuilder.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
		}

	}
}