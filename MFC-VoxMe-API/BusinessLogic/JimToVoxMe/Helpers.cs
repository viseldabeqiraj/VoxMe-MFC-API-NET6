using Dapper;
using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers;
using MFC_VoxMe.Infrastructure.Models;
using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Transactions;
using MFC_VoxMe_API.Models;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Serialization;
using static MFC_VoxMe_API.Dtos.Jobs.CreateJobDto;
using static MFC_VoxMe_API.Dtos.Transactions.AssignStaffDesignateForemanDto;

namespace MFC_VoxMe_API.BusinessLogic.JimToVoxMe
{
    public class Helpers : IHelpers
    {
        public static MovingDataDto _MovingData;
        private readonly IDynamicQueryGenerator _queryGenerator;
        private readonly IConfiguration _configuration;

        public Helpers(IDynamicQueryGenerator queryGenerator, IConfiguration configuration)
        {
            _queryGenerator = queryGenerator;
            _configuration = configuration;
        }

        public async Task<MovingDataDto> XMLParseAsync(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MovingDataDto));
            MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            MovingDataDto movingDataFromXml = (MovingDataDto)serializer.Deserialize(memStream);
            _MovingData = movingDataFromXml;
            //var test = new MovingData()
            //{
            //    ClientName = "viselda test update",
                
            //};
            //var update = new SqlQuery<MovingData>();

            //update.dto = test;
            //update.whereClause = update.Where("ExternalMFID", "RS0210275");
            //update.comparisonOperator = Constants.ComparisonOperators.EQUALTO;
           
            //await _queryGenerator.UpdateTable(update);
            return movingDataFromXml;

        }


        public CreateJobDto CreateJobObjectFromXml()
        {
            CreateJobDto createJobDto = new CreateJobDto();

            var generalInfo = _MovingData.GeneralInfo;
            createJobDto.externalRef = generalInfo.Groupageid;
            var properties = _MovingData.InventoryData.Properties.Property;

            createJobDto.serviceType = "Enum.ServiceType." + properties.FirstOrDefault
                (s => s.Type == "Form.General.Contract")?.Description.Replace(" ", "");
            createJobDto.jobType = "Enum.JobType." + properties.FirstOrDefault
                (s => s.Type == "Form.General.Authority")?.Description.Replace(" ", "");

            createJobDto.serviceLevel = "Enum.ServiceLevel." + generalInfo.Preferences.ServiceLevel.Replace(" ","");
            createJobDto.client.legalName = generalInfo.ClientFirstName + " " + generalInfo.Name;
            createJobDto.client.code = generalInfo.ClientNumber;
            createJobDto.instructionsCrewOrigin = generalInfo.Preferences.Comment + "\n" + generalInfo.Comment;
            createJobDto.instructionsCrewDestination = generalInfo.Preferences.Comment + "\n" + generalInfo.Comment;


            createJobDto.clientPerson = new ClientPerson()
            {
                code = generalInfo.ClientNumber,
                partyCode = generalInfo.ClientNumber,
                personDetails = new PersonDetails()
                {
                    firstName = generalInfo.ClientFirstName,
                    lastName = generalInfo.Name,
                    salutation = !string.IsNullOrEmpty(generalInfo.ClientSalutation)
                    ? "Enum.Salutation." + generalInfo.ClientSalutation : "",
                    contactDetails = new ContactDetails()
                    {
                        mobilePhone = generalInfo.Address.PrimaryPhone ??
                        generalInfo.Address.SecondaryPhone ??
                        generalInfo.Destination.PrimaryPhone ??
                        generalInfo.Destination.SecondaryPhone,
                        email = generalInfo.Address.Email
                    }
                },

            };

            createJobDto.managedBy = new ManagedBy()
            {
                code = generalInfo.CoordinatorID,
                personDetails = new PersonDetails()
                {
                    firstName = generalInfo.EstimatorName.Substring(generalInfo.EstimatorName.LastIndexOf(',') + 1),
                    lastName = generalInfo.EstimatorName.Split(',')[0],
                    contactDetails = new ContactDetails()
                    {
                        email = generalInfo.Coordinatoremail,
                        mobilePhone = generalInfo.CoordinatorMobile ?? "+123456789"
                    },

                }

            };
            createJobDto.bookerPerson.personDetails = createJobDto.managedBy.personDetails;
            createJobDto.bookerPerson.code = generalInfo.CoordinatorID;

            if (properties.Any(s => s.Type == "Form.General.Account" && s.Description != null))
            {
                createJobDto.account = new Account();
                createJobDto.accountPerson = new AccountPerson();
                createJobDto.account.code = "Enum.PartyType." + properties.FirstOrDefault
                    (s => s.Type == "Form.General.Account").Description.Replace(" ", "");
                createJobDto.account.legalName = createJobDto.account.code;
                createJobDto.accountPerson.personDetails = createJobDto.managedBy.personDetails;
                createJobDto.accountPerson.code = generalInfo.CoordinatorID;

                createJobDto.accountPerson.partyCode = properties.FirstOrDefault

                    (s => s.Type == "Form.General.Account").Description.Replace(" ", "");
            }

            createJobDto.originAddress = new OriginAddress()
            {
                partyCode = generalInfo.ClientNumber,
                addressDetails = new AddressDetails()
                {
                    street1 = generalInfo.Address.Street,
                    city = generalInfo.Address.City,
                    area = generalInfo.Address.State,
                    country = generalInfo.Address.Country,
                    floor = generalInfo.Address.AccessInfo.Floor,
                    notes = generalInfo.Address.Comment,
                    zip = generalInfo.Address.Zip,
                }
            };

            createJobDto.destinationAddress = new DestinationAddress()
            {
                partyCode = generalInfo.ClientNumber,
                addressDetails = new AddressDetails()
                {
                    street1 = generalInfo.Destination.Street,
                    city = generalInfo.Destination.City,
                    area = generalInfo.Destination.State,
                    country = generalInfo.Destination.Country,
                    floor = generalInfo.Destination.AccessInfo.Floor,
                    notes = generalInfo.Comment,
                    zip = generalInfo.Destination.Zip,
                }
            };

            createJobDto.originPartyContact = new OriginPartyContact()
            {
                code = generalInfo.ClientNumber,
                partyCode = generalInfo.ClientNumber,
                personDetails = createJobDto.clientPerson.personDetails
            };

            createJobDto.destinationPartyContact = new DestinationPartyContact()
            {

                code = generalInfo.ClientNumber,
                partyCode = generalInfo.ClientNumber,
                personDetails = createJobDto.clientPerson.personDetails
            };

            return createJobDto;

        }

        public string GetTransactionEnum(string str)
        {
            var transType = "Enum.TransactionType.";
            switch (str)
            {
                case "APU Floor":
                case "APU Trailer":
                case "Crate & Freight":
                case "Export":
                case "Final Pickup":
                case "Load":
                case "Load & Deliver":
                case "Pack":
                case "Pickup":
                case "SIT at Origin":
                case "Storage In":
                    return transType + "Pickup";
                case "Claims":
                case "Debris Pickup":
                case "Deliver":
                case "Import":
                case "SIT at Destination":
                case "Storage Out":
                case "Unpack":
                    return transType + "Delivery";
                case "Receive":
                case "Release Floor":
                case "SIT Inbound":
                    return transType + "WarehouseReceiveIn";
                case "Release":
                case "Release Overflow":
                case "Release Trailer":
                    return transType + "WarehouseOutload";
                case "Storage Access":
                    return transType + "OnSite";
                default:
                    return transType + str.Replace(" ", "");
            }
        }

        public CreateTransactionDto CreateTransactionObjectFromXml()
        {

            CreateTransactionDto createTransaction = new CreateTransactionDto();
            var generalInfo = _MovingData.GeneralInfo;

            createTransaction.externalRef = generalInfo.EMFID;
            createTransaction.transactionType = GetTransactionEnum(generalInfo.ShipmentType);
            createTransaction.services = new List<string>(1)
                { $"Enum.TransactionService.{generalInfo.ShipmentType.Replace(" ", "")}" };
            createTransaction.originParty = generalInfo.ClientNumber;
            createTransaction.destinationParty = generalInfo.ClientNumber;
            createTransaction.jobExternalRef = generalInfo.Groupageid;
            createTransaction.instructionsCrewOrigin = generalInfo.Preferences.Comment + "\n" + generalInfo.Comment;
            createTransaction.instructionsCrewDestination = generalInfo.Preferences.Comment + "\n" + generalInfo.Comment;
            createTransaction.scheduledDate = Convert.ToDateTime(generalInfo.Preferences.PackingDate);

            var properties = _MovingData.InventoryData.Properties.Property;

            if (properties.Any())
            {
                List<CreateTransactionDto.QuestionnaireQuestion> questionnaireQuestions = properties.Select
                    (a => new CreateTransactionDto.QuestionnaireQuestion()
                    {
                        name = a.Type,
                        numericValue = a.QtyTaken,
                        stringValue = "TEST",//a.Description ?? String.Empty,
                        listValues = a.Value,
                        booleanValue = a.QtyTaken > 0 ? true : false

                    }).ToList();
                createTransaction.questionnaireQuestions = questionnaireQuestions;
            }
            var services = _MovingData.InventoryData.Services.Service;
            if (services.Any())
            {
                List<CreateTransactionDto.AuxService> auxServices = services.Select
                    (a => new CreateTransactionDto.AuxService()
                    {
                        name = a.Type,
                        numericValue = a.QtyTaken,
                        stringValue = "TEST",//a.Description ?? String.Empty,
                        listValues = a.Value,
                        booleanValue = a.QtyTaken > 0 ? true : false

                    }).ToList();
                createTransaction.auxServices = auxServices;
            }

            if (generalInfo.ShipmentType == "Delivery")
            {

                List<CreateTransactionDto.LoadingUnit> loadingUnits = new List<CreateTransactionDto.LoadingUnit>(1);
                CreateTransactionDto.LoadingUnit loadingUnit = new CreateTransactionDto.LoadingUnit()
                {
                    uniqueId = generalInfo.EMFID + ".Delivery",
                    loadingUnitDetails = new CreateTransactionDto.LoadingUnitDetails()
                    {
                        labelNr = 1 //TODO
                    }
                };
                loadingUnits.Add(loadingUnit);
                createTransaction.loadingUnits = loadingUnits;
            }
            createTransaction.originAddress = new CreateTransactionDto.OriginAddress()
            {
                partyCode = generalInfo.ClientNumber,
                addressDetails = new CreateTransactionDto.AddressDetails()
                {
                    street1 = generalInfo.Address.Street,
                    city = generalInfo.Address.City,
                    area = generalInfo.Address.State,
                    country = generalInfo.Address.Country,
                    floor = generalInfo.Address.AccessInfo.Floor,
                    notes = generalInfo.Address.Comment,
                    zip = generalInfo.Address.Zip,
                }
            };

            createTransaction.destinationAddress = new CreateTransactionDto.DestinationAddress()
            {
                partyCode = generalInfo.ClientNumber,
                addressDetails = new CreateTransactionDto.AddressDetails()
                {
                    street1 = generalInfo.Destination.Street,
                    city = generalInfo.Destination.City,
                    area = generalInfo.Destination.State,
                    country = generalInfo.Destination.Country,
                    floor = generalInfo.Destination.AccessInfo.Floor,
                    notes = generalInfo.Comment,
                    zip = generalInfo.Destination.Zip,
                }
            };
            createTransaction.originPartyContact = new CreateTransactionDto.OriginPartyContact()
            {
                code = generalInfo.ClientNumber,
                partyCode = generalInfo.ClientNumber,
                //personDetails = createTransaction.clientPerson.personDetails
            };

            createTransaction.destinationPartyContact = new CreateTransactionDto.DestinationPartyContact()
            {

                code = generalInfo.ClientNumber,
                partyCode = generalInfo.ClientNumber,
                //personDetails = createTransaction.clientPerson.personDetails
            };

            return createTransaction;
        }


        public AssignMaterialsToTransactionDto GetTransactionMaterials()
        {
            var materials = _MovingData.InventoryData.Materials.Material.ToList();
            AssignMaterialsToTransactionDto assignMaterialsToTransaction =
                new AssignMaterialsToTransactionDto();

            if (materials != null)
            {
                List<AssignMaterialsToTransactionDto.HandedMaterial> materialList = materials.Select
                    (a => new AssignMaterialsToTransactionDto.HandedMaterial()
                    {
                        code = "Enum.MaterialType." + a.Type.Replace(" ", ""),
                        qty = Convert.ToDouble(a.QtyTaken)
                    }).ToList();
                assignMaterialsToTransaction.handedMaterials = materialList;
            }
            return assignMaterialsToTransaction;
        }

        public AssignStaffDesignateForemanDto GetTransactionResources()
        {
            var packers = _MovingData.InventoryData.Packers.Packer.ToList();
            AssignStaffDesignateForemanDto resourceCodesDto =
                new AssignStaffDesignateForemanDto();

            if (packers.Any())
            {
                List<StaffResourceCode> packerList = packers.Select
                    (a => new StaffResourceCode()
                    {
                        code = a.Name,
                        isForeman = bool.Parse(a.IsForeman)
                    }).ToList();
                resourceCodesDto.staffResourceCodes = packerList;
            }
            return resourceCodesDto;
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
                        { table = "MovingData", dto = movingData});

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
                Email=generalInfo.Address.Email,
                Comment= generalInfo.Address.Comment,   
                Zip=generalInfo.Address.Zip,
                State=generalInfo.Address.State,
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

        public async Task UpdateMovingData()
        {
            var test = new MovingData()
            {
                ClientName = "viselda test update",
            };
            var update = new SqlQuery<MovingData>();

            update.dto = test;
            update.whereClause = update.Where("ExternalMFID", "RS0210275");
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
                    {"ExternalMFID", @$"'{externalRef}'"}
                },
                comparisonOperator = Constants.ComparisonOperators.EQUALTO
            };           
            return await _queryGenerator.SelectFrom(select);
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

        //_config.GetSection("API_Url:AccessToken").Get<AccessTokenConfigDto>(); ;
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

    }
}
