namespace MFC_VoxMe_API.Dtos.Transactions
{
    public class TransactionDetailsDto
    {
        public string externalRef { get; set; }
        public string uniqueId { get; set; }
        public string transactionType { get; set; }
        public string transactionStatus { get; set; }
        public string onsiteStatus { get; set; }
        public DateTime scheduledDate { get; set; }
        public DateTime crewArrivalOnSiteDate { get; set; }
        public DateTime crewDepartureFromSiteDate { get; set; }
        public string crewNotes { get; set; }
        public int transitTime { get; set; }
        public int piecesNr { get; set; }
        public int handledPiecesNr { get; set; }
        public int firstLabelNr { get; set; }
        public int lastLabelNr { get; set; }
        public int volume { get; set; }
        public int weight { get; set; }
        public double grossVolume { get; set; }
        public double grossWeight { get; set; }
        public string labelColor { get; set; }
        public string firsTag { get; set; }
        public string clientSignature { get; set; }
        public string driverSignature { get; set; }
        public string destClientSignature { get; set; }
        public string destDriverSignature { get; set; }
        public List<string> services { get; set; }
        public List<QuestionnaireQuestion> questionnaireQuestions { get; set; }
        public List<AuxService> auxServices { get; set; }
        public List<object> мaterialsUsed { get; set; } //TODO
        public List<object> loadingUnitUniqueIds { get; set; }
        public List<object> timeSheets { get; set; }
        public List<object> crewMembers { get; set; }
        public List<object> vehiclesCodes { get; set; }
        public List<object> equipmentCodes { get; set; }
        public List<Document> documents { get; set; }
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

        public class Document
        {
            public string fileName { get; set; }
            public string docTitle { get; set; }
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
