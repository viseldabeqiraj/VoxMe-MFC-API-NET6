using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers;
using MFC_VoxMe.Infrastructure.Models;
using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Transactions;
using Newtonsoft.Json.Linq;
using static MFC_VoxMe.Infrastructure.Data.Helpers.Enums;
using static MFC_VoxMe_API.Dtos.Jobs.JobDetailsDto;

namespace MFC_VoxMe_API.BusinessLogic.VoxMeToJim
{
    public class VoxmeToJimHelper : IVoxmeToJimHelper
    {
        public static MovingDataDto _MovingData;
        private readonly IDynamicQueryGenerator _queryGenerator;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private List<LoadingUnit>? _loadingUnits;


        public VoxmeToJimHelper(IDynamicQueryGenerator queryGenerator, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _queryGenerator = queryGenerator;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public string GetValueFromJsonConfig(string key)
        {
            string json = File.ReadAllText(Path.Combine
                         (_hostingEnvironment.ContentRootPath, @"NamingConfigurationFile.json"));

            JObject obj = JObject.Parse(json);
            string value = (string)obj[key];
            return value;
        }

        public async Task<int> testc()
        { 
            var query = await _queryGenerator.SelectFrom(
                    new SqlQuery<string>()
                    {
                        Columns = "Id",
                        Table = Constants.Tables.PACKERS,
                        WhereClause = SqlQuery<string>.Where
                                    ("Name", IEnums.logOperator.LIKE.ToString(), "'BRITT-JUSTIN'") 
                                    +IEnums.logOperator.AND
                                    + SqlQuery<string>.Where("MovingDataID", Constants.ComparisonOperators.EQUALTO, 123),
                    });

            int packerId = query[0].Id;

            return packerId;

        }

        public async Task InsertDataFromJobDetails(JobDetailsDto jobDetails, int movingDataId)
        {
            if (jobDetails.jobInventory.rooms != null)
            {
                var roomDetails = jobDetails.jobInventory.rooms;
                var maxRoomIdQuery = await _queryGenerator.SelectFrom(
                       new SqlQuery<string>()
                       {
                           Columns = "ID",
                           Table = Constants.Tables.ROOMS,
                           Function = IEnums.functions.MAX,
                           As = "ID"
                       });

                short maxRoomId = maxRoomIdQuery[0].ID;

                foreach (var room in roomDetails)
                {
                    maxRoomId++;
                    var newRoom = new MFC_VoxMe.Infrastructure.Models.Rooms()
                    {
                        MovingDataID = movingDataId,
                        Name = GetValueFromJsonConfig(room.name),
                        Description = room.notes,
                        NickName = GetValueFromJsonConfig(room.name),
                        MoveInCondition = room.conditionBeforeService == null
                        ? "" : room.conditionBeforeService.description,
                        MoveInPictures = room.conditionBeforeService == null
                        ? "" : room.conditionBeforeService.photos,
                        MoveOutCondition = room.conditionBeforeService == null
                        ? "" : room.conditionAfterService.description,
                        MoveOutPictures = room.conditionBeforeService == null
                        ? "" : room.conditionAfterService.photos,
                        ID = maxRoomId
                    };

                    await _queryGenerator.InsertInto(
                        new SqlQuery<MFC_VoxMe.Infrastructure.Models.Rooms>()
                        {
                            Table = Constants.Tables.ROOMS,
                            Dto = newRoom
                        });

                    //foreach (var roomElement in room.roomElements)
                    //{
                    //    var roomProperties = new RoomProperties()
                    //    {
                    //        Description = roomElement.description,
                    //        MoveInCondition = roomElement.conditionBeforeService.description,
                    //        MoveInPictures = roomElement.conditionBeforeService.photos,
                    //        MoveOutCondition = roomElement.conditionAfterService.description,
                    //        MoveOutPictures = roomElement.conditionAfterService.photos,
                    //        MovingDataId = movingDataId,
                    //        RoomID = newRoom.ID 
                    //    };
                    //}
                }
            }
            if (jobDetails.jobInventory.packers != null)
            {
                var query = await _queryGenerator.SelectFrom(
                      new SqlQuery<string>()
                      {
                          Columns = "ID",
                          Table = Constants.Tables.PACKERS,
                          Function = IEnums.functions.MAX,
                          As = "ID"
                      });

                int maxPackerId = query[0].ID;

                var packersDetails = jobDetails.jobInventory.packers;

                foreach (var packer in packersDetails)
                {
                    maxPackerId++;
                    var newPacker = new MFC_VoxMe.Infrastructure.Models.Packers
                    {
                        ID = maxPackerId, //?,
                        IsForeman = Convert.ToBoolean(packer.isForeman),
                        Name = packer.packer,
                        MovingDataID = movingDataId
                    };

                    await _queryGenerator.InsertInto(
                     new SqlQuery<MFC_VoxMe.Infrastructure.Models.Packers>()
                     {
                         Table = Constants.Tables.PACKERS,
                         Dto = newPacker
                     });
                }
            }

            if (jobDetails.jobInventory.loadingUnits != null)
            {
                _loadingUnits = jobDetails.jobInventory.loadingUnits;             
            }

            if (jobDetails.jobInventory.pieces != null)
            {
                var piecesDetails = jobDetails.jobInventory.pieces;
                foreach (var piece in piecesDetails)
                {
                    var query = await _queryGenerator.SelectFrom(
                     new SqlQuery<string>()
                     {
                         Columns = "Id",
                         Table = Constants.Tables.PACKERS,
                         LogOperator = IEnums.logOperator.LIKE,
                         ComparisonOperator = IEnums.logOperator.AND.ToString(),
                         WhereClause = SqlQuery<string>.Where
                                    ("Name", IEnums.logOperator.LIKE.ToString(), 
                                    @$"'{piece.packerName}'")
                                    + IEnums.logOperator.AND
                                    + SqlQuery<string>.Where("MovingDataID", 
                                    Constants.ComparisonOperators.EQUALTO, movingDataId)
                     });

                    int packerId = query[0].Id;

                    var roomQuery = await _queryGenerator.SelectFrom(
                    new SqlQuery<string>()
                    {
                        Columns = "Id",
                        Table = Constants.Tables.ROOMS,
                        LogOperator = IEnums.logOperator.LIKE,
                        ComparisonOperator = IEnums.logOperator.AND.ToString(),
                        WhereClause = SqlQuery<string>.Where
                                   ("Name", IEnums.logOperator.LIKE.ToString(), 
                                   @$"'{GetValueFromJsonConfig(piece.roomName)}'")
                                   + IEnums.logOperator.AND
                                   + SqlQuery<string>.Where("MovingDataID", 
                                   Constants.ComparisonOperators.EQUALTO, movingDataId)
                    });

                    int roomId = roomQuery[0].Id;

                   var boxTypeQuery = await _queryGenerator.SelectFrom(
                   new SqlQuery<string>()
                   {
                       Columns = "Id",
                       Table = Constants.Tables.ROOMS,

                       WhereClause = SqlQuery<string>.Where
                                  ("Name", IEnums.logOperator.LIKE.ToString(), 
                                  @$"'{GetValueFromJsonConfig(piece.roomName)}'")
                                  + IEnums.logOperator.AND
                                  + SqlQuery<string>.Where("MovingDataID", 
                                  Constants.ComparisonOperators.EQUALTO, movingDataId)
                   });

                int boxTypeId = boxTypeQuery[0].Id;

                   var skidQuery = await _queryGenerator.SelectFrom(
                   new SqlQuery<string>()
                   {
                       Columns = "Id",
                       Table = Constants.Tables.SKIDS,
                       WhereClause = SqlQuery<string>.Where
                                  ("Barcode", IEnums.logOperator.LIKE.ToString(), 
                                  @$"'{piece.loadUnitUniqueId}'")
                                  + IEnums.logOperator.AND
                                  + SqlQuery<string>.Where("MovingDataID", 
                                  Constants.ComparisonOperators.EQUALTO, movingDataId)
                   });

                    int skidId = skidQuery[0].Id;


                    var newPiece = new Pieces()
                    {
                        Description = piece.tag,
                        Barcode = piece.barcode,
                        PackerID = packerId, //?
                        RoomID = roomId, //?
                        PBO = piece.pbo,
                        BoxType = boxTypeId, //?
                        ShippingCost = piece.packageUnitCost,
                        BoxQty = piece.packageQty, 
                        ID = Convert.ToInt32(piece.labelNr),
                        Void = piece.@void,
                        Weight = piece.weight,
                        Width = piece.width,
                        Height = piece.height,
                        Length = piece.length,
                        Volume = piece.volume,
                        SkidID = skidId //?
                    };

                    await _queryGenerator.InsertInto(
                    new SqlQuery<MFC_VoxMe.Infrastructure.Models.Pieces>()
                    {
                        Table = Constants.Tables.PIECES,
                        Dto = newPiece
                    });

                    foreach (var item in piece.items)
                    {
                        var newItem = new Items()
                        {
                            Name = item.itemName,
                            Type = item.itemType,
                            ItemStatus = item.itemCategory,
                            Volume = item.volume,
                            Value = item.value.ToString(),
                            ValuationCurrency = item.valuationCurrency,
                            Qty = item.qty,
                            Condition = item.condition,
                            Make = item.make,
                            Model = item.model,
                            Year = item.year,
                            SerialNumber = item.serialNumber,
                            Width = item.width,
                            Height = item.height,
                            Length = item.length,
                            IsPart = item.isPart,
                            Dismantle = item.dismantle,
                            IsValuable = item.isValuable,
                            IsInventory = true,
                            PictureAuthor = item.pictureAuthor,
                            PictureTitle = item.pictureTitle,
                            PictureYear = item.pictureYear,
                            MaterialsDesc = item.materialsDesc,
                            CountryOrigin = item.countryOrigin,
                            Comment = item.comment,

                        };

                        await _queryGenerator.InsertInto(
                         new SqlQuery<MFC_VoxMe.Infrastructure.Models.Items>()
                         {
                             Table = Constants.Tables.ITEMS,
                             Dto = newItem
                         });
                    }
                }
            }         
        }

        public async Task InsertDataFromTransactionDetails(TransactionDetailsDto details, int movingDataId)
        {
            if (_loadingUnits != null)
            {
                var loadingUnitUniqueIds = details.loadingUnitUniqueIds;
                var skidQuery = await _queryGenerator.SelectFrom(
                    new SqlQuery<string>()
                    {
                        Columns = "ID",
                        Table = Constants.Tables.SKIDS,
                        Function = IEnums.functions.MAX,
                        As = "ID"
                    });

                int maxSkidId = skidQuery[0].ID;

                foreach (var unit in _loadingUnits)
                {
                    bool alreadyExist = loadingUnitUniqueIds.Contains(unit.uniqueId);
                    if (alreadyExist)
                    {
                        maxSkidId++;
                        var query = await _queryGenerator.SelectFrom(
                         new SqlQuery<string>()
                         {
                             Columns = "ID",
                             Table = Constants.Tables.SKIDTYPES,
                             LogOperator = IEnums.logOperator.LIKE,
                             WhereClause = SqlQuery<string>.Where
                                   ("Name", IEnums.logOperator.LIKE.ToString(), 
                                   $@"'{GetValueFromJsonConfig(unit.unitType)}'")
                         });

                        int skidType = query[0].ID;

                        var newSkid = new MFC_VoxMe.Infrastructure.Models.Skids()
                        {
                            MovingDataID = movingDataId,
                            ID = maxSkidId,
                            Barcode = unit.uniqueId,
                            TypeID = skidType,
                            SerialNumber = unit.serialNumber,
                            SealNumber = unit.sealNumber,
                            Location = unit.warehouseLocation,
                            Width = (int)unit.netWidth,
                            Height = (int)unit.netHeight,
                            GrossVolume = unit.grossVolume,
                            GrossWeight = unit.grossWeight,
                            ChargableVolume = unit.netVolume, //?
                            ChargableWeight = unit.netWeight, //?
                            PictureFileName = unit.photos
                            //Weight = unit.netWeight, //?
                            //Length = unit.extLength, //?

                        };

                        await _queryGenerator.InsertInto(
                            new SqlQuery<MFC_VoxMe.Infrastructure.Models.Skids>()
                            {
                                Table = Constants.Tables.SKIDS,
                                Dto = newSkid
                            });
                    }
                    else continue;
                }
            }
            var newMovingData = new MovingData()
            {
                State = Convert.ToInt32(GetValueFromJsonConfig
                        (details.onsiteStatus.Replace("Enum.TransactionOnsiteStatus.",""))),
                InventorySignature = details.clientSignature,
                DriverSignature = details.driverSignature,
                DestInventorySignature = details.destClientSignature,
                DestDriverSignature = details.destDriverSignature               

            };

            await _queryGenerator.InsertInto(
                  new SqlQuery<MovingData>()
                  {
                   Table = Constants.Tables.MOVINGDATA,
                   Dto = newMovingData
                  });

            var newPrefs = new Prefs()
            {
                PackingDate = details.scheduledDate,
                RealArrivalDate = details.crewArrivalOnSiteDate,
                DepartureDate = details.crewDepartureFromSiteDate,
                Comment = details.crewNotes,
            };

            await _queryGenerator.InsertInto(
                 new SqlQuery<Prefs>()
                 {
                     Table = Constants.Tables.PREFS,
                     Dto = newPrefs
                 });

            if (details.questionnaireQuestions != null)
            {
                var questionnaireQuestions = details.questionnaireQuestions;
                foreach (var item in questionnaireQuestions)
                {
                    var newMaterial = new MFC_VoxMe.Infrastructure.Models.Materials()
                    {
                        BoxType = item.name,
                        QtyTaken = item.numericValue == 0 ? item.booleanValue ? 1 : 0 : item.numericValue,
                        Description = item.stringValue ?? item.dateValue.ToString() ?? item.photoValue ?? item.signatureValue, //?
                        Value = item.listValues,
                        MovingDataID = movingDataId
                                       
                    };

                await _queryGenerator.InsertInto(
                new SqlQuery<MFC_VoxMe.Infrastructure.Models.Materials>()
                {
                    Table = Constants.Tables.MATERIALS,
                    Dto = newMaterial
                });
                }
            }
            if (details.timeSheets != null)
            {
                var timesheets = details.timeSheets;
                foreach (var item in timesheets)
                {
                    var newTimesheet = new PackersTimesheets()
                    {
                        StartTime = item.startDate,
                        EndTime = item.endDate,
                        Break1Duration = item.break1,
                        Break2Duration = item.break2,
                        Break3Duration = item.break3
                    };

                  await _queryGenerator.InsertInto(
                  new SqlQuery<PackersTimesheets>()
                  {
                      Table = Constants.Tables.PACKERSTIMESHEETS,
                      Dto = newTimesheet
                  });
                }        
            }       
             
            if (details.crewMembers != null)
            {
                var crewMembers = details.crewMembers;

                foreach(var crew in crewMembers)
                {
                    var newCrew = new MFC_VoxMe.Infrastructure.Models.Packers()
                    {
                        Name = crew.code,
                        IsForeman = crew.isForeman
                    };

                  await _queryGenerator.InsertInto(
                  new SqlQuery<MFC_VoxMe.Infrastructure.Models.Packers>()
                  {
                      Table = Constants.Tables.PACKERS,
                      Dto = newCrew
                  });
                }
            }
        }
        public async Task UpdateMovingData(string externalRef)
        {
            var test = new MovingData()
            {
                ClientName = "viseldaa test update 2",
            };
            var update = new SqlQuery<MovingData>();

            update.Dto = test;
            update.WhereClause = SqlQuery<string>.Where
                                    ("ExternalMFID", Constants.ComparisonOperators.EQUALTO, @$"'{externalRef}'");
            update.ComparisonOperator = Constants.ComparisonOperators.EQUALTO;

            await _queryGenerator.UpdateTable(update);
        }

        public async Task<dynamic> GetMovingDataId(string externalRef)
        {
            SqlQuery<string> select = new SqlQuery<string>()
            {
                Columns = "*",
                Table = Constants.Tables.MOVINGDATA,
                WhereClause = SqlQuery<string>.Where
                               ("ExternalMFID", Constants.ComparisonOperators.EQUALTO, @$"'{externalRef}'")
            };

            return await _queryGenerator.SelectFrom(select);
        }

        public async Task<string> GetItemsPath(int movingDataId)
        {
            SqlQuery<string> select = new SqlQuery<string>();
            select.Columns = "ItemsPath";
            select.Table = Constants.Tables.PREFS;
            select.WhereClause = SqlQuery<string>.Where
                               ("MovingDataId", Constants.ComparisonOperators.EQUALTO, @$"'{movingDataId}'");
            select.ComparisonOperator = Constants.ComparisonOperators.EQUALTO;

            var result = await _queryGenerator.SelectFrom(select);
            return _configuration.GetValue<string>("Client_Folder_Dir:stagingDir") + result.ItemsPath + "\\";
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



    }
}
