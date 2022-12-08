using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Models
{
    public class Rooms
    {
		public short ID { get; set; }
		public int MovingDataID { get; set; }
		public string? Name { get; set; }
		public string? NickName { get; set; }
		public int NumOfPeople { get; set; }
		public int EstimatedVolume { get; set; }
		public string? Description { get; set; }
		public string? MoveInCondition { get; set; }
		public string? MoveOutCondition { get; set; }
		public string? MoveInPictures { get; set; }
		public string? MoveOutPictures { get; set; }
	}
}
