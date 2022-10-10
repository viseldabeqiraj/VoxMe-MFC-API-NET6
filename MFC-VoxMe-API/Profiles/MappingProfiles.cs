using AutoMapper;
using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;

namespace MFC_VoxMe_API.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<MovingData, CreateJobDto>();
        }       

    }
}
