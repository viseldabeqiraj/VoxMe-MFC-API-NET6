
namespace MFC_VoxMe_API.Dtos.Jobs
{
    public class CreateJobDto
    {
        public Client client { get; set; } = new Client();
        public ClientPerson clientPerson { get; set; } = new ClientPerson();
        public string? externalRef { get; set; }
        public string handlingDivision { get; set; } = "JKMOVING";
        public string jobStatus { get; set; } = "Enum.Status.PackingSet";
        public string? jobType { get; set; }
        public ManagedBy managedBy { get; set; } = new ManagedBy();
        public string? serviceLevel { get; set; }
        public string? serviceType { get; set; }
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
        public InventoryData inventoryData { get; set; } = new InventoryData();
        public string? jobCreationWebhookUrl { get; set; }
        public string? jobStatusUpdateWebhookUrl { get; set; }

        public class Account
        {
            public string? code { get; set; }
            public string? legalName { get; set; }
            public string? partyType { get; set; }
        }

        public class AccountPerson
        {
            public string? code { get; set; }
            public string? partyCode { get; set; }
            public PersonDetails? personDetails { get; set; } = new PersonDetails();
        }

        public class AddressDetails
        {
            public string city { get; set; }
            public string country { get; set; }
            public string? street1 { get; set; }
            public string? street2 { get; set; }
            public string? street3 { get; set; }
            public string area { get; set; }
            public string zip { get; set; }
            public string floor { get; set; }
            public string notes { get; set; }
        }

        public class Booker
        {
            public string code { get; set; } = "JKMOVING";
            public string legalName { get; set; } = "Jk Moving Services";
            public string partyType { get; set; } = "Enum.PartyType.Division";
        }

        public class BookerPerson
        {
            public string code { get; set; }
            public string partyCode { get; set; } = "JKMOVING";
            public PersonDetails personDetails { get; set; } = new PersonDetails();
        }

        public class Client
        {
            public string code { get; set; }
            //[MatchParent("ClientFirstName")] //TODO: legalName: GeneralInfo/ClientFirstName + GeneralInfo/Name
            public string legalName { get; set; }
            public string partyType { get; set; } = "Enum.PartyType.Transferee";
        }

        public class ClientPerson
        {
            public string code { get; set; }
            public string partyCode { get; set; }
            public PersonDetails personDetails { get; set; } = new PersonDetails();
        }

        public class ContactDetails
        {
            public string email { get; set; }
            public string mobilePhone { get; set; }
            public string homePhone { get; set; }
            public string workPhone { get; set; }
        }

        public class DestinationAddress
        {
            public string partyCode { get; set; }
            public AddressDetails addressDetails { get; set; } = new AddressDetails();
        }

        public class DestinationPartyContact
        {
            public string code { get; set; }
            public string partyCode { get; set; }
            public PersonDetails personDetails { get; set; } = new PersonDetails();
        }

        public class InventoryData
        {
            public int firstLabelNr { get; set; }
            public double grossVolume { get; set; }
            public double grossWeight { get; set; }
            public int lastLabelNr { get; set; }
            public int piecesNr { get; set; }
            public double value { get; set; }
            public double volume { get; set; }
            public double weight { get; set; }
            public string? labelColor { get; set; }
            public string? firstTag { get; set; }
            public List<Room> rooms { get; set; } 
            public List<Packer> packers { get; set; }
            public double? loadingUnits { get; set; }
            public List<Piece>? pieces { get; set; }
        }

        public class Item
        {
            public string? comment { get; set; }
            public string? condition { get; set; }
            public string? conditionLocation { get; set; }
            public string? countryOrigin { get; set; }
            public bool dismantle { get; set; }
            public double height { get; set; }
            public bool isCrated { get; set; }
            public bool isPart { get; set; }
            public bool isValuable { get; set; }
            public string? itemCategory { get; set; }
            public string? itemName { get; set; }
            public string? itemNr { get; set; }
            public string? itemType { get; set; }
            public double? length { get; set; }
            public string? make { get; set; }
            public string? materialsDesc { get; set; }
            public string? model { get; set; }
            public string? photos { get; set; }
            public string? pictureAuthor { get; set; }
            public string? pictureTitle { get; set; }
            public string? pictureYear { get; set; }
            public int qty { get; set; }
            public string sealNumber { get; set; }
            public string serialNumber { get; set; }
            public string valuationCurrency { get; set; }
            public double value { get; set; }
            public double volume { get; set; }
            public double width { get; set; }
            public string year { get; set; }
        }

        public class ManagedBy
        {
            public string code { get; set; }
            public string partyCode { get; set; } = "JKMOVING";
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
            public string preferredLanguage { get; set; } = "en-us";
            public string salutation { get; set; }
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
            public string name { get; set; }
            public string notes { get; set; }
            public string roomType { get; set; }
            public object conditionBeforeService { get; set; }
            public object conditionAfterService { get; set; }
            public List<object> roomElements { get; set; }
        }

    }
}
