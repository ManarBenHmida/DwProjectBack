using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;

public class KpiService : IKpiService
{
    private readonly DatabaseContext _context;

    public KpiService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<KpiData>> GetSalesByCategory()
    {
        var query = @"
            SELECT cc.CustomerCategoryName as Label, 
                   COUNT(i.InvoiceID) as Value,
                   'SalesByCategory' as Category
            FROM Invoices i
            JOIN Customersdim c ON i.CustomerID = c.CustomerID
            JOIN CustomeryCategory cc ON c.CustomerCategoryID = cc.CustomerCategoryID
            GROUP BY cc.CustomerCategoryName";

        var dataTable = await _context.ExecuteQueryAsync(query);
        return ConvertToKpiData(dataTable);
    }

    public async Task<List<KpiData>> GetMonthlySales()
    {
        try
        {
            var query = @"
SELECT FORMAT(InvoiceDate, 'yyyy-MM') as Label,
       SUM(il.Quantity * il.UnitPrice) as Value,
       'MonthlySales' as Category
FROM Invoices i
JOIN fact il ON i.InvoiceID = il.InvoiceID 
GROUP BY FORMAT(InvoiceDate, 'yyyy-MM')
ORDER BY Label";

            var dataTable = await _context.ExecuteQueryAsync(query);
            return ConvertToKpiData(dataTable);
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ChartData> GetCustomerDistribution()
    {
        var query = @"
            SELECT cc.CustomerCategoryName as Label, 
                   COUNT(c.CustomerID) as Value
            FROM Customersdim c
            JOIN CustomeryCategory cc ON c.CustomerCategoryID = cc.CustomerCategoryID
            GROUP BY cc.CustomerCategoryName";

        var dataTable = await _context.ExecuteQueryAsync(query);
        return ConvertToChartData(dataTable);
    }

    public async Task<List<KpiData>> GetTopProducts()
    {
        var query = @"
            SELECT TOP 10 si.StockItemName as Label,
                   SUM(il.Quantity) as Value,
                   'TopProducts' as Category
            FROM fact il
            JOIN DestProducts si ON il.StockItemID = si.StockItemID
            GROUP BY si.StockItemName
            ORDER BY Value DESC";

        var dataTable = await _context.ExecuteQueryAsync(query);
        return ConvertToKpiData(dataTable);
    }
    public async Task<ChartData> GetColorDistribution()
    {
        var query = @"
        SELECT 
            dc.ColorName as Label, 
            COUNT(il.InvoiceLineID) as Value
        FROM fact il
        JOIN DestProducts dp ON il.StockItemID = dp.StockItemID
        JOIN DestiationColors dc ON dp.ColorID = dc.ColorID
        WHERE dc.ColorName IS NOT NULL
        GROUP BY dc.ColorName
        ORDER BY Value DESC";

        var dataTable = await _context.ExecuteQueryAsync(query);
        return ConvertToChartData(dataTable);
    }

    public async Task<List<KpiData>> GetTopColorsBySales()
    {
        var query = @"
        SELECT TOP 10
            dc.ColorName as Label,
            SUM(il.Quantity * il.LineProfit) as Value,
            'TopColors' as Category
        FROM fact il
        JOIN DestProducts dp ON il.StockItemID = dp.StockItemID
        JOIN DestiationColors dc ON dp.ColorID = dc.ColorID
        WHERE dc.ColorName IS NOT NULL
        GROUP BY dc.ColorName
        ORDER BY Value DESC";

        var dataTable = await _context.ExecuteQueryAsync(query);
        return ConvertToKpiData(dataTable);
    }
    public async Task<ChartData> GetDeliveryMethodDistribution()
    {
        var query = @"
            SELECT dm.DeliveryMethodName as Label,
                   COUNT(i.InvoiceID) as Value
            FROM Invoices i
            JOIN DelevryMethod dm ON i.DeliveryMethodID = dm.DeliveryMethodID
            GROUP BY dm.DeliveryMethodName";

        var dataTable = await _context.ExecuteQueryAsync(query);
        return ConvertToChartData(dataTable);
    }

    private List<KpiData> ConvertToKpiData(DataTable dataTable)
    {
        var kpiData = new List<KpiData>();
        foreach (DataRow row in dataTable.Rows)
        {
            kpiData.Add(new KpiData
            {
                Label = row["Label"].ToString(),
                Value = Convert.ToDecimal(row["Value"]),
                Category = row["Category"]?.ToString()
            });
        }
        return kpiData;
    }

    private ChartData ConvertToChartData(DataTable dataTable)
    {
        var labels = new List<string>();
        var values = new List<decimal>();

        foreach (DataRow row in dataTable.Rows)
        {
            labels.Add(row["Label"].ToString());
            values.Add(Convert.ToDecimal(row["Value"]));
        }

        return new ChartData
        {
            Labels = labels,
            Values = values,
            ChartType = "pie"
        };
    }
}