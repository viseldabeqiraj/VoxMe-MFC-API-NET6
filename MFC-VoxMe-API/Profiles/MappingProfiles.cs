using AutoMapper;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Transactions;
using MFC_VoxMe_API.Models;

namespace MFC_VoxMe_API.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //Job mapping profiles
            CreateMap<CreateJobDto, UpdateJobDto>();
            CreateMap<CreateJobDto.OriginAddress, UpdateJobDto.OriginAddress>();
            CreateMap<CreateJobDto.AddressDetails, UpdateJobDto.AddressDetails>();
            CreateMap<CreateJobDto.OriginPartyContact, UpdateJobDto.OriginPartyContact>();
            CreateMap<CreateJobDto.PersonDetails, UpdateJobDto.PersonDetails>();
            CreateMap<CreateJobDto.DestinationPartyContact, UpdateJobDto.DestinationPartyContact>();
            CreateMap<CreateJobDto.ContactDetails, UpdateJobDto.ContactDetails>();
            CreateMap<CreateJobDto.DestinationAddress, UpdateJobDto.DestinationAddress>();

            //Transaction mapping profiles
            CreateMap<CreateTransactionDto, UpdateTransactionDto>();
            CreateMap<CreateTransactionDto.OriginAddress, UpdateTransactionDto.OriginAddress>();
            CreateMap<CreateTransactionDto.AddressDetails, UpdateTransactionDto.AddressDetails>();
            CreateMap<CreateTransactionDto.OriginPartyContact, UpdateTransactionDto.OriginPartyContact>();
            CreateMap<CreateTransactionDto.PersonDetails, UpdateTransactionDto.PersonDetails>();
            CreateMap<CreateTransactionDto.DestinationPartyContact, UpdateTransactionDto.DestinationPartyContact>();
            CreateMap<CreateTransactionDto.ContactDetails, UpdateTransactionDto.ContactDetails>();
            CreateMap<CreateTransactionDto.DestinationAddress, UpdateTransactionDto.DestinationAddress>();
            CreateMap<CreateTransactionDto.QuestionnaireQuestion, UpdateTransactionDto.QuestionnaireQuestion>();
            CreateMap<CreateTransactionDto.AuxService, UpdateTransactionDto.AuxService>();
            CreateMap<CreateTransactionDto.LoadingUnit, UpdateTransactionDto.LoadingUnit>();
            CreateMap<CreateTransactionDto.LoadingUnitDetails, UpdateTransactionDto.LoadingUnitDetails>();

            //movingdata
            //CreateMap<CreateJobDto, MovingData>().ForMember(vm => vm.ExternalMFID, m => m.MapFrom(u => (u.externalRef != null)));

        }       

    }
}
