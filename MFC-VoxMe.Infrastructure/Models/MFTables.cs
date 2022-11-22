using MFC_VoxMe_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Models
{
    public interface IMFTables { }
    public class MFTables //: IMFTables
    {
        public Address Address { get; set; }
        public MovingData MovingData { get; set; }
        public Prefs Prefs { get; set; }
    }

}
