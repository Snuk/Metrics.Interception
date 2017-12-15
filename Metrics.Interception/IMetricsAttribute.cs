namespace Metrics.Interception
{
    public interface IMetricsAttribute
    {
        string Context { get; set; }

        string Name { get; set; }
    }
}
