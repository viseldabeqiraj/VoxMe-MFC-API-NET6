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

        public void CreateFileInFolder(string filePath, byte[] bytes)
        {
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.Write(bytes);
            }
        }

        public void DeleteFilesFromFolder(List<string> serverPaths)
        {
            foreach(var path in serverPaths)
            {
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] files = di.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.Delete();
                }
            }
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

        //public async Task<List<ServicePaperworkModel>> GetPaperworkDocuments(MovingDataDto movingData)
        //{
        //    var paperworkDocuments = await _queryGenerator.GetRequiredPaperwork(movingData);
        //    return paperworkDocuments;
        //}

        public async Task InsertDataFromJobDetails(JobDetailsDto jobDetails, int movingDataId)
        {
            if (jobDetails.jobInventory.rooms != null)
            {
                var roomDetails = jobDetails.jobInventory.rooms;
                short maxRoomId = 1;

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
                int maxPackerId = 1;

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
            var loadingUnits = jobDetails.jobInventory.loadingUnits;
            if (loadingUnits != null)
            {              
                int maxSkidId =1;

                foreach (var unit in loadingUnits)
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
                       Table = Constants.Tables.BOXES,

                       WhereClause = SqlQuery<string>.Where
                                  ("Type", IEnums.logOperator.LIKE.ToString(), 
                                  @$"'{GetValueFromJsonConfig(piece.packageType)}'")
                                  
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
                        MovingDataID = movingDataId,
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
                        SkidID = skidId, //?
                        
                    };

                    await _queryGenerator.InsertInto(
                    new SqlQuery<MFC_VoxMe.Infrastructure.Models.Pieces>()
                    {
                        Table = Constants.Tables.PIECES,
                        Dto = newPiece
                    });
                    short id = 0;
                    foreach (var item in piece.items)
                    {
                        id++;
                        var conditionQuery = await _queryGenerator.SelectFrom(
                          new SqlQuery<string>()
                          {
                              Columns = "ID",
                              Table = Constants.Tables.CONDITIONS,
                              WhereClause = SqlQuery<string>.Where
                                         ("Name", IEnums.logOperator.LIKE.ToString(),
                                         @$"'{GetValueFromJsonConfig(item.condition)}'")                                    
                          });

                        var conditionId = conditionQuery[0].ID;
                        var newItem = new Items()
                        {
                            ID = id,
                            PieceID = newPiece.ID,
                            Name = GetValueFromJsonConfig(item.itemName),
                            Type = GetValueFromJsonConfig(item.itemType),
                            ItemStatus = GetValueFromJsonConfig(item.itemCategory),
                            Volume = item.volume,
                            Value = item.value.ToString(),
                            ValuationCurrency = item.valuationCurrency,
                            Qty = item.qty,
                            Condition = conditionId,
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
                            MovingDataID = movingDataId

                        };

                        await _queryGenerator.InsertInto(
                         new SqlQuery<Items>()
                         {
                             Table = Constants.Tables.ITEMS,
                             Dto = newItem
                         });
                    }
                }
            }         
        }

        public async Task DeleteTables(int movingDataID)
        {
            var tables = new List<string>()
            {
                "Rooms","Packers","Skids","Pieces","Items", "Materials", "PackersTimesheets"
            };
            foreach (var table in tables)
            {
                await _queryGenerator.Delete(
                     new SqlQuery<string>()
                     {
                         Table = table,
                         WhereClause = SqlQuery<string>.Where
                                       ("MovingDataID", Constants.ComparisonOperators.EQUALTO,
                                       @$"'{movingDataID}'")
                     });
            }
        }

        public async Task UpdateMovingDataStatus(int state, int movingDataID)
        {
            var movingData = new MovingData()
            {
                State = state
            };
            await _queryGenerator.UpdateTable(
                new SqlQuery<MovingData>()
                {
                    Dto = movingData,
                    WhereClause = SqlQuery<string>.Where
                                  ("ID", Constants.ComparisonOperators.EQUALTO,
                                  @$"'{movingDataID}'")
                });
        }


        public async Task InsertDataFromTransactionDetails(TransactionDetailsDto details, int movingDataId)
        {           
            var newMovingData = new MovingData()
            {
                State = Convert.ToInt32(GetValueFromJsonConfig
                        (details.onsiteStatus.Replace("Enum.TransactionOnSiteStatus.", ""))),
                InventorySignature = details.clientSignature,
                DriverSignature = details.driverSignature,
                DestInventorySignature = details.destClientSignature,
                DestDriverSignature = details.destDriverSignature         
            };

            await _queryGenerator.UpdateTable(
                  new SqlQuery<MovingData>()
                  {
                   Dto = newMovingData,
                   WhereClause = SqlQuery<string>.Where
                                       ("ID", Constants.ComparisonOperators.EQUALTO,
                                       @$"'{movingDataId}'")
                  });

            var newPrefs = new Prefs()
            {
                PackingDate = details.scheduledDate,
                RealArrivalDate = details.crewArrivalOnSiteDate,
                DepartureDate = details.crewDepartureFromSiteDate,
                Comment = details.crewNotes,
                MovingDataID = movingDataId
            };

            await _queryGenerator.UpdateTable(
                 new SqlQuery<Prefs>()
                 {
                     Dto = newPrefs,
                     WhereClause = SqlQuery<string>.Where
                                       ("MovingDataID", Constants.ComparisonOperators.EQUALTO,
                                       @$"'{movingDataId}'")
                 });

            if (details.questionnaireQuestions != null)
            {
                var questionnaireQuestions = details.questionnaireQuestions;
                foreach (var item in questionnaireQuestions)
                {
                    var newMaterial = new MFC_VoxMe.Infrastructure.Models.Materials()
                    {
                        BoxType = GetValueFromJsonConfig(item.name),
                        QtyTaken = item.numericValue == 0 
                        ? item.booleanValue ? 1 : 0 : item.numericValue,
                        Description = item.stringValue 
                        ?? item.dateValue.ToString() ?? item.photoValue ?? item.signatureValue, //?
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
             if (details.crewMembers != null)
            {
                var crewMembers = details.crewMembers;

                foreach(var crew in crewMembers)
                {
                    var newCrew = new MFC_VoxMe.Infrastructure.Models.Packers()
                    {
                        Name = crew.code,
                        IsForeman = crew.isForeman,
                        MovingDataID = movingDataId
                    };

                  await _queryGenerator.UpdateTable(
                  new SqlQuery<MFC_VoxMe.Infrastructure.Models.Packers>()
                  {
                      Dto = newCrew,
                      WhereClause = SqlQuery<string>.Where
                                       ("MovingDataID", Constants.ComparisonOperators.EQUALTO,
                                       @$"'{movingDataId}'")
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
                        Break3Duration = item.break3,
                        MovingDataId = movingDataId
                    };

                  await _queryGenerator.InsertInto(
                  new SqlQuery<PackersTimesheets>()
                  {
                      Table = Constants.Tables.PACKERSTIMESHEETS,
                      Dto = newTimesheet
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

            return _configuration.GetValue<string>
                ("Client_Folder_Dir:stagingDir") + result[0].ItemsPath + "\\";
        }

        public List<KeyValuePair<string, string>> GetImages(JobDetailsDto jobDetails,TransactionDetailsDto transactiondetails)
        {
            var questionnaireQuestions = transactiondetails?.questionnaireQuestions.ToList();
            var auxServices = transactiondetails?.auxServices.ToList();

            List<KeyValuePair<string, string>> imagesToStore =
                        new List<KeyValuePair<string, string>>();
            var lists = questionnaireQuestions?.Zip(auxServices, (q, a) =>
                              new { questionnaireQuestions = q, auxServices = a });
            if (lists is not null)
            {
                foreach (var item in lists)
                {
                    if (!string.IsNullOrEmpty(item.questionnaireQuestions.signatureValue))
                    imagesToStore.Add
                            (new KeyValuePair<string, string>
                            ("QuestionnaireQuestions", item.questionnaireQuestions.signatureValue));

                    if (!string.IsNullOrEmpty(item.questionnaireQuestions.photoValue))
                        imagesToStore.Add
                            (new KeyValuePair<string, string>
                            ("QuestionnaireQuestions", item.questionnaireQuestions.photoValue));

                    if (!string.IsNullOrEmpty(item.auxServices.signatureValue))
                        imagesToStore.Add
                            (new KeyValuePair<string, string>
                            ("AuxServices", item?.auxServices.signatureValue));

                    if (!string.IsNullOrEmpty(item?.auxServices.photoValue))
                        imagesToStore.Add
                            (new KeyValuePair<string, string>
                            ("AuxServices", item?.auxServices.photoValue));
                }
            }

            foreach (var item in jobDetails.jobInventory.rooms)
            {
                if (!string.IsNullOrEmpty(item.conditionBeforeService?.photos))
                    imagesToStore.Add
                           (new KeyValuePair<string, string>
                           ("Rooms", item.conditionBeforeService?.photos));

                if (!string.IsNullOrEmpty(item.conditionAfterService?.photos))
                    imagesToStore.Add
                           (new KeyValuePair<string, string>
                           ("Rooms", item.conditionAfterService?.photos));
            }
            if (!string.IsNullOrEmpty(transactiondetails.clientSignature))
                imagesToStore.Add
               (new KeyValuePair<string, string>
               ("Transaction", transactiondetails.clientSignature));

            if (!string.IsNullOrEmpty(transactiondetails.driverSignature))
                imagesToStore.Add
               (new KeyValuePair<string, string>
               ("Transaction", transactiondetails.driverSignature));

            if (!string.IsNullOrEmpty(transactiondetails.destDriverSignature))
                imagesToStore.Add
               (new KeyValuePair<string, string>
               ("Transaction", transactiondetails.destDriverSignature));

            if (!string.IsNullOrEmpty(transactiondetails.destClientSignature))
                imagesToStore.Add
               (new KeyValuePair<string, string>
               ("Transaction", transactiondetails.destClientSignature));
            return imagesToStore;
        }



    }
}
