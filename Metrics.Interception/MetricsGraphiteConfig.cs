using System;

namespace Metrics.Interception
{
    public class MetricsGraphiteConfig : IMetricsGraphiteConfig
    {
        public string GraphiteUri { get; set; }

        public TimeSpan GraphiteInterval { get; set; }
    }
}
