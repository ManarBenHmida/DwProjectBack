public class KpiData
{
    public string Label { get; set; }
    public decimal Value { get; set; }
    public string Category { get; set; }
    public DateTime Date { get; set; }
}

public class ChartData
{
    public List<string> Labels { get; set; }
    public List<decimal> Values { get; set; }
    public string ChartType { get; set; }
}