namespace MFC_VoxMe_API.Dtos.Jobs
{
    public class JobDetailsDto
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public string externalRef { get; set; }
        public string jobStatus { get; set; }
        public string jobType { get; set; }
        public string uniqueId { get; set; }
        public List<Document> documents { get; set; }
        public JobInventory jobInventory { get; set; }

        public class ConditionAfterService
        {
            public string value { get; set; }
        }

        public class ConditionBeforeService
        {
            public string value { get; set; }
        }

        public class Document
        {
            public string docTitle { get; set; }
            public string fileName { get; set; }
        }

        public class Item
        {
            public string value { get; set; }
        }

        public class JobInventory
        {
            public int firstLabelNr { get; set; }
            public int grossVolume { get; set; }
            public int grossWeight { get; set; }
            public int lastLabelNr { get; set; }
            public int piecesNr { get; set; }
            public double value { get; set; }
            public int volume { get; set; }
            public int weight { get; set; }
            public string labelColor { get; set; }
            public string firstTag { get; set; }
            public List<Room> rooms { get; set; }
            public List<Packer> packers { get; set; }
            public List<LoadingUnit> loadingUnits { get; set; }
            public List<Piece> pieces { get; set; }
        }

        public class LoadingUnit
        {
            public double extHeight { get; set; }
            public double extLength { get; set; }
            public double extWidth { get; set; }
            public double grossVolume { get; set; }
            public double grossWeight { get; set; }
            public double netHeight { get; set; }
            public double netLength { get; set; }
            public double netVolume { get; set; }
            public double netWeight { get; set; }
            public double netWidth { get; set; }
            public string photos { get; set; }
            public string serialNumber { get; set; }
            public string unitType { get; set; }
            public int labelNr { get; set; }
            public string sealNumber { get; set; }
            public string warehouseLocation { get; set; }
            public string storageUnit { get; set; }
            public DateTime dateIn { get; set; }
            public DateTime dateOut { get; set; }
        }

        public class Packer
        {
            public bool isForeman { get; set; }
            public string packer { get; set; }
        }

        public class Piece
        {
            public string barcode { get; set; }
            public double height { get; set; }
            public string labelNr { get; set; }
            public double length { get; set; }
            public int packageQty { get; set; }
            public string packageType { get; set; }
            public string packerName { get; set; }
            public string roomName { get; set; }
            public string tag { get; set; }
            public bool @void { get; set; }
            public double volume { get; set; }
            public double weight { get; set; }
            public double width { get; set; }
            public string loadUnitUniqueId { get; set; }
            public List<Item> items { get; set; }
        }

        public class Room
        {
            public string name { get; set; }
            public string notes { get; set; }
            public string roomType { get; set; }
            public ConditionBeforeService conditionBeforeService { get; set; }
            public ConditionAfterService conditionAfterService { get; set; }
            public List<RoomElement> roomElements { get; set; }
        }

        public class RoomElement
        {
            public string value { get; set; }
        }
    }
}
