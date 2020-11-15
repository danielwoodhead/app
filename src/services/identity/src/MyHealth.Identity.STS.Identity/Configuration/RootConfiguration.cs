using MyHealth.Identity.Shared.Configuration.Identity;
using MyHealth.Identity.STS.Identity.Configuration.Interfaces;

namespace MyHealth.Identity.STS.Identity.Configuration
{
    public class RootConfiguration : IRootConfiguration
    {      
        public AdminConfiguration AdminConfiguration { get; } = new AdminConfiguration();
        public RegisterConfiguration RegisterConfiguration { get; } = new RegisterConfiguration();
    }
}





