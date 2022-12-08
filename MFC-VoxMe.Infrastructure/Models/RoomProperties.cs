using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Models
{
    public class RoomProperties
    {
		public int RoomID { get; set; }
		public int MovingDataId { get; set; }
		public string? Description { get; set; }
		public string? MoveInCondition { get; set; }
		public string? MoveOutCondition { get; set; }
		public string? MoveInPictures { get; set; }
		public string? MoveOutPictures { get; set; }
	}
}
