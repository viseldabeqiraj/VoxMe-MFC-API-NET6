namespace MFC_VoxMe_API.Dtos.Jobs
{
    public class JobUpdateDto
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public string jobStatus { get; set; }
        public OriginAddress originAddress { get; set; }
        public OriginPartyContact originPartyContact { get; set; }
        public DestinationAddress destinationAddress { get; set; }
        public DestinationPartyContact destinationPartyContact { get; set; }
        public string handlingDivision { get; set; }
        public string managedBy { get; set; }
        public string instructionsCrewOrigin { get; set; }
        public string instructionsCrewDestination { get; set; }
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

    }
}
