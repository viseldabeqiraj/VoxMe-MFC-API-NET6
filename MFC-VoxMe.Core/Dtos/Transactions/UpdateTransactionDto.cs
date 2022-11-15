namespace MFC_VoxMe_API.Dtos.Transactions
{
    public class UpdateTransactionDto
    {
        public string originParty { get; set; } = string.Empty; 
        public OriginAddress originAddress { get; set; }
        public OriginPartyContact originPartyContact { get; set; }
        public string destinationParty { get; set; } = string.Empty;
        public DestinationAddress destinationAddress { get; set; }
        public DestinationPartyContact destinationPartyContact { get; set; }
        public string transactionStatus { get; set; } = string.Empty;
        public string onsiteStatus { get; set; } = string.Empty;
        public string managedBy { get; set; } = string.Empty;
        public string instructionsCrewOrigin { get; set; } = string.Empty;
        public string instructionsCrewDestination { get; set; } = string.Empty;
        public List<string> services { get; set; }
        public List<QuestionnaireQuestion> questionnaireQuestions { get; set; }
        public List<AuxService> auxServices { get; set; }
        public List<LoadingUnit> loadingUnits { get; set; }
        public class AddressDetails
        {
            public string city { get; set; } = string.Empty;
            public string country { get; set; } = string.Empty;
            public string street1 { get; set; } = string.Empty;
            public string street2 { get; set; } = string.Empty;
            public string street3 { get; set; } = string.Empty;
            public string area { get; set; } = string.Empty;
            public string zip { get; set; } = string.Empty;
            public string floor { get; set; } = string.Empty;
            public string notes { get; set; } = string.Empty;
        }

        public class AuxService
        {
            public string name { get; set; } = string.Empty;
            public int numericValue { get; set; } 
            public string stringValue { get; set; } = string.Empty;
            public string listValues { get; set; } = string.Empty;
            public bool booleanValue { get; set; }
            //public DateTime dateValue { get; set; }
            //public string photoValue { get; set; } = string.Empty;
            public string signatureValue { get; set; } = string.Empty;
        }

        public class ContactDetails
        {
            public string email { get; set; } = String.Empty;
            public string mobilePhone { get; set; } = "+123456789";
            public string homePhone { get; set; } = String.Empty;
            public string workPhone { get; set; } = String.Empty;
        }

        public class DestinationAddress
        {
            public string partyCode { get; set; } = string.Empty;
            public AddressDetails addressDetails { get; set; }
        }

        public class DestinationPartyContact
        {
            public string code { get; set; } = string.Empty;
            public string partyCode { get; set; } = string.Empty;
            public PersonDetails personDetails { get; set; }
        }

        public class LoadingUnit
        {
            public string uniqueId { get; set; } = string.Empty;
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
            public string photos { get; set; } = string.Empty;
            public string serialNumber { get; set; } = string.Empty;
            public string uniqueId { get; set; } = string.Empty;
            public string unitType { get; set; } = string.Empty;
            public int labelNr { get; set; }
            public string sealNumber { get; set; } = string.Empty;
            public string warehouseLocation { get; set; } = string.Empty;
            public string storageUnit { get; set; } = string.Empty;
            public DateTime dateIn { get; set; }
            public DateTime dateOut { get; set; }
        }

        public class OriginAddress
        {
            public string partyCode { get; set; } = string.Empty;
            public AddressDetails addressDetails { get; set; }
        }

        public class OriginPartyContact
        {
            public string code { get; set; } = string.Empty;
            public string partyCode { get; set; } = string.Empty;
            public PersonDetails personDetails { get; set; }
        }

        public class PersonDetails
        {
            public ContactDetails contactDetails { get; set; }
            public string firstName { get; set; } = string.Empty;
            public string lastName { get; set; } = string.Empty;
            public string preferredLanguage { get; set; } = string.Empty;
            public string salutation { get; set; } = string.Empty;
        }

        public class QuestionnaireQuestion
        {
            public string name { get; set; } = string.Empty;
            public int numericValue { get; set; }
            public string stringValue { get; set; } = string.Empty;
            public string listValues { get; set; } = string.Empty;
            public bool booleanValue { get; set; }
            //public DateTime dateValue { get; set; }
            //public string photoValue { get; set; }
            public string signatureValue { get; set; } = string.Empty;
        }
    }
}
