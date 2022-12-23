using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using MimeKit.Text;
using MailKit.Security;

namespace MFC_VoxMe.Core.Dtos.Email
{
    public class EmailMessage
    {
        public string EmailToId { get; set; }
        public string EmailToName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    
    }
}
