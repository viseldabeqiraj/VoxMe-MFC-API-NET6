using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers
{
    public static class Constants
    {
        public class Tables
        {
            public const string MOVINGDATA = "MovingData";
            public const string PREFS = "Prefs";
            public const string ADDRESS = "Address";
        }

        public class ComparisonOperators
        {
            public const string EQUALTO = "=";
            public const string GREATERTHAN = ">";
            public const string LESSTHAN = "<";
        }
        
    }
}
