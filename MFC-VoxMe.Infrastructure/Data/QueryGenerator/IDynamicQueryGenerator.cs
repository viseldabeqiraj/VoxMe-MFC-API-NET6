using MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers;
using MFC_VoxMe.Infrastructure.Models;
using MFC_VoxMe_API.Dtos.Common;
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
        //Task<List<ServicePaperworkModel>> GetRequiredPaperwork(MovingDataDto movingData);
    }
}
