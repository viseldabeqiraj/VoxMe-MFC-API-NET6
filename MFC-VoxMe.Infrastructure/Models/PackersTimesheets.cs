using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Models
{
    public class PackersTimesheets
    {
		public int PackerId { get; set; }
		public int MovingDataId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public Single Break1Duration { get; set; }
		public Single Break2Duration { get; set; }
		public Single Break3Duration { get; set; }
	}
}
