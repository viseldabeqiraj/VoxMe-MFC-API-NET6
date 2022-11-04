namespace MFC_VoxMe_API.Models
{
    public class Resources
    {
		public int ID { get; set; }
		public string? ResourceName { get; set; }
		public string? ResourceType { get; set; }
		public string? Qualifications { get; set; }
		public double? HourCost { get; set; }
		public int? ParentResourceID { get; set; }
		public string? EventTypeUsedFor { get; set; }
	}
}
