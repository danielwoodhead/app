using System.Collections.Generic;
using MyHealth.Identity.Admin.Configuration.Identity;

namespace MyHealth.Identity.Admin.Configuration.IdentityServer
{
    public class Client : global::IdentityServer4.Models.Client
    {
        public List<Claim> ClientClaims { get; set; } = new List<Claim>();
    }
}






