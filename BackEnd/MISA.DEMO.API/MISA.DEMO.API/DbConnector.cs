using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API
{
    public class DbConnector<TEntity>
    {
        
        public IEnumerable<TEntity> GetAll()
        {
            var className = typeof(TEntity).Name;
            string connectionString = "User Id=nvmanh;Host=103.124.92.43;port=3306;password=12345678;Database=MS2_31_PVTRONG_CukCuk;Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var sql = $"SELECT * FROM {className}";
            var entities = dbConnection.Query<TEntity>(sql);
            return entities; 
        }
        
    }
}
