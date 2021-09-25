using System;

namespace Zeppeling.Infrastructure.Core.Configuration
{
    public class ZeppelingInfrastructureConfiguration: IZeppelingInfrastructureConfiguration
    {
        public string ContextKey { get; set; }
        public string ContextName { get; set; }
        public bool MockIntegration { get; set; }
    }
}
