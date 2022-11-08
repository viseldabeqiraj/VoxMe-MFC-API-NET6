using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.GlobalErrorHandling
{
    public class ErrorDetails
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Response { get; set; }
        public string? MethodName { get; set; }
        public string? ClassName { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
