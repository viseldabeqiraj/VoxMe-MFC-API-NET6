using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Data
{
    public class DynamicQueryGenerator
    {
        private readonly DapperContext _context;

        public DynamicQueryGenerator(DapperContext context)
        {
            _context = context;
        }
        public void InsertInto(string table, string[] cols, string[] data)
        {
            var query = @$"INSERT INTO [dbo].[{table}]
                               ({cols})
                         VALUES
                               ({data})";
            using (var connection = _context.CreateConnection())
            {
                var companies = connection.QueryAsync<string>(query);
            }
        }
    }
}
