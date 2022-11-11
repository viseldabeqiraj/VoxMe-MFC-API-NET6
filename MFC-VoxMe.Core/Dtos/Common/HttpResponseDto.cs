using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Core.Dtos.Common
{
    public  class HttpResponseDto<T>
    {
        public HttpStatusCode responseStatus { get; set; }
        public T? dto { get; set; }
        public string Message { get; set; } = String.Empty;
    }

}
