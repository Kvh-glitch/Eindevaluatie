using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace AuctionHouseImport.DatabaseConfig
{
    public static class DataBaseConnection
    {
        public const string ConnectionString = "Data Source = pandora\\sqlexpress;Initial Catalog = AuctionHouse; Integrated Security = True; Trust Server Certificate=True";

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
