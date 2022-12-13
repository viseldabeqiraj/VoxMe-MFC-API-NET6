using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator;
using MFC_VoxMe.Infrastructure.Data.QueryGenerator.Helpers;
using MFC_VoxMe.Infrastructure.Models;
using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Transactions;
using MFC_VoxMe_API.Models;
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
                         columns = "ID",
                         table = Constants.Tables.SKIDTYPES,
                         logOperator = IEnums.logOperator.LIKE,
                         whereClause = new Dictionary<string, object>()
                         {
                             { "Name", "Liftvan"}
                         }
                     });

            int skidType = query[0].ID;
            return skidType;

        }

        public async Task InsertDataFromJobDetails(JobDetailsDto jobDetails, int movingDataId)
        {
            if (jobDetails.jobInventory.rooms != null)
            {
                var roomDetails = jobDetails.jobInventory.rooms;
                var maxRoomIdQuery = await _queryGenerator.SelectFrom(
                       new SqlQuery<string>()
                       {
                           columns = "ID",
                           table = Constants.Tables.ROOMS,
                           function = IEnums.functions.MAX,
                           As = "ID"
                       });

                short maxRoomId = maxRoomIdQuery.FirstOrDefault().ID;

                foreach (var room in roomDetails)
                {
                    var newRoom = new Rooms()
                    {
                        MovingDataID = movingDataId,
                        Name = room.name,
                        //Description = room.notes,
                        //NickName = room.name,
                        //MoveInCondition = room.conditionBeforeService.description,
                        //MoveInPictures = room.conditionBeforeService.photos,
                        //MoveOutCondition = room.conditionAfterService.description,
                        //MoveOutPictures = room.conditionAfterService.photos,
                        ID = (short)(maxRoomId + 1) //?
                    };

                    await _queryGenerator.InsertInto(
                        new SqlQuery<Rooms>()
                        {
                            table = Constants.Tables.ROOMS,
                            dto = newRoom
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
                var packersDetails = jobDetails.jobInventory.packers;

                foreach (var packer in packersDetails)
                {
                    var newPacker = new MFC_VoxMe.Infrastructure.Models.Packers
                    {
                        ID = 0, //?,
                        IsForeman = packer.isForeman,
                        Name = packer.packer,
                        MovingDataID = movingDataId
                    };
                }
            }
            if (jobDetails.jobInventory.pieces != null)
            {
                var piecesDetails = jobDetails.jobInventory.pieces;
                foreach (var piece in piecesDetails)
                {
                    var newPiece = new Pieces()
                    {
                        Description = piece.tag,
                        Barcode = piece.barcode,
                        PackerID = 0, //?
                        RoomID = 0, //?
                        PBO = piece.pbo,
                        BoxType = 0, //?
                        ShippingCost = piece.packageUnitCost,
                        BoxQty = 0, //?,
                        Void = piece.@void,
                        Weight = piece.weight,
                        Width = piece.width,
                        Height = piece.height,
                        Length = piece.length,
                        Volume = piece.volume,

                    };

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
                    }
                }
            }
         
            if (jobDetails.jobInventory.loadingUnits != null)
            {
                var loadingUnits = jobDetails.jobInventory.loadingUnits;

                foreach(var unit in loadingUnits)
                {
                    var query = await _queryGenerator.SelectFrom(
                     new SqlQuery<string>()
                     {
                         columns = "ID",
                         table = Constants.Tables.SKIDTYPES,
                         logOperator = IEnums.logOperator.LIKE,
                         whereClause = new Dictionary<string, object>()
                         {
                             { "Name", unit.unitType}
                         }
                     });

                    int skidType = query[0].ID; //get skid id for the 

                    var newSkid = new MFC_VoxMe.Infrastructure.Models.Skids()
                    {
                        Barcode = unit.uniqueId,
                        TypeID = skidType,
                        SerialNumber = unit.serialNumber,
                        SealNumber = unit.sealNumber,
                        Location = unit.warehouseLocation,
                        Width = (int)unit.netWidth,
                        Height = (int)unit.netHeight,
                       
                    };

                    await _queryGenerator.InsertInto(
                        new SqlQuery<MFC_VoxMe.Infrastructure.Models.Skids>()
                        {
                            table = Constants.Tables.SKIDS,
                            dto = newSkid
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
