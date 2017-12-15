using System;

namespace Metrics.Interception
{
    public class MetricsHistogramAttribute : Attribute, IMetricsAttribute
    {
        public MetricsHistogramAttribute(string context, string name, string paramName = null)
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
