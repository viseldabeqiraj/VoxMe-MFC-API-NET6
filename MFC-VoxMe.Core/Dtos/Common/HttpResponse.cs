using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Core.Dtos.Common
{
    public  class HttpResponse<T> where T : class
    {
        public HttpStatusCode responseStatus { get; set; }
        public T dto { get; set; }

    }
}
