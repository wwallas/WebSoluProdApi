using Microsoft.Data.SqlClient;
using System.Data;
//using System.Data.SqlClient;
//using System.Data;

namespace WebProdApi.Data
{
    public class AdoNetContext
    {
        private readonly string _connectionString;

        public AdoNetContext(IConfiguration configuration)
        {
            // Fixing the invalid method call and ensuring proper syntax
            _connectionString = configuration.GetConnectionString("DefaultConnection") + ";TrustServerCertificate=True";
        }

        // Ejecuta un SP y retorna un DataTable
        public DataTable ExecuteStoredProcedure(string sp_GetAllProducts, SqlParameter[] parameters = null)
        {
            var dataTable = new DataTable();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sp_GetAllProducts, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
            }

            return dataTable;
        }

        // Ejecuta un SP y retorna un valor escalar (ej: ID insertado)
        public object ExecuteScalarStoredProcedure(string sp_GetProductById, SqlParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sp_GetProductById, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                connection.Open();
                return command.ExecuteScalar();
            }
        }
    }

    internal record struct NewStruct(object Item1, object Item2)
    {
        public static implicit operator (object, object)(NewStruct value)
        {
            return (value.Item1, value.Item2);
        }

        public static implicit operator NewStruct((object, object) value)
        {
            return new NewStruct(value.Item1, value.Item2);
        }
    }
}
