using MFC_VoxMe_API.Profiles;
using Newtonsoft.Json;

namespace MFC_VoxMe_API.Models
{
		public class MovingData
		{
			public string? ClientName { get; set; }
			public DateTime? Date { get; set; }
			public string? JobDescription { get; set; }
			public string? EstimatorName { get; set; }
			public string? Comment { get; set; }
			public string? FileName { get; set; }
			public string? BillOfLadingNo { get; set; }
			public string? ExternalMFID { get; set; }
			public int? State { get; set; }
			public string? OrgID { get; set; }
			public DateTime? LastAccessTime { get; set; }
			public string? CoordinatorName { get; set; }
			public string? CoordinatorEmail { get; set; }
			public string? BookingAgent { get; set; }
			public string? BookingAgentContact { get; set; }
			public string? BookingAgentContactEmail { get; set; }
			public string? OriginAgent { get; set; }
			public string? ShipmentType { get; set; }
			public string? ClientSalutation { get; set; }
			public string? ClientFirstName { get; set; }
			public int? TripNumber { get; set; }
			public string? Manager { get; set; }
			public bool? Hold { get; set; }
		}
	
}