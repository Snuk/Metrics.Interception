using System;

namespace Metrics.Interception
{
    public class MetricsMeterAttribute : Attribute, IMetricsAttribute
    {
        public MetricsMeterAttribute(string context, string name, string paramName = null)
        {
            Context = context;
            Name = name;
            ParamName = paramName;
        }

        public string Context { get; set; }

        public string Name { get; set; }

        public string ParamName { get; }
    }
}
