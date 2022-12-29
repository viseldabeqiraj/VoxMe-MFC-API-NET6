using Dapper;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers;
using MFC_VoxMe.Infrastructure.GlobalErrorHandling.Retry_Policy;
using MFC_VoxMe.Infrastructure.Models;
using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
using Microsoft.Data.SqlClient;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data;
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
                    var value = propertyInfo.GetValue(insertInto.Dto)?.ToString();

                    if (value.Contains("'"))
                    {
                        var index = value.IndexOf("'");
                        value = value.Insert(index, "'");
                    }
                    dataList.Add("'" + value + "'");
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
                if (!string.IsNullOrEmpty(propertyInfo.GetValue(update.Dto)?.ToString()))
                {
                    colsValues += propertyInfo.Name + "=" + "'" + propertyInfo.GetValue(update.Dto)?.ToString() + "'" + ",";
                }
            }
            colsValues = colsValues.Remove(colsValues.Length - 1);
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


        //public async Task<List<ServicePaperworkModel>> GetRequiredPaperwork(MovingDataDto movingData)
        //{
        //    //GeneralHelper.LogMessage("Begin GetRequiredPaperwork");

        //    var properties = movingData.InventoryData.Properties.Property;

        //    string contract = properties.FirstOrDefault(s => s.Type == "Form.General.Contract").Description;
        //    string authority = properties.FirstOrDefault(s => s.Type == "Form.General.Authority").Description;
        //    string account = string.Empty;
        //    if(!string.IsNullOrEmpty(properties.FirstOrDefault(s => s.Type == "Form.General.Account").Description))
        //    {
        //        account = properties.FirstOrDefault(s => s.Type == "Form.General.Account").Description;
        //    }
        //    List<ServicePaperworkModel> paperworkList = new List<ServicePaperworkModel>();
        //    using (var sqlConnection = _context.CreateConnection1())
        //    {
        //        if (sqlConnection.State == ConnectionState.Open)
        //        {
        //            sqlConnection.Close();
        //        }

        //        sqlConnection.Open();
        //        SqlDataReader reader;

        //        using (var sqlCommand = new SqlCommand("Eden.p_GetRequiredPaperwork", sqlConnection))
        //        {
        //            sqlCommand.CommandType = CommandType.StoredProcedure;
        //            sqlCommand.Parameters.AddWithValue("@moveContract", contract);
        //            sqlCommand.Parameters.AddWithValue("@service", movingData.GeneralInfo.ShipmentType);
        //            sqlCommand.Parameters.AddWithValue("@accountName", account);
        //            sqlCommand.Parameters.AddWithValue("@authority", authority);
        //            sqlCommand.Parameters.AddWithValue("@applicationName", "Paperwork Audit");
        //            reader = sqlCommand.ExecuteReader();
        //        }

        //        while (reader.Read())
        //        {
        //            ServicePaperworkModel audit = new ServicePaperworkModel
        //            {
        //                PaperworkGuid = reader["jkmoving_serivcepaperworkid"].ToString(),
        //                PaperworkName = reader["jkmoving_name"].ToString(),
        //            };

        //            // Business Rule: Only POV moves should have the Vehicle Condition report.
        //            // Business Rule: HHG moves shouldn't receive the Security Endorsement form.
        //            if ((movingData.GeneralInfo.Preferences.ServiceLevel != "POV" && audit.PaperworkName == "Vehicle Condition Report")
        //                || (movingData.GeneralInfo.Preferences.ServiceLevel == "HHG" && audit.PaperworkName == "UAB And HHE Security Endorsement Form"))
        //                continue;

        //            paperworkList.Add(audit);
        //        }

        //        return paperworkList;
        //    }
        //}

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
