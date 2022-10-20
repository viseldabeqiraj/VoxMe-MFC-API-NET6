using AutoMapper;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Transactions;

namespace MFC_VoxMe_API.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //Job mapping profiles
            CreateMap<CreateJobDto, UpdateJobDto>();
            CreateMap<UpdateJobDto, CreateJobDto>();
            CreateMap<CreateJobDto.OriginAddress, UpdateJobDto.OriginAddress>();
            CreateMap<CreateJobDto.AddressDetails, UpdateJobDto.AddressDetails>();
            CreateMap<CreateJobDto.OriginPartyContact, UpdateJobDto.OriginPartyContact>();
            CreateMap<CreateJobDto.PersonDetails, UpdateJobDto.PersonDetails>();
            CreateMap<CreateJobDto.DestinationPartyContact, UpdateJobDto.DestinationPartyContact>();
            CreateMap<CreateJobDto.ContactDetails, UpdateJobDto.ContactDetails>();
            CreateMap<CreateJobDto.DestinationAddress, UpdateJobDto.DestinationAddress>();

            //Transaction mapping profiles
            CreateMap<CreateTransactionDto, UpdateTransactionDto>();
            CreateMap<CreateJobDto, CreateTransactionDto>();
            CreateMap<CreateJobDto.OriginAddress, CreateTransactionDto.OriginAddress>();
            CreateMap<CreateJobDto.AddressDetails, CreateTransactionDto.AddressDetails>();
            CreateMap<CreateJobDto.OriginPartyContact, CreateTransactionDto.OriginPartyContact>();
            CreateMap<CreateJobDto.PersonDetails, CreateTransactionDto.PersonDetails>();
            CreateMap<CreateJobDto.DestinationPartyContact, CreateTransactionDto.DestinationPartyContact>();
            CreateMap<CreateJobDto.ContactDetails, CreateTransactionDto.ContactDetails>();
            CreateMap<CreateJobDto.DestinationAddress, CreateTransactionDto.DestinationAddress>();
        }       

    }
}
