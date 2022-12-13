
namespace MFC_VoxMe_API.Dtos.Jobs
{
    public class CreateJobDto
    {
        public Client client { get; set; } = new Client();
        public ClientPerson clientPerson { get; set; } = new ClientPerson();
        public string? externalRef { get; set; } = String.Empty;
        public string handlingDivision { get; set; } = "JKMOVINGTEST002";
        public string jobStatus { get; set; } = "Enum.Status.PackingSet";
        public string? jobType { get; set; } = String.Empty;
        public ManagedBy managedBy { get; set; } = new ManagedBy();
        public string? serviceLevel { get; set; } = String.Empty;
        public string? serviceType { get; set; } = String.Empty;
        public string sourceOfInquiry { get; set; } = "Enum.SourceOfInquiry.Email";
        public string bookingType { get; set; } = "Enum.BookingType.Private";
        public string loadType { get; set; } = "Enum.LoadType.FTL";
        public string transportMode { get; set; } = "Enum.TransportMode.Truck";
        public Booker booker { get; set; } = new Booker();
        public BookerPerson bookerPerson { get; set; } = new BookerPerson();
        public Account account { get; set; }
        public AccountPerson accountPerson { get; set; }
        public OriginAddress originAddress { get; set; } = new OriginAddress();
        public OriginPartyContact originPartyContact { get; set; } = new OriginPartyContact();
        public DestinationAddress destinationAddress { get; set; } = new DestinationAddress();
        public DestinationPartyContact destinationPartyContact { get; set; } = new DestinationPartyContact();
        public string? instructionsCrewOrigin { get; set; }
        public string? instructionsCrewDestination { get; set; }
        public InventoryData inventoryData { get; set; } //= new InventoryData();
        public string? jobCreationWebhookUrl { get; set; } = String.Empty;
        public string? jobStatusUpdateWebhookUrl { get; set; } = String.Empty;

        public class Account
        {
            public string? code { get; set; } = String.Empty;
            public string? legalName { get; set; } = String.Empty;
            public string? partyType { get; set; } = "Enum.PartyType.ACCOUNT";
        }

        public class AccountPerson
        {
            public string? code { get; set; } = String.Empty;
            public string? partyCode { get; set; } = String.Empty;
            public PersonDetails? personDetails { get; set; } = new PersonDetails();
        }

        public class AddressDetails
        {
            public string city { get; set; } = String.Empty;
            public string country { get; set; } = String.Empty;
            public string? street1 { get; set; } = String.Empty;
            public string? street2 { get; set; } = String.Empty;
            public string? street3 { get; set; } = String.Empty;
            public string area { get; set; } = String.Empty;
            public string zip { get; set; } = String.Empty;
            public string floor { get; set; } = String.Empty;
            public string notes { get; set; } = String.Empty;
        }

        public class Booker
        {
            public string code { get; set; } = "JKMOVINGTEST002";
            public string legalName { get; set; } = "JK Moving Services";
            public string partyType { get; set; } = "Enum.PartyType.Division";
        }

        public class BookerPerson
        {
            public string code { get; set; } = String.Empty;
            public string partyCode { get; set; } = "JKMOVINGTEST002";
            public PersonDetails personDetails { get; set; } = new PersonDetails();
        }

        public class Client
        {
            public string code { get; set; } = String.Empty;
            //[MatchParent("ClientFirstName")] //TODO: legalName: GeneralInfo/ClientFirstName + GeneralInfo/Name
            public string legalName { get; set; } = String.Empty;
            public string partyType { get; set; } = "Enum.PartyType.Transferee";
        }

        public class ClientPerson
        {
            public string code { get; set; } = String.Empty;
            public string partyCode { get; set; } = String.Empty;
            public PersonDetails personDetails { get; set; } = new PersonDetails();
        }

        public class ContactDetails
        {
            public string email { get; set; } = String.Empty;
            public string mobilePhone { get; set; } = String.Empty;
            public string homePhone { get; set; } = String.Empty;
            public string workPhone { get; set; } = String.Empty;
        }

        public class DestinationAddress
        {
            public string partyCode { get; set; } = String.Empty;
            public AddressDetails addressDetails { get; set; } = new AddressDetails();
        }

        public class DestinationPartyContact
        {
            public string code { get; set; } = String.Empty;
            public string partyCode { get; set; } = String.Empty;
            public PersonDetails personDetails { get; set; } = new PersonDetails();
        }

        public class InventoryData
        {
            public int firstLabelNr { get; set; } = 0;
            public double grossVolume { get; set; } = 0.0;
            public double grossWeight { get; set; } = 0.0;
            public int lastLabelNr { get; set; } = 0;
            public int piecesNr { get; set; } = 0;
            public double value { get; set; } = 0.0;
            public double volume { get; set; } = 0.0;
            public double weight { get; set; } = 0.0;
            public string? labelColor { get; set; } = String.Empty;
            public string? firstTag { get; set; } = "001";
            public List<Room> rooms { get; set; }
            public List<Packer> packers { get; set; }
            public List<LoadingUnit>? loadingUnits { get; set; }
            public List<Piece>? pieces { get; set; }
        }

        public class LoadingUnit
        {
            public string uniqueId { get; set; } = String.Empty;
            public string unitType { get; set; } = String.Empty;
            public string serialNumber { get; set; } = String.Empty;
            public int labelNr { get; set; }
            public string sealNumber { get; set; } = String.Empty;
            public string warehouseLocation { get; set; } = String.Empty;
            public string storageUnit { get; set; } = String.Empty;
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
            public string photos { get; set; } = String.Empty;
            public DateTime dateIn { get; set; }
            public DateTime dateOut { get; set; }

        }

        public class Item
        {
            public string? comment { get; set; } = String.Empty;
            public string? condition { get; set; } = String.Empty;
            public string? conditionLocation { get; set; } = String.Empty;
            public string? countryOrigin { get; set; } = String.Empty;
            public bool dismantle { get; set; }
            public double height { get; set; }
            public bool isCrated { get; set; }
            public bool isPart { get; set; }
            public bool isValuable { get; set; }
            public string? itemCategory { get; set; } = String.Empty;
            public string? itemName { get; set; } = String.Empty;
            public string? itemNr { get; set; } = String.Empty;
            public string? itemType { get; set; } = String.Empty;
            public double? length { get; set; }
            public string? make { get; set; } = String.Empty;
            public string? materialsDesc { get; set; } = String.Empty;
            public string? model { get; set; } = String.Empty;
            public string? photos { get; set; } = String.Empty;
            public string? pictureAuthor { get; set; } = String.Empty;
            public string? pictureTitle { get; set; } = String.Empty;
            public string? pictureYear { get; set; } = String.Empty;
            public int qty { get; set; }
            public string sealNumber { get; set; } = String.Empty;
            public string serialNumber { get; set; } = String.Empty;
            public string valuationCurrency { get; set; } = String.Empty;
            public double value { get; set; }
            public double volume { get; set; }
            public double width { get; set; }
            public string year { get; set; } = String.Empty;
        }

        public class ManagedBy
        {
            public string code { get; set; }
            public string partyCode { get; set; } = "JKMOVINGTEST002";
            public PersonDetails personDetails { get; set; } = new PersonDetails();
        }

        public class OriginAddress
        {
            public string partyCode { get; set; }
            public AddressDetails addressDetails { get; set; } = new AddressDetails();
        }

        public class OriginPartyContact
        {
            public string code { get; set; }
            public string partyCode { get; set; }
            public PersonDetails personDetails { get; set; } = new PersonDetails();
        }

        public class Packer
        {
            public string packer { get; set; }
            public bool isForeman { get; set; }
        }

        public class PersonDetails
        {
            public ContactDetails contactDetails { get; set; } = new ContactDetails();
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string preferredLanguage { get; set; } = "english";
            public string salutation { get; set; } =  String.Empty;
        }

        public class Piece
        {
            public string barcode { get; set; }
            public double height { get; set; }
            public string labelNr { get; set; }
            public double length { get; set; }
            public int packageQty { get; set; }
            public string packageType { get; set; }
            public double packageUnitCost { get; set; }
            public string packerName { get; set; }
            public bool pbo { get; set; }
            public string roomName { get; set; }
            public string tag { get; set; }
            public bool @void { get; set; }
            public double volume { get; set; }
            public double weight { get; set; }
            public double width { get; set; }
            public object loadUnitUniqueId { get; set; }
            public List<Item> items { get; set; }
        }

        public class Room
        {
            public string name { get; set; } = String.Empty;
            public string notes { get; set; } = String.Empty;
            public string roomType { get; set; } = String.Empty;
            public object conditionBeforeService { get; set; } 
            public object conditionAfterService { get; set; }
            public List<object> roomElements { get; set; }
        }

    }
}
