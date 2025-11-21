public interface IKpiService
{
    Task<List<KpiData>> GetSalesByCategory();
    Task<List<KpiData>> GetMonthlySales();
    Task<ChartData> GetCustomerDistribution();
    Task<List<KpiData>> GetTopProducts();
    Task<ChartData> GetDeliveryMethodDistribution();
    Task<ChartData> GetColorDistribution();
    Task<List<KpiData>> GetTopColorsBySales();


}
