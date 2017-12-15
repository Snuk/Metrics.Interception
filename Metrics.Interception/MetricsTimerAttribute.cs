using System;

namespace Metrics.Interception
{
    public class MetricsTimerAttribute : Attribute, IMetricsAttribute
    {
        public MetricsTimerAttribute(string context, string name)
        {
            Context = context;
            Name = name;
        }

        public string Context { get; set; }

        public string Name { get; set; }
    }
}
