using System;

namespace DEVINO.Infrastructure.Metrics.Settings
{
    public class MetricsSettings : IMetricsSettings
    {
        public string GraphiteUri { get; set; }

        public TimeSpan GraphiteInterval { get; set; }

        public bool EnableSystemCountersMetrics { get; set; }

        public bool EnableAppCountersMetrics { get; set; }
    }
}
