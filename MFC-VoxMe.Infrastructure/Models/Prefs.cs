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
		public byte? PrefferedLanguageID { get; set; }
		public DateTime? PackingDate { get; set; }
		public byte? ServiceTypeID { get; set; }
		public string? Comment { get; set; }
		public DateTime? VacationDate { get; set; }
		public DateTime? DepartureDate { get; set; }
		public string? ItemsPath { get; set; }
		public DateTime? PromisedArrivalDate { get; set; }
		public DateTime? RealArrivalDate { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public DateTime? SurveyDate { get; set; }
		public DateTime? CreationDate { get; set; }
		public DateTime? PackingFinishDate { get; set; }
		public DateTime? DeliveryFinishDate { get; set; }
		public DateTime? ExportCustomsDate { get; set; }
		public DateTime? OriginStorageInDate { get; set; }
		public DateTime? OriginStorageOutDate { get; set; }
		public DateTime? DestCustomsInDate { get; set; }
		public DateTime? DestCustomsOutDate { get; set; }
		public DateTime? DestStorageInDate { get; set; }
		public DateTime? DestStorageOutDate { get; set; }
		public DateTime? CompletionDate { get; set; }
		public DateTime? SurveyEndDate { get; set; }

	}
}
