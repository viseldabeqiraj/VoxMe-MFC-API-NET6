using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Models
{
    public class Packers
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public int MovingDataID { get; set; }
        public bool IsForeman { get; set; }
    }
}
