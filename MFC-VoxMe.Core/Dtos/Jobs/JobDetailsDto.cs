namespace MFC_VoxMe_API.Dtos.Jobs
{
    public class JobDetailsDto
    {
            public string externalRef { get; set; }
            public string uniqueId { get; set; }
            public string jobType { get; set; }
            public string jobStatus { get; set; }
            public List<object> documents { get; set; }
            public JobInventory jobInventory { get; set; }
        
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
            public List<object> loadingUnits { get; set; }
            public List<Piece> pieces { get; set; }
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
            public List<object> items { get; set; }
        }

        public class Room
        {
            public string name { get; set; }
            public string notes { get; set; }
            public string roomType { get; set; }
            public object conditionBeforeService { get; set; }
            public object conditionAfterService { get; set; }
            public List<object> roomElements { get; set; }
        }

     

    }
}
