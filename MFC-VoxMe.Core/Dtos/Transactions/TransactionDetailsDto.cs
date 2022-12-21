namespace MFC_VoxMe_API.Dtos.Transactions
{
    public class TransactionDetailsDto
    {
        public string externalRef { get; set; }
        public int firstLabelNr { get; set; }
        public int grossVolume { get; set; }
        public int grossWeight { get; set; }
        public int handledPiecesNr { get; set; }
        public int lastLabelNr { get; set; }
        public int piecesNr { get; set; }
        public DateTime scheduledDate { get; set; }
        public string transactionType { get; set; }
        public double transitTime { get; set; }
        public string uniqueId { get; set; }
        public int volume { get; set; }
        public int weight { get; set; }
        public string transactionStatus { get; set; }
        public string onsiteStatus { get; set; }
        public DateTime crewArrivalOnSiteDate { get; set; }
        public DateTime crewDepartureFromSiteDate { get; set; }
        public string crewNotes { get; set; }
        public string labelColor { get; set; }
        public string firsTag { get; set; }
        public string clientSignature { get; set; }
        public string driverSignature { get; set; }
        public string destClientSignature { get; set; }
        public string destDriverSignature { get; set; }
        public List<string> services { get; set; }
        public List<QuestionnaireQuestion> questionnaireQuestions { get; set; }
        public List<AuxService>? auxServices { get; set; }
        public List<МaterialsUsed> materialsUsed { get; set; }
        public List<string> loadingUnitUniqueIds { get; set; }
        public List<TimeSheet> timeSheets { get; set; }
        public List<CrewMember> crewMembers { get; set; }
        public List<string> vehiclesCodes { get; set; }
        public List<string> equipmentCodes { get; set; }
        public List<Document> documents { get; set; }
        public class AuxService
        {
            public string name { get; set; }
            public int? numericValue { get; set; }
            public string? stringValue { get; set; }
            public string? listValues { get; set; }
            public bool? booleanValue { get; set; }
            public DateTime? dateValue { get; set; }
            public string? photoValue { get; set; }
            public string? signatureValue { get; set; }
        }

        public class CrewMember
        {
            public string code { get; set; }
            public bool isForeman { get; set; }
        }

        public class Document
        {
            public string docTitle { get; set; }
            public string fileName { get; set; }
        }

        public class QuestionnaireQuestion
        {
            public string name { get; set; }
            public int numericValue { get; set; }
            public string? stringValue { get; set; }
            public string? listValues { get; set; }
            public bool booleanValue { get; set; }
            public DateTime? dateValue { get; set; }
            public string? photoValue { get; set; }
            public string? signatureValue { get; set; }
        }

        public class TimeSheet
        {
            public int break1 { get; set; }
            public int break2 { get; set; }
            public int break3 { get; set; }
            public double breaksDuration { get; set; }
            public DateTime endDate { get; set; }
            public bool isForeman { get; set; }
            public string name { get; set; }
            public DateTime startDate { get; set; }
            public double totalDuration { get; set; }
            public int totalWorkDuration { get; set; }
        }

        public class МaterialsUsed
        {
            public string code { get; set; }
            public double materialUnitCost { get; set; }
            public int qty { get; set; }
        }



    }
}
