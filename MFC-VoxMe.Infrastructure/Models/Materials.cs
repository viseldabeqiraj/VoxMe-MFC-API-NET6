using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Models
{
    public class Materials
    {
        public string? BoxType { get; set; } = string.Empty;
        public int MovingDataID { get; set; }
        public double QtyTaken { get; set; }
        public double QtyReturned { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? Value { get; set; } = string.Empty;
    }
}
