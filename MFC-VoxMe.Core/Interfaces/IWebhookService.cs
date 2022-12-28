using MFC_VoxMe.Core.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Core.Interfaces
{
    public interface IWebhookService
    {
        Task<HttpResponseDto<string>> PostVoxmeStatus(string job_number, string move_id, string status);
    }
}
