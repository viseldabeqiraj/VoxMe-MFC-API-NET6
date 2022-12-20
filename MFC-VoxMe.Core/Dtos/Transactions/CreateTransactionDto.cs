namespace MFC_VoxMe_API.Dtos.Transactions
{
    public class CreateTransactionDto
    {
        public string externalRef { get; set; } = String.Empty;
        public string jobExternalRef { get; set; } =  String.Empty;
        public DateTime scheduledDate { get; set; } 
        public string transactionType { get; set; } = String.Empty;
        public string originParty { get; set; } = String.Empty;
        public OriginAddress originAddress { get; set; }
        public OriginPartyContact originPartyContact { get; set; }
        public string destinationParty { get; set; } = String.Empty;
        public DestinationAddress destinationAddress { get; set; }
        public DestinationPartyContact destinationPartyContact { get; set; }
        public string transactionStatus { get; set; } = "Enum.TransactionStatus.Scheduled";
        public string onsiteStatus { get; set; } = "Enum.TransactionOnSiteStatus.ActivityScheduled";
        public string handlingDivision { get; set; } = "JKMOVINGTEST002";
        public string managedBy { get; set; } = String.Empty;
        public string instructionsCrewOrigin { get; set; } = String.Empty;
        public string instructionsCrewDestination { get; set; } = String.Empty;
        public List<string> services { get; set; }
        public List<QuestionnaireQuestion> questionnaireQuestions { get; set; }
        public List<AuxService> auxServices { get; set; }
        public List<LoadingUnit> loadingUnits { get; set; }
        public string transactionCreationWebhookUrl { get; set; } = String.Empty;
        public string transactionStatusUpdateWebhookUrl { get; set; } = "https://webhook.site/8ed65dc6-fe6a-415c-996e-8653ee6d9fad";
        public class AddressDetails
        {
            public string city { get; set; } = String.Empty;
            public string country { get; set; } = String.Empty;
            public string street1 { get; set; } = String.Empty;
            public string street2 { get; set; } = String.Empty;
            public string street3 { get; set; } = String.Empty;
            public string area { get; set; } = String.Empty;
            public string zip { get; set; } = String.Empty;
            public string floor { get; set; } = String.Empty;
            public string notes { get; set; } = String.Empty;
        }

        public class AuxService
        {
            public string name { get; set; } = String.Empty;
            public double numericValue { get; set; } 
            public string stringValue { get; set; } = String.Empty;
            public string listValues { get; set; } = String.Empty;
            public bool booleanValue { get; set; }
            //public DateTime dateValue { get; set; }
            //public string photoValue { get; set; } = String.Empty;
            public string signatureValue { get; set; } = String.Empty;
        }

        public class ContactDetails
        {
            public string value { get; set; } = String.Empty;
        }

        public class DestinationAddress
        {
            public string partyCode { get; set; } = String.Empty;
            public AddressDetails addressDetails { get; set; }
        }

        public class DestinationPartyContact
        {
            public string code { get; set; } = String.Empty;
            public string partyCode { get; set; } = String.Empty;
            public PersonDetails personDetails { get; set; }
        }

        public class LoadingUnit
        {
            public string uniqueId { get; set; } = String.Empty;
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
           // public string photos { get; set; } 
            public string serialNumber { get; set; } = "UNPACKED";
            public string uniqueId { get; set; }
            public string unitType { get; set; } = "Enum.ShipmentUnitType.Delivery";
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
            public string name { get; set; } = string.Empty;    
            public double numericValue { get; set; } 
            public string stringValue { get; set; } = string.Empty;
            public string listValues { get; set; } = string.Empty;
            public bool booleanValue { get; set; }
            //public DateTime dateValue { get; set; }
            //public string photoValue { get; set; } = string.Empty;
            public string signatureValue { get; set; } = string.Empty;
        }

    }
}
