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
		public string Barcode { get; set; }
		public int TypeID { get; set; }
		public double Volume { get; set; }
		public string SerialNumber { get; set; }
		public string SealNumber { get; set; }
		public string BillOfLading { get; set; }
		public string ForwardingAgent { get; set; }
		public string ForwardingAgentRef { get; set; }
		public string DestAgent { get; set; }
		public string DestAgentRef { get; set; }
		public string LoadType { get; set; }
		public string ShippingLine { get; set; }
		public string Vessel { get; set; }
		public string POE { get; set; }
		public DateTime ETD { get; set; }
		public DateTime ETA { get; set; }
		public DateTime ETAW { get; set; }
		public DateTime DateOfUnloading { get; set; }
		public int NumOfPieces { get; set; }
		public int Weight { get; set; }
		public string ContentDesc { get; set; }
		public string POL { get; set; }
		public string Voyage { get; set; }
		public string BookingRef { get; set; }
		public string POD { get; set; }
		public double GrossVolume { get; set; }
		public double ChargableVolume { get; set; }
		public double GrossWeight { get; set; }
		public double ChargableWeight { get; set; }
		public string ShippingLineCode { get; set; }
		public string Trucker { get; set; }
		public double THC { get; set; }
		public double Duty { get; set; }
		public double Demurrage { get; set; }
		public double Detention { get; set; }
		public double Storage { get; set; }
		public double OtherCharges { get; set; }
		public string PictureFileName { get; set; }
		public string CustomsOffice { get; set; }
		public string HSCode { get; set; }
		public string UOM { get; set; }
		public string Location { get; set; }
		public DateTime ETR { get; set; }
		public DateTime CAD { get; set; }
		public DateTime DOCCUTOFF { get; set; }
		public DateTime PORTCUTOFF { get; set; }
		public string Handler { get; set; }
	}
}
