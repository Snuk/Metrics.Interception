using System;

namespace Metrics.Interception
{
    public interface IMetricsProvider
    {
        IDisposable Timer(string contextName, string name);

        void Counter(string contextName, string name, long amount);

        void Meter(string contextName, string name, long count);

        void Histogram(string contextName, string name, long value);
    }
}