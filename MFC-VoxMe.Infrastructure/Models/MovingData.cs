using Newtonsoft.Json;

namespace MFC_VoxMe.Infrastructure.Models
{
		public class MovingData
		{
		//public int ID { get; set; }
		public string ClientName { get; set; }
		public DateTime? Date{get;set;}
		public string JobDescription { get; set; } = String.Empty;
		public string EstimatorName { get; set; } = String.Empty;
		public string Comment { get; set; } = String.Empty;
		public string FileName { get; set; } = String.Empty;
		public string ConsigneeName { get; set; } = String.Empty;
		public string BillOfLadingNo { get; set; } = String.Empty;
		public string ExternalMFID { get; set; } = String.Empty;
		public string ClientLocalID { get; set; } = String.Empty;
		public string ClientPassportID { get; set; } = String.Empty;
		public string SourceOfRequest { get; set; } = String.Empty;
		public int State { get; set; }
		public string InUse { get; set; } = String.Empty;
		public string SurveySignature { get; set; } = String.Empty;
		public string InventorySignature { get; set; } = String.Empty;
		public string CheckerSignature { get; set; } = String.Empty;
		public string AgentJobId { get; set; } = String.Empty;
		public string AgentGroupageId { get; set; } = String.Empty;
		public string OrgID { get; set; } = String.Empty;
		public DateTime? LastAccessTime { get; set; }
		public string CoordinatorOfficeID { get; set; } = String.Empty;
		public string CoordinatorName { get; set; } = String.Empty;
		public string CoordinatorEmail { get; set; } = String.Empty;
		public string BookingAgent { get; set; } = String.Empty;
		public string BookingAgentContact { get; set; } = String.Empty;
		public string BookingAgentContactEmail { get; set; } = String.Empty;
		public string OriginAgent { get; set; } = String.Empty;
		public string? OriginAgentContact { get; set; } = String.Empty;
		public string? OriginAgentContactEmail { get; set; } = String.Empty;
		public string? DestAgent { get; set; } = String.Empty;
		public string? DestAgentContact { get; set; } = String.Empty;
		public string? DestAgentContactEmail { get; set; } = String.Empty;
		public string? ShipmentType { get; set; } = String.Empty;
		public string? RelocationCompany { get; set; } = String.Empty;
		public string? ProgressEmailList { get; set; } = String.Empty;
		public string? CompletionEmailList { get; set; } = String.Empty;
		public string? OriginContractor { get; set; } = String.Empty;
		public string? OriginContractorContact { get; set; } = String.Empty;
		public string? OriginContractorContactEmail { get; set; } = String.Empty;
		public string? DestContractor { get; set; } = String.Empty;
		public string? DestContractorContact { get; set; } = String.Empty;
		public string? DestContractorContactEmail { get; set; } = String.Empty;
		public string? BillTo { get; set; } = String.Empty;
		public string? ClientSalutation { get; set; } = String.Empty;
		public string? ClientFirstName { get; set; } = String.Empty;
		public string? ConsigneeSalutation { get; set; } = String.Empty;
		public string? ConsigneeFirstName { get; set; } = String.Empty;
		public int? TripNumber { get; set; }
		public string? Manager { get; set; } = String.Empty;
		public string? DriverSignature { get; set; } = String.Empty;
		public string? DestInventorySignature { get; set; } = String.Empty;
		public string? DestDriverSignature { get; set; } = String.Empty;
		public bool? Hold { get; set; }
		public string? HoldBy { get; set; } = String.Empty;
		public string? HoldReason { get; set; } = String.Empty;
		public string? GoBy { get; set; } = String.Empty;
		public string? GoReason { get; set; } = String.Empty;
		public DateTime? GoDate { get; set; } = null;
		public string? MoveInAgentSignature { get; set; } = String.Empty;
		public string? MoveInClientSignature { get; set; } = String.Empty;
		public string? MoveOutAgentSignature { get; set; } = String.Empty;
		public string? MoveOutClientSignature { get; set; } = String.Empty;
		public string? MoveInAgentName { get; set; } = String.Empty;
		public string? MoveOutAgentName { get; set; } = String.Empty;
	}
	
	}