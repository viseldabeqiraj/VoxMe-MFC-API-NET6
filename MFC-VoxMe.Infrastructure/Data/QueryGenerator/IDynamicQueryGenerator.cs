using MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Data.QueryGenerator
{
    public interface IDynamicQueryGenerator
    {
        Task<dynamic> SelectFrom(SqlQuery<string> select);
         Task UpdateTable<T>(SqlQuery<T> update);
         Task InsertInto<T>(SqlQuery<T> insertInto);
        Task Delete<T>(SqlQuery<T> delete);
    }
}
