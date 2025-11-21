using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class KpiController : ControllerBase
{
    private readonly IKpiService _kpiService;

    public KpiController(IKpiService kpiService)
    {
        _kpiService = kpiService;
    }

    [HttpGet("sales-by-category")]
    public async Task<ActionResult<List<KpiData>>> GetSalesByCategory()
    {
        return Ok(await _kpiService.GetSalesByCategory());
    }

    [HttpGet("monthly-sales")]
    public async Task<ActionResult<List<KpiData>>> GetMonthlySales()
    {
        return Ok(await _kpiService.GetMonthlySales());
    }

    [HttpGet("customer-distribution")]
    public async Task<ActionResult<ChartData>> GetCustomerDistribution()
    {
        return Ok(await _kpiService.GetCustomerDistribution());
    }

    [HttpGet("top-products")]
    public async Task<ActionResult<List<KpiData>>> GetTopProducts()
    {
        return Ok(await _kpiService.GetTopProducts());
    }

    [HttpGet("delivery-method-distribution")]
    public async Task<ActionResult<ChartData>> GetDeliveryMethodDistribution()
    {
        return Ok(await _kpiService.GetDeliveryMethodDistribution());
    }
    [HttpGet("color-distribution")]
    public async Task<ActionResult<ChartData>> GetColorDistribution()
    {
        return Ok(await _kpiService.GetColorDistribution());
    }

    [HttpGet("top-colors")]
    public async Task<ActionResult<List<KpiData>>> GetTopColorsBySales()
    {
        return Ok(await _kpiService.GetTopColorsBySales());
    }
}