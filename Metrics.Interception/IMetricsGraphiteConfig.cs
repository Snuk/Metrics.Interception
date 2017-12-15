using System;

namespace Metrics.Interception
{
    public interface IMetricsGraphiteConfig
    {
        string GraphiteUri { get; set; }

        TimeSpan GraphiteInterval { get; set; }
    }
}