using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers;
using MFC_VoxMe.Infrastructure.Models;
using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Transactions;
using MFC_VoxMe_API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static MFC_VoxMe.Infrastructure.Data.Helpers.Enums;

namespace MFC_VoxMe_API.BusinessLogic.VoxMeToJim
{
    public class VoxmeToJimHelper : IVoxmeToJimHelper
    {
        public static MovingDataDto _MovingData;
        private readonly IDynamicQueryGenerator _queryGenerator;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public VoxmeToJimHelper(IDynamicQueryGenerator queryGenerator, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _queryGenerator = queryGenerator;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task InsertTableRecords()
        {
            var generalInfo = _MovingData.GeneralInfo;

            var movingData = new MovingData()
            {
                ClientFirstName = generalInfo.ClientFirstName,
                ClientName = generalInfo.Name,
                Date = DateTime.Parse
                            (generalInfo.Preferences.PackingDate),
                JobDescription = "Imperial",
                EstimatorName = generalInfo.EstimatorName,
                Comment = generalInfo.Comment,
                FileName = "",
                BillOfLadingNo = generalInfo.Groupageid,
                ExternalMFID = generalInfo.EMFID,
                State = 3,
                OrgID = "",
                LastAccessTime = DateTime.Now,
                CoordinatorName = "Voxme",
                CoordinatorEmail = "jkmoving@voxme.com",
                BookingAgent = "JK",
                BookingAgentContact = "Voxme",
                BookingAgentContactEmail = "jkmoving@voxme.com",
                OriginAgent = "JK",
                ShipmentType = generalInfo.ShipmentType,
                ClientSalutation = generalInfo.ClientSalutation,
                TripNumber = 0,
                Manager = "Voxme",
                Hold = false
            };

            await _queryGenerator.InsertInto(new SqlQuery<MovingData>()
            { table = "MovingData", dto = movingData });

            var resp = await _queryGenerator.SelectFrom(
              new SqlQuery<string>()
              {
                  function = IEnums.functions.MAX,
                  columns = "id",
                  table = "MovingData",
                  As = "as ID"
              });
            var NewMovingDataId = resp.ID as int?;
            CreateClientFoderDir(movingData.ClientFirstName, movingData.ClientName, (int)NewMovingDataId);


            var prefs = new Prefs()
            {
                MovingDataID = (int)NewMovingDataId,
                PrefferedLanguageID = 1,
                PackingDate = DateTime.Parse
                            (generalInfo.Preferences.PackingDate),
                ServiceTypeID = 1,
                Comment = generalInfo.Preferences.Comment,
                ItemsPath = generalInfo.ClientFirstName + "_" + generalInfo.Name + "_" + NewMovingDataId,
                //RealArrivalDate = "",
                DeliveryDate = DateTime.Parse
                                (generalInfo.Preferences.DeliveryDate),
                //SurveyDate = "",
                CreationDate = DateTime.Now,
                PackingFinishDate = DateTime.Parse
                                (generalInfo.Preferences.PackingFinishDate)
            };

            var originAddress = new MFC_VoxMe.Infrastructure.Models.Address()
            {
                MovingDataID = (int)NewMovingDataId,
                IsDestination = false,
                Street = generalInfo.Address.Street,
                City = generalInfo.Address.City,
                Country = generalInfo.Address.Country,
                Floor = generalInfo.Address.AccessInfo.Floor,
                Elevator = Convert.ToBoolean
                            (generalInfo.Address.AccessInfo.HasElevator),
                DistanceToParking = 0,
                NeedCrane = false,
                PrimaryPhone = generalInfo.Address.PrimaryPhone,
                SecondaryPhone = generalInfo.Address.SecondaryPhone,
                Email = generalInfo.Address.Email,
                Comment = generalInfo.Address.Comment,
                Zip = generalInfo.Address.Zip,
                State = generalInfo.Address.State,
                PropertyType = generalInfo.Address.AccessInfo.PropertyType,
                ParkingReservationRequired = false,
                NumOfParkingSpots = 0,
                ParkingSpotSize = 0,
                CarryRequired = Convert.ToBoolean
                                (generalInfo.Address.AccessInfo.CarryRequired),
                CarryLength = 0,
                ShuttleRequired = Convert.ToBoolean
                                (generalInfo.Address.AccessInfo.ShuttleRequired),
                ShuttleDistance = 0,
                StairCarryRequired = Convert.ToBoolean
                                (generalInfo.Address.AccessInfo.StairCarryRequired),
                StairCarryLength = 0,
                AdditionalStopRequired = Convert.ToBoolean
                                (generalInfo.Address.AccessInfo.AdditionalStopRequired)

            };

            var destinationAddress = new MFC_VoxMe.Infrastructure.Models.Address()
            {
                MovingDataID = (int)NewMovingDataId,
                IsDestination = true,
                Street = generalInfo.Destination.Street,
                City = generalInfo.Destination.City,
                Country = generalInfo.Destination.Country,
                Floor = generalInfo.Destination.AccessInfo.Floor,
                Elevator = Convert.ToBoolean
                            (generalInfo.Destination.AccessInfo.HasElevator),
                DistanceToParking = 0,
                NeedCrane = false,
                PrimaryPhone = generalInfo.Destination.PrimaryPhone,
                SecondaryPhone = generalInfo.Destination.SecondaryPhone,
                Email = generalInfo.Destination.Email,
                Comment = generalInfo.Destination.Comment,
                Zip = generalInfo.Destination.Zip,
                State = generalInfo.Destination.State,
                PropertyType = generalInfo.Destination.AccessInfo.PropertyType,
                ParkingReservationRequired = false,
                NumOfParkingSpots = 0,
                ParkingSpotSize = 0,
                CarryRequired = Convert.ToBoolean
                                (generalInfo.Destination.AccessInfo.CarryRequired),
                CarryLength = 0,
                ShuttleRequired = Convert.ToBoolean
                                (generalInfo.Destination.AccessInfo.ShuttleRequired),
                ShuttleDistance = 0,
                StairCarryRequired = Convert.ToBoolean
                                (generalInfo.Destination.AccessInfo.StairCarryRequired),
                StairCarryLength = 0,
                AdditionalStopRequired = Convert.ToBoolean
                                (generalInfo.Destination.AccessInfo.AdditionalStopRequired)
            };

            List<KeyValuePair<string, object>> objectsToInsert =
                         new List<KeyValuePair<string, object>>();

            objectsToInsert.Add
                (new KeyValuePair<string, object>("Prefs", prefs));
            objectsToInsert.Add
                (new KeyValuePair<string, object>("Address", originAddress));
            objectsToInsert.Add
                (new KeyValuePair<string, object>("Address", destinationAddress));

            foreach (KeyValuePair<string, object> valuePair in objectsToInsert)
            {
                var query = new SqlQuery<object>()
                {
                    table = valuePair.Key,
                    dto = valuePair.Value
                };
                await _queryGenerator.InsertInto(query);
            }

        }

        public async Task InsertDataFromJobDetails(JobDetailsDto jobDetails, int movingDataId)
        {
            var roomDetails = jobDetails.jobInventory.rooms;
            var packersDetails = jobDetails.jobInventory.packers;
            foreach (var room in roomDetails)
            {
                var rooms = new Rooms()
                {
                    MovingDataID = movingDataId,
                    Name = room.name,
                    Description = room.notes,
                    NickName = room.name,
                    MoveInCondition = room.conditionBeforeService.description,
                    MoveInPictures = room.conditionBeforeService.photos,
                    MoveOutCondition = room.conditionAfterService.description,
                    MoveOutPictures = room.conditionAfterService.photos,
                    ID = 2

                };
            }
        }
        public async Task UpdateMovingData(string externalRef)
        {
            var test = new MovingData()
            {
                ClientName = "viseldaa test update 2",
            };
            var update = new SqlQuery<MovingData>();

            update.dto = test;
            update.whereClause = update.Where("ExternalMFID", externalRef);
            update.comparisonOperator = Constants.ComparisonOperators.EQUALTO;

            await _queryGenerator.UpdateTable(update);
        }

        public async Task<dynamic> GetMovingDataId(string externalRef)
        {
            SqlQuery<string> select = new SqlQuery<string>()
            {
                columns = "*",
                table = Constants.Tables.MOVINGDATA,
                whereClause = new Dictionary<string, object>()
                    {
                        {"ExternalMFID", externalRef},
                        {"JobDescription", "Imperial"}
                    }, // select.Where("ExternalMFID", @$"'{externalRef}'");
                comparisonOperator = Constants.ComparisonOperators.EQUALTO,
                logOperator = IEnums.logOperator.AND
            };

            return await _queryGenerator.SelectFrom(select);
        }

        public async Task<string> GetItemsPath(int movingDataId)
        {
            SqlQuery<string> select = new SqlQuery<string>();
            select.columns = "ItemsPath";
            select.table = Constants.Tables.PREFS;
            select.whereClause = select.Where("MovingDataID", @$"'{movingDataId}'");
            select.comparisonOperator = Constants.ComparisonOperators.EQUALTO;

            var result = await _queryGenerator.SelectFrom(select);
            return _configuration.GetValue<string>("Client_Folder_Dir:stagingDir") + result.ItemsPath;
        }

        public List<KeyValuePair<string, string>> GetImages(HttpResponseDto<TransactionDetailsDto> transactiondetails)
        {
            var questionnaireQuestions = transactiondetails.dto?.questionnaireQuestions.ToList();
            var auxServices = transactiondetails.dto?.auxServices.ToList();

            List<KeyValuePair<string, string>> imagesToStore =
                        new List<KeyValuePair<string, string>>();
            var lists = questionnaireQuestions?.Zip(auxServices, (q, a) =>
                              new { questionnaireQuestions = q, auxServices = a });
            if (lists is not null)
            {
                foreach (var item in lists)
                {
                    imagesToStore.Add
                            (new KeyValuePair<string, string>
                            ("QuestionnaireQuestions", item.questionnaireQuestions.signatureValue));
                    imagesToStore.Add
                            (new KeyValuePair<string, string>
                            ("QuestionnaireQuestions", item?.questionnaireQuestions.photoValue));
                    imagesToStore.Add
                            (new KeyValuePair<string, string>
                            ("AuxServices", item?.auxServices.signatureValue));
                    imagesToStore.Add
                            (new KeyValuePair<string, string>
                            ("AuxServices", item?.auxServices.photoValue));
                }
            }
            imagesToStore.Add
               (new KeyValuePair<string, string>("Transaction", transactiondetails.dto.clientSignature));
            imagesToStore.Add
               (new KeyValuePair<string, string>("Transaction", transactiondetails.dto.driverSignature));
            imagesToStore.Add
               (new KeyValuePair<string, string>("Transaction", transactiondetails.dto.destDriverSignature));
            imagesToStore.Add
               (new KeyValuePair<string, string>("Transaction", transactiondetails.dto.destClientSignature));
            return imagesToStore;
        }

        private void CreateClientFoderDir(string firstname, string lastname, int moveDataId)
        {
            var clientFullName = $@"{firstname}_{lastname}";

            var clientDirStaging = _configuration.GetValue<String>("Client_Folder_Dir:stagingDir") + $@"\\{clientFullName}_{moveDataId}";
            var clientDirProduction = _configuration.GetValue<String>("Client_Folder_Dir:productionDir") + $@"\\{clientFullName}_{moveDataId}";

            if (!Directory.Exists(clientDirStaging))
            {
                Directory.CreateDirectory(clientDirStaging);
                Directory.CreateDirectory(clientDirStaging + "\\Documents");
                Directory.CreateDirectory(clientDirStaging + "\\DocumentsForSuppliers");
                Directory.CreateDirectory(clientDirStaging + "\\Files");
                Directory.CreateDirectory(clientDirStaging + "\\Pictures");
            }
        }

        public string GetJsonConfigFile(string key)
        {
            string json = File.ReadAllText(Path.Combine
                         (_hostingEnvironment.ContentRootPath, @"NamingConfigurationFile.json"));

            //var data = (JObject)JsonConvert.DeserializeObject(json);
            //string x = data["Enum.RoomResidential.Foyer"].Value<string>();
            JObject obj = JObject.Parse(json);
            string x = (string)obj["RoomsConfig"]["Enum.RoomResidential.Foyer"];
            return x;

        }
    }
}
