using Dapper;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Data.QueryGenerator
{
    public class DynamicQueryGenerator : IDynamicQueryGenerator
    {
        private readonly DapperContext _context;

        public DynamicQueryGenerator(DapperContext context)
        {
            _context = context;
        }
        public async Task<dynamic> SelectFrom(SelectFrom select)
        {
            string sub = "", query = "";
            if (select.whereClause != null)
            {
                sub = "where ";
                if (select.whereClause.Count == 1)
                    sub += select.whereClause.First().Key + "=" + select.whereClause.First().Value;
                else
                {
                    foreach (KeyValuePair<string, object> valuePair in select.whereClause)
                    {
                        var last = select.whereClause.Last();
                        if (valuePair.Equals(last))
                        {
                            sub += valuePair.Key + "=" + valuePair.Value;
                        }
                        else sub += valuePair.Key + "=" + valuePair.Value + " " + select.logOperator + " ";
                    }
                }
            }
            if (select.function != null)
            {
                query = @$"SELECT {select.function}({select.columns}) FROM [dbo].[{select.table}]
                           {sub}";
            }
            else query = @$"SELECT {select.columns} FROM [dbo].[{select.table}]
                           {sub}";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QuerySingleAsync(query);
                return result;
                
                //return result
                //    .Cast<IDictionary<string, object>>()
                //    .Select(it => it.ToDictionary(it => it.Key, it => it.Value));
            }
        }

        public void InsertInto<T>(string table, T dto)
        {
            var colsList = new List<string>();
            var dataList = new List<string>();

            foreach (var propertyInfo in dto.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(dto) != null)
                {
                    colsList.Add(propertyInfo.Name);
                    dataList.Add("'" + propertyInfo.GetValue(dto).ToString() + "'");
                }
            }
            string cols = string.Join(",", colsList);
            string data = string.Join(",", dataList);

            var query = @$"INSERT INTO [dbo].[{table}]
                               ({cols})
                         VALUES
                               ({data})";
            using (var connection = _context.CreateConnection())
            {
                var result = connection.Query<string>(query);
            }
        }

    }
}
