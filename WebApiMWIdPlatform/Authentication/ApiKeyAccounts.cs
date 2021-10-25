using System.Collections.Generic;

namespace WebApiMWIdPlatform.Authentication {
	public class ApiKeyAccounts : Dictionary<string, ApiKeyAccount> {
        public const string ConfigSection = "ApiKeyAccounts";
    }
}