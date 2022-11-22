using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Models
{
    public class Prefs
    {
		public int MovingDataID { get; set; }
		public byte PrefferedLanguageID { get; set; }
		public DateTime PackingDate { get; set; }
		public byte ServiceTypeID { get; set; }
		public string Comment { get; set; }
		public string ItemsPath { get; set; }
		//public DateTime RealArrivalDate { get; set; }
		public DateTime DeliveryDate { get; set; }
		//public DateTime SurveyDate { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime PackingFinishDate { get; set; }
		
	}
}
