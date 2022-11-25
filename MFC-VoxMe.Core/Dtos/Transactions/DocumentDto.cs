using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Core.Dtos.Transactions
{
    public class DocumentDto
    {
        public byte[] File { get; set; } 
        public string DocTitle { get; set; }
    }
}
