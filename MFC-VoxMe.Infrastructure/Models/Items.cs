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
		public string? Name { get; set; } = String.Empty;
		public string? Type { get; set; } = String.Empty;
		public double Volume { get; set; }
		public int BoxType { get; set; }
		public string? Value { get; set; } = String.Empty;
		public short Qty { get; set; }
		public int Condition { get; set; } //byte
		public string Make { get; set; } = String.Empty;
		public string Model { get; set; } = String.Empty;
		public string? Year { get; set; } = String.Empty;
		public string? SerialNumber { get; set; } = String.Empty;
		public Single Width { get; set; }
		public Single Height { get; set; }
		public Single Length { get; set; }
		public bool IsPart { get; set; }
		public bool Dismantle { get; set; }
		public bool SpecialContainer { get; set; }
		public DateTime? LastChange { get; set; }
		public int LastChangeAuthor { get; set; }
		public string? PictureTitle { get; set; } = String.Empty;
		public string? PictureAuthor { get; set; } = String.Empty;
		public string? PictureYear { get; set; } = String.Empty;
		public string? CarpetYear { get; set; } = String.Empty;
		public string CarpetSerficeSize { get; set; } = String.Empty;
		public string Comment { get; set; } = String.Empty;
		public string PictureFileName { get; set; } = String.Empty;
		public string temp { get; set; } = String.Empty;
		public bool IsValuable { get; set; }
		public string MaterialsDesc { get; set; } = String.Empty;
		public string CountryOrigin { get; set; } = String.Empty;
		public string HarmCode { get; set; } = String.Empty;
		public string ValuationCurrency { get; set; } = String.Empty;
		public string ItemStatus { get; set; } = String.Empty;
		public string TIEntryNumber { get; set; } = String.Empty;
		public double HammerPrice { get; set; }
		public double VatOnHammerPrice { get; set; }
		public double VatRateOnHammerPrice { get; set; }
		public double BuyerPremium { get; set; }
		public double VatOnBuyerPremium { get; set; }
		public double VatRateOnBuyerPremium { get; set; }
		public DateTime? CustomsStatusDate { get; set; }
		public double VatRate { get; set; }
		public bool Assemble { get; set; }
		public string? PictureAuthorFirstName { get; set; } = String.Empty;
		public string? CustomsItemStatus { get; set; } = String.Empty;
		public bool Bonded { get; set; }
		public bool TA { get; set; }
		public bool InsuredByClient { get; set; }
		public string? RegistarNotes { get; set; } = String.Empty;
		public string? MoveInComment { get; set; } = String.Empty;
		public string? MoveOutComment { get; set; } = String.Empty;
		public string? MoveInPhotoFileName { get; set; } = String.Empty;
		public string? MoveOutPhotoFileName { get; set; }
	}
}
