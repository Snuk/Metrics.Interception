using System;

namespace Metrics.Interception
{
    public class MetricsProvider : IMetricsProvider
    {
        public IDisposable Timer(string contextName, string name)
        {
            return Metric.Context(contextName).Timer(name, Unit.None).NewContext();
        }

        public void Counter(string contextName, string name, long amount)
        {
            Metric.Context(contextName).Counter(name, Unit.None).Increment(amount);
        }

        public void Meter(string contextName, string name, long count)
        {
            Metric.Context(contextName).Meter(name, Unit.None).Mark(count);
        }

        public void Histogram(string contextName, string name, long value)
        {
            Metric.Context(contextName).Histogram(name, Unit.None).Update(value);
        }
    }
}
