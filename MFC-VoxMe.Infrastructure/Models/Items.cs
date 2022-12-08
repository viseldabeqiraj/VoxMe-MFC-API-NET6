using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Models
{
    public class Items
    {
		public short ID { get; set; }
		public int MovingDataID { get; set; }
		public int PieceID { get; set; }
		public bool IsInventory { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public double Volume { get; set; }
		public int BoxType { get; set; }
		public string Value { get; set; }
		public short Qty { get; set; }
		public byte Condition { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public string Year { get; set; }
		public string SerialNumber { get; set; }
		public Single Width { get; set; }
		public Single Height { get; set; }
		public Single Length { get; set; }
		public bool IsPart { get; set; }
		public bool Dismantle { get; set; }
		public bool SpecialContainer { get; set; }
		public DateTime LastChange { get; set; }
		public int LastChangeAuthor { get; set; }
		public string PictureTitle { get; set; }
		public string PictureAuthor { get; set; }
		public string PictureYear { get; set; }
		public string CarpetYear { get; set; }
		public string CarpetSerficeSize { get; set; }
		public string Comment { get; set; }
		public string PictureFileName { get; set; }
		public string temp { get; set; }
		public bool IsValuable { get; set; }
		public string MaterialsDesc { get; set; }
		public string CountryOrigin { get; set; }
		public string HarmCode { get; set; }
		public string ValuationCurrency { get; set; }
		public string ItemStatus { get; set; }
		public string TIEntryNumber { get; set; }
		public double HammerPrice { get; set; }
		public double VatOnHammerPrice { get; set; }
		public double VatRateOnHammerPrice { get; set; }
		public double BuyerPremium { get; set; }
		public double VatOnBuyerPremium { get; set; }
		public double VatRateOnBuyerPremium { get; set; }
		public DateTime CustomsStatusDate { get; set; }
		public double VatRate { get; set; }
		public bool Assemble { get; set; }
		public string PictureAuthorFirstName { get; set; }
		public string CustomsItemStatus { get; set; }
		public bool Bonded { get; set; }
		public bool TA { get; set; }
		public bool InsuredByClient { get; set; }
		public string RegistarNotes { get; set; }
		public string MoveInComment { get; set; }
		public string MoveOutComment { get; set; }
		public string MoveInPhotoFileName { get; set; }
		public string MoveOutPhotoFileName { get; set; }
	}
}
