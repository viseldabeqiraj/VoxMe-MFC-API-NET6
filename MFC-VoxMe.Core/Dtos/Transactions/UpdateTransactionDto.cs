namespace MFC_VoxMe_API.Dtos.Transactions
{
    public class UpdateTransactionDto
    {
        public string originParty { get; set; }
        public OriginAddress originAddress { get; set; }
        public OriginPartyContact originPartyContact { get; set; }
        public string destinationParty { get; set; }
        public DestinationAddress destinationAddress { get; set; }
        public DestinationPartyContact destinationPartyContact { get; set; }
        public string transactionStatus { get; set; }
        public string onsiteStatus { get; set; }
        public string managedBy { get; set; }
        public string instructionsCrewOrigin { get; set; }
        public string instructionsCrewDestination { get; set; }
        public List<string> services { get; set; }
        public List<QuestionnaireQuestion> questionnaireQuestions { get; set; }
        public List<AuxService> auxServices { get; set; }
        public List<LoadingUnit> loadingUnits { get; set; }
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

        public class AuxService
        {
            public string name { get; set; }
            public int numericValue { get; set; }
            public string stringValue { get; set; }
            public string listValues { get; set; }
            public bool booleanValue { get; set; }
            public DateTime dateValue { get; set; }
            public string photoValue { get; set; }
            public string signatureValue { get; set; }
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

        public class LoadingUnit
        {
            public string uniqueId { get; set; }
            public LoadingUnitDetails loadingUnitDetails { get; set; }
        }

        public class LoadingUnitDetails
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
            public string uniqueId { get; set; }
            public string unitType { get; set; }
            public int labelNr { get; set; }
            public string sealNumber { get; set; }
            public string warehouseLocation { get; set; }
            public string storageUnit { get; set; }
            public DateTime dateIn { get; set; }
            public DateTime dateOut { get; set; }
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

        public class PersonDetails
        {
            public ContactDetails contactDetails { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string preferredLanguage { get; set; }
            public string salutation { get; set; }
        }

        public class QuestionnaireQuestion
        {
            public string name { get; set; }
            public int numericValue { get; set; }
            public string stringValue { get; set; }
            public string listValues { get; set; }
            public bool booleanValue { get; set; }
            public DateTime dateValue { get; set; }
            public string photoValue { get; set; }
            public string signatureValue { get; set; }
        }
    }
}
