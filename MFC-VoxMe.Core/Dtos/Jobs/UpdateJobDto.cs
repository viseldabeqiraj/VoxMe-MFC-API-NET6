namespace MFC_VoxMe_API.Dtos.Jobs
{
    public class UpdateJobDto
    {

        public string jobStatus { get; set; }
        public OriginAddress originAddress { get; set; } = new OriginAddress();
        public OriginPartyContact originPartyContact { get; set; } = new OriginPartyContact();
        public DestinationAddress destinationAddress { get; set; } = new DestinationAddress();
        public DestinationPartyContact destinationPartyContact { get; set; } = new DestinationPartyContact();
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
            public string email { get; set; } = String.Empty;
            public string mobilePhone { get; set; } = "+123456789";
            public string homePhone { get; set; } = String.Empty;
            public string workPhone { get; set; } = String.Empty;
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

        public class PersonDetails
        {
            public ContactDetails contactDetails { get; set; } = new ContactDetails();
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string preferredLanguage { get; set; } = "en-us";
            public string salutation { get; set; }
        }

    }
}
