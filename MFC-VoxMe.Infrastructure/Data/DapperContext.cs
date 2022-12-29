using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Data
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _jimStagingConnectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            //_jimStagingConnectionString = _configuration.GetConnectionString("JIMStagingConnection");
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        //public SqlConnection CreateConnection1()
        //   => new SqlConnection(_jimStagingConnectionString);
    }
}
