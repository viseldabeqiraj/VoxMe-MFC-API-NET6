using Dapper;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers;
using MFC_VoxMe.Infrastructure.GlobalErrorHandling.Retry_Policy;
using Polly.Retry;
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
            string query, sub = "";
            if (select.WhereClause != string.Empty)
            {
                sub = "WHERE " + select.WhereClause;
            }
            if (select.Function != null)
            {
                query = @$"SELECT {select.Function}({select.Columns}) {select.As} FROM [dbo].[{select.Table}]
                           {sub}";
            }
            else query = @$"SELECT {select.Columns}{select.As} FROM [dbo].[{select.Table}]
                           {sub}";
            using (var connection = _context.CreateConnection())
            {
                var test = await connection.QueryAsyncWithRetry(query);
                return await connection.QueryAsyncWithRetry(query);
            }
        }

        public async Task InsertInto<T>(SqlQuery<T> insertInto) 
        {
            var colsList = new List<string>();
            var dataList = new List<string>();

            foreach (var propertyInfo in insertInto.Dto.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(insertInto.Dto) != null)
                {
                    colsList.Add(propertyInfo.Name);
                    dataList.Add("'" + propertyInfo.GetValue(insertInto.Dto)?.ToString() + "'");
                }
            }
            string cols = string.Join(",", colsList);
            string data = string.Join(",", dataList);

            var query = @$"INSERT INTO [dbo].[{insertInto.Table}]
                               ({cols})
                         VALUES
                               ({data})";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsyncWithRetry(query);
            }
        }

        public async Task UpdateTable<T>(SqlQuery<T> update) 
        {
            string colsValues = "";
            string table = update.Dto.GetType().Name;
            foreach (var propertyInfo in update.Dto.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(update.Dto) != null)
                {
                    colsValues += propertyInfo.Name + "=" + "'" + propertyInfo.GetValue(update.Dto)?.ToString() + "'";
                }
            }
            //string whereClause = GetWhereClause(update);
            string whereClause = update.WhereClause;

            var query = @$"UPDATE [dbo].[{table}]
                              SET {colsValues} WHERE {whereClause}";
            using (var connection = _context.CreateConnection())
            {
                var result =  await connection.QueryAsyncWithRetry(query);
            }
        }

        public async Task Delete<T>(SqlQuery<T> delete)
        {
            var query = $@"DELETE FROM [dbo].[{delete.Table}]
                            WHERE {delete.WhereClause}";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsyncWithRetry(query);
            }
        }

        //public string GetWhereClause<T>(SqlQuery<T> query)
        //{
        //    string whereClause = "";

        //    var last = query.whereClause.Last();

        //    foreach (var valuePair in query.whereClause)
        //    {
        //        whereClause += valuePair.Key.ToString() + " ";
        //        whereClause += query.comparisonOperator != null
        //            ? query.comparisonOperator.ToString()
        //            : query.logOperator.ToString();
        //        whereClause += " " + $@"'{valuePair.Value}'" + " ";

        //        if (!valuePair.Equals(last))
        //        {
        //            whereClause += " " + query.logOperator.ToString() + " ";
        //        }
        //    }         

        //    return whereClause;
        //}

    }
}
