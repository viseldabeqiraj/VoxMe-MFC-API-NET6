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
            public const string ROOMS = "Rooms";
            public const string PACKERS = "Packers";
            public const string ITEMS = "Items";
            public const string SKIDS = "Skids";
            public const string SKIDTYPES = "SkidTypes";
            public const string PIECES = "Pieces";
            public const string PACKERSTIMESHEETS = "PackersTimesheets";
            public const string MATERIALS = "Materials";
            public const string BOXES = "Boxes";
        }

        public class ComparisonOperators
        {
            public const string EQUALTO = "=";
            public const string GREATERTHAN = ">";
            public const string LESSTHAN = "<";
        }

    }
}
