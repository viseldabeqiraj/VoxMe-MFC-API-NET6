using Newtonsoft.Json;

namespace MFC_VoxMe.Infrastructure.Models
{
		public class MovingData
		{
		public int ID { get; set; }
		public string ClientName { get; set; }
		public DateTime Date{get;set;}
		public string JobDescription { get; set; }
		public string EstimatorName { get; set; }
		public string Comment { get; set; }
		public string FileName { get; set; }
		public string ConsigneeName { get; set; }
		public string BillOfLadingNo { get; set; }
		public string ExternalMFID { get; set; }
		public string ClientLocalID { get; set; }
		public string ClientPassportID { get; set; }
		public string SourceOfRequest { get; set; }
		public int State { get; set; }
		public string InUse { get; set; }
		public string SurveySignature { get; set; }
		public string InventorySignature { get; set; }
		public string CheckerSignature { get; set; }
		public string AgentJobId { get; set; }
		public string AgentGroupageId { get; set; }
		public string OrgID { get; set; }
		public DateTime LastAccessTime { get; set; }
		public string CoordinatorOfficeID { get; set; }
		public string CoordinatorName { get; set; }
		public string CoordinatorEmail { get; set; }
		public string BookingAgent { get; set; }
		public string BookingAgentContact { get; set; }
		public string BookingAgentContactEmail { get; set; }
		public string OriginAgent { get; set; }
		public string OriginAgentContact { get; set; }
		public string OriginAgentContactEmail { get; set; }
		public string DestAgent { get; set; }
		public string DestAgentContact { get; set; }
		public string DestAgentContactEmail { get; set; }
		public string ShipmentType { get; set; }
		public string RelocationCompany { get; set; }
		public string ProgressEmailList { get; set; }
		public string CompletionEmailList { get; set; }
		public string OriginContractor { get; set; }
		public string OriginContractorContact { get; set; }
		public string OriginContractorContactEmail { get; set; }
		public string DestContractor { get; set; }
		public string DestContractorContact { get; set; }
		public string DestContractorContactEmail { get; set; }
		public string BillTo { get; set; }
		public string ClientSalutation { get; set; }
		public string ClientFirstName { get; set; }
		public string ConsigneeSalutation { get; set; }
		public string ConsigneeFirstName { get; set; }
		public int TripNumber { get; set; }
		public string Manager { get; set; }
		public string DriverSignature { get; set; }
		public string DestInventorySignature { get; set; }
		public string DestDriverSignature { get; set; }
		public bool Hold { get; set; }
		public string HoldBy { get; set; }
		public string HoldReason { get; set; }
		public string GoBy { get; set; }
		public string GoReason { get; set; }
		public DateTime GoDate { get; set; }
		public string MoveInAgentSignature { get; set; }
		public string MoveInClientSignature { get; set; }
		public string MoveOutAgentSignature { get; set; }
		public string MoveOutClientSignature { get; set; }
		public string MoveInAgentName { get; set; }
		public string MoveOutAgentName { get; set; }
}
	
}