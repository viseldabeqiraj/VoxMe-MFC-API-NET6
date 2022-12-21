using Dapper;
using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers;
using MFC_VoxMe.Infrastructure.Models;
using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Transactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Xml.Serialization;
using static MFC_VoxMe.Infrastructure.Data.Helpers.Enums;
using static MFC_VoxMe_API.Dtos.Jobs.CreateJobDto;
using static MFC_VoxMe_API.Dtos.Transactions.AssignStaffDesignateForemanDto;

namespace MFC_VoxMe_API.BusinessLogic.JimToVoxMe
{
    public class JimToVoxmeHelper : IJimToVoxmeHelper
    {
        public static MovingDataDto _MovingData;
        private readonly IDynamicQueryGenerator _queryGenerator;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public JimToVoxmeHelper(IDynamicQueryGenerator queryGenerator, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _queryGenerator = queryGenerator;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<MovingDataDto> XMLParseAsync(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MovingDataDto));
            MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            MovingDataDto movingDataFromXml = (MovingDataDto)serializer.Deserialize(memStream);
            _MovingData = movingDataFromXml;
            return movingDataFromXml;

        }



        public CreateJobDto CreateJobObjectFromXml()
        {
            CreateJobDto createJobDto = new CreateJobDto();

            var generalInfo = _MovingData.GeneralInfo;
            createJobDto.externalRef = generalInfo.Groupageid;
            var properties = _MovingData.InventoryData.Properties.Property;

            createJobDto.serviceType = "Enum.ServiceType." + properties.FirstOrDefault
                (s => s.Type == "Form.General.Contract").Description.Replace(" ", "");
            createJobDto.jobType = "Enum.JobType." + properties.FirstOrDefault
                (s => s.Type == "Form.General.Authority").Description.Replace(" ", "");

            createJobDto.serviceLevel = "Enum.ServiceLevel." + generalInfo.Preferences.ServiceLevel;
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
                        mobilePhone = generalInfo.CoordinatorMobile
                    },

                }

            };
            createJobDto.bookerPerson.personDetails = createJobDto.managedBy.personDetails;
            createJobDto.bookerPerson.code = generalInfo.CoordinatorID;

            if (properties.Any(s => s.Type == "Form.General.Account" && s.Description == "U.S.Military"))
            {
                createJobDto.account = new Account();
                createJobDto.accountPerson = new AccountPerson();
                createJobDto.account.code = "U.S.MILITARY001";
                createJobDto.account.legalName = createJobDto.account.code;
                createJobDto.accountPerson.code = "U.S.MILITARY001";
                createJobDto.accountPerson.partyCode = "U.S.MILITARY001"; 
                createJobDto.accountPerson.personDetails = createJobDto.managedBy.personDetails;
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

            if (generalInfo.Address.Rooms.Room != null)
            {
                var rooms = generalInfo.Address.Rooms.Room;
                createJobDto.inventoryData = new CreateJobDto.InventoryData()
                {
                    rooms = rooms.Select(x => new CreateJobDto.Room()
                    {
                        roomType = "Enum.RoomResidential." + x.Name.Replace(" ", ""),
                        name = "Enum.RoomResidential." + x.Name.Replace(" ", ""),
                    }
                   ).ToList()
                };
            }

            if (_MovingData.InventoryData.Packers.Packer != null)
            {
                var packers = _MovingData.InventoryData.Packers.Packer;
                createJobDto.inventoryData.packers = packers.Select(x => new CreateJobDto.Packer()
                {
                    packer = x.Name,
                    isForeman = bool.Parse(x.IsForeman)
                }).ToList();
            }

            if (_MovingData.InventoryData.Skids.Skid != null)
            {
                var skids = _MovingData.InventoryData.Skids.Skid;

                createJobDto.inventoryData.loadingUnits = skids.Select(x => new LoadingUnit()
                {
                    uniqueId = x.Barcode,
                    unitType = "Enum.ShipmentUnitType." 
                    + x.Type.Substring(0, 1) + x.Type.Substring(1).ToLower(),
                    serialNumber = x.SerialNo,
                    labelNr = Convert.ToInt32(x.ID)
                }).ToList();
            }

            if (_MovingData.InventoryData.Piece != null)
            {

                var pieces = _MovingData.InventoryData.Piece;
                createJobDto.inventoryData.pieces = pieces.Select(x => new CreateJobDto.Piece()
                {
                    labelNr = x.Id.ToString(),
                    tag = x.Id.ToString(),
                    barcode = x.Barcode,
                    packerName = x.Packer,
                    roomName = "Enum.RoomResidential." + x.Location.Replace(" ", ""),
                    pbo = x.PBO,
                    packageType = x.Box.Name == "No Box" 
                    ? "" : "Enum.MaterialType." + x.Box.Name.Replace(" ", ""),
                    packageQty = x.Box.Quantity,
                    loadUnitUniqueId = _MovingData.InventoryData.Skids.Skid.FirstOrDefault().Barcode, //? check if we recieve list of skids or just one skid
                    @void = x.Void,
                    height = x.Size.Height,
                    length = x.Size.Length,
                    volume = x.Volume,
                    weight = x.Weight,
                    //packageUnitCost = x.Box., //??
                    items = new List<CreateJobDto.Item>()
                {
                    new CreateJobDto.Item()
                    {
                        itemNr = x.Id.ToString(),
                        itemName = "Enum.CompanyItem."+ x.Item.Name.Replace(" ", ""),
                        itemType =  string.IsNullOrEmpty(x.Item.Type) 
                        ? "Enum.ItemType.Generic" 
                        : "Enum.ItemType." + x.Item.Type.Replace(" ", ""),
                        itemCategory = "Enum.ItemCategory." + x.Item.Category.Replace(" ", ""),
                        volume = x.Volume,
                        value = x.Item.Value,
                        valuationCurrency = "USD",
                        qty=x.Item.Quantity,
                        condition = x.Item.Condition,
                        make = x.Item.Make,
                        model = x.Item.Model,
                        year = x.Item.Year,
                        serialNumber = x.Item.SerialNumber,
                        width = x.Item.Size.Width,
                        height = x.Item.Size.Height,
                        length = x.Item.Size.Length,
                        isPart = x.Item.IsPart,
                        dismantle = x.Item.Dismantling,
                        isCrated = x.Item.IsPart,
                        isValuable = x.Item.IsValuable,
                        pictureAuthor = x.Item.PictureAuthor,
                        pictureTitle = x.Item.PictureName,
                        pictureYear= x.Item.PictureYear,
                        materialsDesc = x.Item.MaterialsDesc,
                        countryOrigin = x.Item.CountryOrigin,
                        comment = x.Item.Comment,
                        //photos = x.Item.PictureFileName,
                    }
                }
                }).ToList();
            }

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
                        stringValue = a.Description ?? String.Empty,
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
                        stringValue = a.Description ?? String.Empty,
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
            AssignMaterialsToTransactionDto assignMaterialsToTransaction = new AssignMaterialsToTransactionDto();
            var materialsDescription = _MovingData.InventoryData.Properties.Property.ToList().Find(prop => prop.Type == "Form.General.Materials").Description;
            if (string.IsNullOrEmpty(materialsDescription))
                return assignMaterialsToTransaction;
            materialsDescription = materialsDescription.Replace('[',' ');
            materialsDescription = materialsDescription.Replace(']', ' ');
            var materieals = materialsDescription.Split(',');
            if(materieals != null)
            {
                List<AssignMaterialsToTransactionDto.HandedMaterial> materialList = materieals.Select
                    (item => 
                    new AssignMaterialsToTransactionDto.HandedMaterial()
                    {
                        code = "Enum.MaterialType." + item.Split(':')[0].Replace(" ", ""),
                        qty = Int32.Parse(item.Split(':')[1])
                    }
                    ).ToList();
                var materilasToSendToMfc = materialList.Where(item => doesItMatchWithMFC(item.code)).ToList();
                assignMaterialsToTransaction.handedMaterials = materilasToSendToMfc;
            }
            return assignMaterialsToTransaction;
        }

        private bool doesItMatchWithMFC(string code)
        {
            if(
                code.Contains("1.5Carton")
                || code.Contains("3.1Carton")
                || code.Contains("4.5Carton")
                || code.Contains("AcidFreeTissuePaper")
                || code.Contains("BlanketWrap")
                || code.Contains("BikeBox")
                || code.Contains("BubbleWrap")
                || code.Contains("CarpetShield")
                || code.Contains("ClockCarton")
                || code.Contains("CrateCount")
                || code.Contains("CribMattressCarton")
                || code.Contains("DAirFreightContainer")
                || code.Contains("DishCarton")
                || code.Contains("DolphinFoam")
                || code.Contains("DoubleMattressCarton")
                || code.Equals("Enum.MaterialType.FlatWardrobe")
                || code.Contains("GatorBox")
                || code.Contains("GreenTape")
                || code.Contains("HoistingStraps")
                || code.Contains("KingMattressCarton")
                || code.Contains("KraftBubble")
                || code.Contains("LampCarton")
                || code.Contains("LdnAirFreightContainer")
                || code.Contains("LiftVans")
                || code.Contains("MattressBags-Single")
                || code.Contains("MattressBags-Queen/Double")
                || code.Contains("MattressBags-King")
                || code.Contains("Memoryfoam")
                || code.Contains("Mirror4Piece")
                || code.Contains("OfficeTotes")
                || code.Contains("Paper")
                || code.Contains("PaperPad")
                || code.Contains("PartsBox")
                || code.Contains("Peanuts")
                || code.Contains("PillowtopCarton")
                || code.Contains("QueenMattressCarton")
                || code.Contains("ShortyLiftvan")
                || code.Contains("SingleMattressCarton")
                || code.Contains("StretchWrap")
                || code.Contains("Tape")
                || code.Contains("Triwall10")
                || code.Contains("Triwall15")
                || code.Contains("Triwall5")
                || code.Contains("TvBox")
                || code.Contains("UsedLiftvans")
                || code.Contains("WardrobeRent")
                || code.Contains("WardrobeSold")
                || code.Contains("WasherKitFL")
                || code.Contains("WasherKitTL")
                || code.Contains("WineBox")
                || code.Contains("Brownpaperpads")
                || code.Contains("6.5Carton")
                || code.Contains("6.0Carton")
                || code.Contains("Glassine")
                || code.Contains("SlatBox")
                || code.Contains("CribMattressBag")
                )
            {
                return true;
            }
            else
            {
                return false;
            }
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
            { Table = "MovingData", Dto = movingData });

            var resp = await _queryGenerator.SelectFrom(
              new SqlQuery<string>()
              {
                  Function = IEnums.functions.MAX,
                  Columns = "id",
                  Table = "MovingData",
                  As = "as ID"
              });
            var NewMovingDataId = resp[0].ID as int?;
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
                    Table = valuePair.Key,
                    Dto = valuePair.Value
                };
                await _queryGenerator.InsertInto(query);
            }

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

        public byte[] GetDoc()
        {
            return File.ReadAllBytes(Path.Combine
                     (_hostingEnvironment.ContentRootPath, @"BusinessLogic\JimToVoxMe\pdf-test.pdf"));
        }


    }
}
