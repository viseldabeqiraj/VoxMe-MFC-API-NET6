using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers
{
    public class SelectFrom
    {
        public string table { get; set; }
        public Dictionary<string, object>? whereClause { get; set; }
        public string columns { get; set; }
        public string comparisonOperator { get; set; }
        public IEnums.logOperator? logOperator = null;
        public IEnums.functions? function = null;


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
