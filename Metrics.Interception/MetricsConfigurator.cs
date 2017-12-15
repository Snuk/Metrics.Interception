using System;
using Metrics.Graphite;

namespace Metrics.Interception
{
    public class MetricsConfigurator
    {
        public MetricsConfigurator(IMetricsGraphiteConfig config)
        {
            Metric.Config.WithReporting(report => report.WithGraphite(new Uri(config.GraphiteUri), config.GraphiteInterval));
        }
    }
}
