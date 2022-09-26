namespace MFC_VoxMe_API.Dtos.Transactions
{
    public class GetTransactionDownloadDetails
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
       
            public DateTime downloadDate { get; set; }
            public string externalRef { get; set; }
            public string objectType { get; set; }
            public string applicationType { get; set; }
            public string deviceId { get; set; }

    }
}
