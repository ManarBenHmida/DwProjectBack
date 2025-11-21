
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<DataTable> ExecuteQueryAsync(string query, SqlParameter[] parameters = null)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(query, connection);

        if (parameters != null)
            command.Parameters.AddRange(parameters);

        await connection.OpenAsync();
        var dataTable = new DataTable();
        using var adapter = new SqlDataAdapter(command);
        adapter.Fill(dataTable);

        return dataTable;
    }
}