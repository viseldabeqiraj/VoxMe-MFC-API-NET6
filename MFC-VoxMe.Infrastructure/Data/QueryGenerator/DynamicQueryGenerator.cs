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
        public async Task<dynamic> SelectFrom(SqlQuery<string> select)
        {
            string sub = "", query = "";
            if (select.whereClause != null)
            {
                sub = "where ";
                //if (select.whereClause.Count == 1)
                //    sub += select.whereClause.First().Key + select.comparisonOperator + select.whereClause.First().Value;
                //else
                //{
                //    foreach (KeyValuePair<string, object> valuePair in select.whereClause)
                //    {
                //        var last = select.whereClause.Last();
                //        if (valuePair.Equals(last))
                //        {
                //            sub += valuePair.Key + select.comparisonOperator + valuePair.Value;
                //        }
                //        else sub += valuePair.Key + select.comparisonOperator + valuePair.Value + " " + select.logOperator + " ";
                //    }
                //}
                sub += GetWhereClause(select);
            }
            if (select.function != null)
            {
                query = @$"SELECT {select.function}({select.columns}) {select.As} FROM [dbo].[{select.table}]
                           {sub}";
            }
            else query = @$"SELECT {select.columns}{select.As} FROM [dbo].[{select.table}]
                           {sub}";
            using (var connection = _context.CreateConnection())
            {
                var test = await connection.QueryAsync(query);
                return await connection.QueryAsync(query);
            }
        }

        public async Task InsertInto<T>(SqlQuery<T> insertInto) 
        {
            var colsList = new List<string>();
            var dataList = new List<string>();

            foreach (var propertyInfo in insertInto.dto.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(insertInto.dto) != null)
                {
                    colsList.Add(propertyInfo.Name);
                    dataList.Add("'" + propertyInfo.GetValue(insertInto.dto)?.ToString() + "'");
                }
            }
            string cols = string.Join(",", colsList);
            string data = string.Join(",", dataList);

            var query = @$"INSERT INTO [dbo].[{insertInto.table}]
                               ({cols})
                         VALUES
                               ({data})";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<string>(query);
            }
        }

        public async Task UpdateTable<T>(SqlQuery<T> update) 
        {
            string colsValues = "";
            string table = update.dto.GetType().Name;
            foreach (var propertyInfo in update.dto.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(update.dto) != null)
                {
                    colsValues += propertyInfo.Name + "=" + "'" + propertyInfo.GetValue(update.dto)?.ToString() + "'";
                }
            }
            string whereClause = GetWhereClause(update);

            var query = @$"UPDATE [dbo].[{table}]
                              SET {colsValues} WHERE {whereClause}
                          ";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<string>(query);
            }
        }

        public string GetWhereClause<T>(SqlQuery<T> query)
        {
            string whereClause = "";

            var last = query.whereClause.Last();

            foreach (var valuePair in query.whereClause)
            {
                whereClause += valuePair.Key.ToString();
                whereClause += query.comparisonOperator != null
                    ? query.comparisonOperator.ToString()
                    : query.logOperator.ToString();
                whereClause += $@"'{valuePair.Value}'";

                if (!valuePair.Equals(last))
                {
                    whereClause += " " + query.logOperator.ToString() + " ";
                }
            }         

            return whereClause;
        }

    }
}
