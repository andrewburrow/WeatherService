using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMWIdPlatform.Authentication {
	public class ApiKeyAccount {
        public string GatewayKey { get; set; }
        public List<string> Roles { get; set; }
    }
}