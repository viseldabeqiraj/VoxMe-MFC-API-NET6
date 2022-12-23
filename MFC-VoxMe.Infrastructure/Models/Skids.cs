using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Models
{
	public class Skids
	{
		public int MovingDataID { get; set; }
		public int ID { get; set; }
		public int Length { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public string Barcode { get; set; } = String.Empty;
		public int TypeID { get; set; }
		public double Volume { get; set; }
		public string SerialNumber { get; set; } = String.Empty;
		public string SealNumber { get; set; } = String.Empty;
		public string BillOfLading { get; set; } = String.Empty;
		public string ForwardingAgent { get; set; } = String.Empty;
		public string ForwardingAgentRef { get; set; } = String.Empty;
		public string DestAgent { get; set; } = String.Empty;
		public string DestAgentRef { get; set; } = String.Empty;
		public string LoadType { get; set; } = String.Empty;
		public string ShippingLine { get; set; } = String.Empty;
		public string Vessel { get; set; } = String.Empty;
		public string POE { get; set; } = String.Empty;
		public DateTime? ETD { get; set; }
		public DateTime? ETA { get; set; }
		public DateTime? ETAW { get; set; }
		public DateTime? DateOfUnloading { get; set; }
		public int NumOfPieces { get; set; }
		public int Weight { get; set; }
		public string? ContentDesc { get; set; } = String.Empty;
		public string? POL { get; set; } = String.Empty;
		public string? Voyage { get; set; } = String.Empty;
		public string? BookingRef { get; set; } = String.Empty;
		public string? POD { get; set; } = String.Empty;
		public double GrossVolume { get; set; }
		public double? ChargableVolume { get; set; }
		public double GrossWeight { get; set; }
		public double ChargableWeight { get; set; }
		public string? ShippingLineCode { get; set; } = String.Empty;
		public string? Trucker { get; set; } = String.Empty;
		public double THC { get; set; }
		public double Duty { get; set; }
		public double Demurrage { get; set; }
		public double Detention { get; set; }
		public double Storage { get; set; }
		public double OtherCharges { get; set; }
		public string PictureFileName { get; set; } = String.Empty;
		public string CustomsOffice { get; set; } = String.Empty;
		public string HSCode { get; set; } = String.Empty;
		public string UOM { get; set; } = String.Empty;
		public string Location { get; set; } = String.Empty;
		public DateTime? ETR { get; set; }
		public DateTime? CAD { get; set; }
		public DateTime? DOCCUTOFF { get; set; }
		public DateTime? PORTCUTOFF { get; set; }
		public string? Handler { get; set; } = String.Empty;
	}
}
