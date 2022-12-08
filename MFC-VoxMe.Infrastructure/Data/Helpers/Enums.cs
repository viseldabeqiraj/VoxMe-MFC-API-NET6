using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Data.Helpers
{
    public class Enums
    {
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
            public enum TransactionOnSiteStatus
            {
                GeneratePaperwork = 23,
                Completed = 24,
                SubmitTimesheets = 21
            }

        }


    }
}
