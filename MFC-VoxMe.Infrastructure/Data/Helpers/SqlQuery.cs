using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MFC_VoxMe.Infrastructure.Data.Helpers.Enums;

namespace MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers
{
    public class SqlQuery<T> 
    {
        public string Table { get; set; }
        public T Dto { get; set; }
        public string As { get; set; } = string.Empty;
        public string WhereClause { get; set; } = string.Empty;
        public string Columns { get; set; }
        public string ComparisonOperator { get; set; }

        public IEnums.logOperator? LogOperator = null;
        public IEnums.functions? Function = null;

        public static string Where(string key, string operators, object value )
        {
            return $" {key} {operators} {value} ";
        }

    }

}
