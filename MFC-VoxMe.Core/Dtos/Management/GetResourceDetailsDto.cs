namespace MFC_VoxMe_API.Dtos.Management
{
    public class GetResourceDetailsDto
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public Resource resource { get; set; }

        public class Resource
        {
            public string code { get; set; }
            public string homeDivisionId { get; set; }
            public string resourceName { get; set; }
            public string resourceType { get; set; }
        }

    }
}
