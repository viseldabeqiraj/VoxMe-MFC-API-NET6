namespace MFC_VoxMe_API.Dtos.Jobs
{
    public class CreateJobDto
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public Client client { get; set; }
        public string externalRef { get; set; }
        public string handlingDivision { get; set; }
        public string jobStatus { get; set; }
        public string jobType { get; set; }
        public ManagedBy managedBy { get; set; }
        public string serviceLevel { get; set; }
        public string serviceType { get; set; }
        public string sourceOfInquiry { get; set; }
        public string bookingType { get; set; }
        public string loadType { get; set; }
        public string transportMode { get; set; }
        public Booker booker { get; set; }
        public BookerPerson bookerPerson { get; set; }
        public Account account { get; set; }
        public AccountPerson accountPerson { get; set; }
        public OriginAddress originAddress { get; set; }
        public OriginPartyContact originPartyContact { get; set; }
        public DestinationAddress destinationAddress { get; set; }
        public DestinationPartyContact destinationPartyContact { get; set; }
        public string instructionsCrewOrigin { get; set; }
        public string instructionsCrewDestination { get; set; }
        public InventoryData inventoryData { get; set; }
        public string jobCreationWebhookUrl { get; set; }
        public string jobStatusUpdateWebhookUrl { get; set; }
        public class Account
        {
            public string code { get; set; }
            public string legalName { get; set; }
            public string partyType { get; set; }
        }

        public class AccountPerson
        {
            public string code { get; set; }
            public string partyCode { get; set; }
            public PersonDetails personDetails { get; set; }
        }

        public class AddressDetails
        {
            public string city { get; set; }
            public string country { get; set; }
            public string street1 { get; set; }
            public string street2 { get; set; }
            public string street3 { get; set; }
            public string area { get; set; }
            public string zip { get; set; }
            public string floor { get; set; }
            public string notes { get; set; }
        }

        public class Booker
        {
            public string code { get; set; }
            public string legalName { get; set; }
            public string partyType { get; set; }
        }

        public class BookerPerson
        {
            public string code { get; set; }
            public string partyCode { get; set; }
            public PersonDetails personDetails { get; set; }
        }

        public class Client
        {
            public string code { get; set; }
            public string legalName { get; set; }
            public string partyType { get; set; }
        }

        public class ConditionAfterService
        {
            public string value { get; set; }
        }

        public class ConditionBeforeService
        {
            public string value { get; set; }
        }

        public class ContactDetails
        {
            public string value { get; set; }
        }

        public class DestinationAddress
        {
            public string partyCode { get; set; }
            public AddressDetails addressDetails { get; set; }
        }

        public class DestinationPartyContact
        {
            public string code { get; set; }
            public string partyCode { get; set; }
            public PersonDetails personDetails { get; set; }
        }

        public class InventoryData
        {
            public int firstLabelNr { get; set; }
            public int grossVolume { get; set; }
            public int grossWeight { get; set; }
            public int lastLabelNr { get; set; }
            public int piecesNr { get; set; }
            public int value { get; set; }
            public int volume { get; set; }
            public int weight { get; set; }
            public string labelColor { get; set; }
            public string firstTag { get; set; }
            public List<Room> rooms { get; set; }
            public List<Packer> packers { get; set; }
            public List<LoadingUnit> loadingUnits { get; set; }
            public List<Piece> pieces { get; set; }
        }

        public class Item
        {
            public string value { get; set; }
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

        public class ManagedBy
        {
            public string code { get; set; }
            public string partyCode { get; set; }
            public PersonDetails personDetails { get; set; }
        }

        public class OriginAddress
        {
            public string partyCode { get; set; }
            public AddressDetails addressDetails { get; set; }
        }

        public class OriginPartyContact
        {
            public string code { get; set; }
            public string partyCode { get; set; }
            public PersonDetails personDetails { get; set; }
        }

        public class Packer
        {
            public bool isForeman { get; set; }
            public string packer { get; set; }
        }

        public class PersonDetails
        {
            public ContactDetails contactDetails { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string preferredLanguage { get; set; }
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
            public string packerName { get; set; }
            public string roomName { get; set; }
            public string tag { get; set; }
            public double @void { get; set; }
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
