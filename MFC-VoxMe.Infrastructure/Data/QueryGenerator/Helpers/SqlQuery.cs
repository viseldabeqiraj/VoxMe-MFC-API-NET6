using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers
{
    public class SqlQuery<T> 
    {
        public string table { get; set; }
        public T dto { get; set; }
        public Dictionary<string, object> whereClause { get; set; }
        public string columns { get; set; }
        public string comparisonOperator { get; set; }
        public IEnums.logOperator? logOperator = null;
        public IEnums.functions? function = null;

        public Dictionary<string, object> Where(string key, string value)
        {
            return new Dictionary<string, object>()
            {
                {key, value},
            };
        }

    }
    public interface IEnums
    {
        public enum logOperator
        {
            AND,
            OR,
            ANY,
            EXISTS,
            NOT,
            LIKE
        }
        public enum functions
        {
            MAX,
            SUM,
            COUNT,
            AVG,
            BETWEEN
        }
    }
}
