using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Practices.Unity.InterceptionExtension;
using NLog;

namespace Metrics.Interception
{
    public class MetricsInterceptor : IInterceptionBehavior
    {
        private readonly IMetricsProvider _metricsProvider;
        private readonly ILogger _logger;
        private static readonly Regex MacroRegex = new Regex("{.*}", RegexOptions.Compiled);

        public MetricsInterceptor(IMetricsProvider metricsProvider, ILogger logger)
        {
            _metricsProvider = metricsProvider;
            _logger = logger;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var metricsAttributes = input.MethodBase.GetCustomAttributes(true).OfType<IMetricsAttribute>();
            var disposables = new List<IDisposable>();

            try
            {
                foreach (var metricsAttribute in metricsAttributes)
                {
                    FillMacros(input, metricsAttribute);

                    switch (metricsAttribute)
                    {
                        case MetricsTimerAttribute timerAttribute:
                            var metricTimer = _metricsProvider.Timer(timerAttribute.Context, timerAttribute.Name);
                            disposables.Add(metricTimer);
                            break;

                        case MetricsCounterAttribute counterAttribute:
                            var counterValue = long.Parse(GetMethodParamValue(input, counterAttribute.ParamName) ?? "1");
                            _metricsProvider.Counter(counterAttribute.Context, counterAttribute.Name, counterValue);
                            break;

                        case MetricsMeterAttribute meterAttribute:
                            var meterValue = long.Parse(GetMethodParamValue(input, meterAttribute.ParamName) ?? "1");
                            _metricsProvider.Meter(meterAttribute.Context, meterAttribute.Name, meterValue);
                            break;

                        case MetricsHistogramAttribute histogramAttribute:
                            var histogramValue = long.Parse(GetMethodParamValue(input, histogramAttribute.ParamName) ?? "1");
                            _metricsProvider.Meter(histogramAttribute.Context, histogramAttribute.Name, histogramValue);
                            break;
                    }
                }

                return getNext()(input, getNext);
            }
            finally
            {
                foreach (var disposable in disposables)
                {
                    disposable?.Dispose();
                }
            }
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute => true;

        private void FillMacros(IMethodInvocation input, IMetricsAttribute attribute)
        {
            try
            {
                attribute.Context = GetMacroValue(input, attribute.Context);
                attribute.Name = GetMacroValue(input, attribute.Name);
            }
            catch (Exception ex)
            {
                _logger.Warn(
                    ex,
                    "Не удалось заполнить макросы для метрики {0}.{1}, возможно что-то криво настроено",
                    attribute.Context,
                    attribute.Name);
            }
        }

        private static string GetMacroValue(IMethodInvocation input, string origin)
        {
            var macro = MacroRegex.Match(origin).Value;
            if (string.IsNullOrEmpty(macro))
            {
                return origin;
            }

            var macroValue = GetMethodParamValue(input, macro);
            return origin.Replace(macro, macroValue ?? string.Empty);
        }

        private static string GetMethodParamValue(IMethodInvocation input, string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
            {
                return null;
            }

            paramName = paramName.Replace("{", string.Empty).Replace("}", string.Empty);

            for (var index = 0; index < input.Arguments.Count; index++)
            {
                var parameterInfo = input.Arguments.GetParameterInfo(index);
                var inputArgument = input.Arguments[index];
                var paramSubNames = paramName.Split('.');

                if (!string.Equals(parameterInfo.Name, paramSubNames.FirstOrDefault(), StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                return GetParamValueByPath(inputArgument, paramSubNames)?.ToString().ToLower();
            }

            return null;
        }

        private static object GetParamValueByPath(object obj, IReadOnlyList<string> path, int index = 1)
        {
            if (index == path.Count)
            {
                return obj;
            }

            if (obj == null)
            {
                return null;
            }

            var properties = obj.GetType().GetProperties();
            foreach (var property in properties.Where(p => string.Equals(p.Name, path[index], StringComparison.OrdinalIgnoreCase)))
            {
                var propValue = property.GetValue(obj, null);
                return GetParamValueByPath(propValue, path, index + 1);
            }

            return null;
        }
    }
}
