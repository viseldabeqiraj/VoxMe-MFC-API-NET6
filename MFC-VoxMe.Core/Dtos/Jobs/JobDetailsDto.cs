namespace MFC_VoxMe_API.Dtos.Jobs
{
    public class JobDetailsDto
    {
            public string externalRef { get; set; }
            public string uniqueId { get; set; }
            public string jobType { get; set; }
            public string jobStatus { get; set; }
            public List<Documents> documents { get; set; }
            public JobInventory jobInventory { get; set; }
        

        public class Documents
        {
            public string fileName { get; set; }
            public string docTitle { get; set; }
        }
        public class JobInventory
        {
            public int piecesNr { get; set; }
            public int firstLabelNr { get; set; }
            public int lastLabelNr { get; set; }
            public int volume { get; set; }
            public int weight { get; set; }
            public int grossVolume { get; set; }
            public int grossWeight { get; set; }
            public double value { get; set; }
            public string labelColor { get; set; }
            public string firstTag { get; set; }
            public List<Room> rooms { get; set; }
            public List<Packer> packers { get; set; }
            public List<LoadingUnit> loadingUnits { get; set; }
            public List<Piece> pieces { get; set; }
        }

        public class LoadingUnit
        {
            public string uniqueId { get; set; }
            public string unitType { get; set; }
            public string serialNumber { get; set; }
            public int labelNr { get; set; }
            public string sealNumber { get; set; }
            public string warehouseLocation { get; set; }
            public string storageUnit { get; set; }
            public double netWidth { get; set; }
            public double netHeight { get; set; }
            public double netLength { get; set; }
            public double netVolume { get; set; }
            public double netWeight { get; set; }
            public double extWidth { get; set; }
            public double extHeight { get; set; }
            public double extLength { get; set; }
            public double grossVolume { get; set; }
            public double grossWeight { get; set; }
            public string photos { get; set; }
            public DateTime dateIn { get; set; }
            public DateTime dateOut { get; set; }
        }

        public class Packer
        {
            public string packer { get; set; }
            public bool isForeman { get; set; }
        }

        public class Piece
        {
            public string labelNr { get; set; }
            public string tag { get; set; }
            public string barcode { get; set; }
            public string packerName { get; set; }
            public string roomName { get; set; }
            public bool pbo { get; set; }
            public string packageType { get; set; }
            public int packageUnitCost { get; set; }
            public int packageQty { get; set; }
            public object loadUnitUniqueId { get; set; }
            public bool @void { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public int length { get; set; }
            public int volume { get; set; }
            public int weight { get; set; }
            public List<Item> items { get; set; }
        }

        public class Room
        {
            public string name { get; set; }
            public string notes { get; set; }
            public string roomType { get; set; }
            public InspectionCondition conditionBeforeService { get; set; }
            public InspectionCondition conditionAfterService { get; set; }
            public List<RoomElements> roomElements { get; set; }
        }

        public class InspectionCondition
        {
            public string photos { get; set; }
            public string description { get; set; }
        }

        public class RoomElements
        {
            public string name { get; set; }
            public string description { get; set; }
            public InspectionCondition conditionBeforeService { get; set; }
            public InspectionCondition conditionAfterService { get; set; }
        }

        public class Item
        {
            public string itemNr { get; set; }
            public string itemName { get; set; }
            public string itemType { get; set; }
            public string itemCategory { get; set; }
            public double volume { get; set; }
            public decimal value { get; set; }
            public string valuationCurrency { get; set; }
            public int qty { get; set; }
            public string condition { get; set; }
            public string conditionLocation { get; set; }
            public string make { get; set; }
            public string model { get; set; }
            public string year { get; set; }
            public string serialNumber { get; set; }
            public string sealNumber { get; set; }
            public double width { get; set; }
            public double height { get; set; }
            public double length { get; set; }
            public bool isPart { get; set; }
            public bool dismantle { get; set; }
            public bool isCrated { get; set; }
            public bool isValuable { get; set; }
            public string pictureTitle { get; set; }
            public string pictureAuthor { get; set; }
            public string pictureYear { get; set; }
            public string materialsDesc { get; set; }
            public string countryOrigin { get; set; }
            public string comment { get; set; }
            public string photos { get; set; }
        }



    }
}
