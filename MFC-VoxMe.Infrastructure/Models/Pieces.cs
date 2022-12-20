using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Models
{
	public class Pieces
	{
		public int ID { get; set; }
		public int MovingDataID { get; set; }
		public bool IsInventory { get; set; }
		public double Volume { get; set; }
		public double RealVolume { get; set; }
		public int BoxType { get; set; }
		public bool PBO { get; set; }
		public string Barcode { get; set; }
		public Single Width { get; set; }
		public Single Height { get; set; }
		public Single Length { get; set; }
		public double Weight { get; set; }
		public DateTime LastChange { get; set; }
		public int LastChangeAuthor { get; set; }
		public double Qty { get; set; }
		public int RoomID { get; set; }
		public string Description { get; set; }
		public int BoxQty { get; set; } //byte
		public bool NeedBox { get; set; }
		public int PackerID { get; set; }
		public int DestRoomID { get; set; }
		public int SkidID { get; set; }
		public string InvId { get; set; }
		public string LotNo { get; set; }
		public int LineItemNumber { get; set; }
		public int NumberOfElements { get; set; }
		public DateTime PhotoShootDate { get; set; }
		public string ShotBy { get; set; }
		public bool CatalogueId { get; set; }
		public string CataloguedBy { get; set; }
		public DateTime CataloguedOn { get; set; }
		public decimal ShippingCost { get; set; }
		public string ShippingTerms { get; set; }
		public decimal InsurancePremium { get; set; }
		public bool Void { get; set; }
	}
}
