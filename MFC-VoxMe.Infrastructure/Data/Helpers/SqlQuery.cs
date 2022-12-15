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
        public string table { get; set; }
        public T dto { get; set; }
        public string As { get; set; } = string.Empty;
        public string whereClause { get; set; } = string.Empty;
        public string columns { get; set; }
        public string comparisonOperator { get; set; }

        public string filterQuery  { get; set; }
        public IEnums.logOperator? logOperator = null;
        public IEnums.functions? function = null;

        public static string Where(string key, string operators, object value )
        {
            return $" {key} {operators} {value} ";
        }

    }

}
