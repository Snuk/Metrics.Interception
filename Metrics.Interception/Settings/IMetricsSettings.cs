using System;

namespace DEVINO.Infrastructure.Metrics.Settings
{
    public interface IMetricsSettings
    {
        string GraphiteUri { get; set; }

        TimeSpan GraphiteInterval { get; set; }

        bool EnableSystemCountersMetrics { get; set; }

        bool EnableAppCountersMetrics { get; set; }
    }
}