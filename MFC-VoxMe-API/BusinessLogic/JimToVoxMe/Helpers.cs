using Dapper;
using MFC_VoxMe.Infrastructure.Data;
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
        private readonly DapperContext _context;

        public Helpers(DapperContext context)
        {
            _context = context;
        }

        public MovingDataDto XMLParse(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MovingDataDto));
            MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            MovingDataDto movingDataFromXml = (MovingDataDto)serializer.Deserialize(memStream);
            _MovingData = movingDataFromXml;
            InsertMovingDataRecords();
            return movingDataFromXml;

        }

        //public void setProperties()
        //{
        //    var moving = new MovingData();
        //    //PropertyMatcher<CreateJobDto, MovingData>.GenerateMatchedObject(CreateJobObjectFromXml(), moving);
        //    //PropertyMatcher<CreateJobDto.Client, MovingData>.GenerateMatchedObject(CreateJobObjectFromXml().client, moving);
        //    var json = JsonConvert.SerializeObject(CreateTransactionObjectFromXml());//JsonSerializer.Serialize(CreateJobObjectFromXml());
        //    var to = JsonConvert.DeserializeObject<MovingData>(json);//JsonSerializer.Deserialize<MovingData>(json);
        //}

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

            createJobDto.serviceLevel = generalInfo.Preferences.ServiceLevel;
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
                    salutation = generalInfo.ClientSalutation ?? "Enum.Salutation." + generalInfo.ClientSalutation,
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

        public void InsertMovingDataRecords()
        {
            var generalInfo = _MovingData.GeneralInfo;

            var dto = new MovingData()
            {
                ClientName = generalInfo.ClientFirstName,
                Date = DateTime.Parse(generalInfo.Preferences.PackingDate),
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
            InsertInto("MovingData", dto);
        }

        public void InsertInto<T>(string table, T dto) //where T : new()
        {
            var colsList = new List<string>();
            var dataList = new List<string>();

            foreach (var propertyInfo in dto.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(dto) != null)
                {
                    colsList.Add(propertyInfo.Name);
                    dataList.Add("'" + propertyInfo.GetValue(dto).ToString() + "'");
                }
            }
            string cols = string.Join(",", colsList);
            string data = string.Join(",", dataList);

            var query = @$"INSERT INTO [dbo].[{table}]
                               ({cols})
                         VALUES
                               ({data})";
            using (var connection = _context.CreateConnection())
            {
                var result = connection.Query<string>(query);
            }
        }
    }
}
