using MFC_VoxMe_API.Dtos.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MFC_VoxMe_API.BusinessLogic.AccessToken
{
    public class AccessTokenConfig: IAccessTokenConfig
    {
        private readonly IConfiguration _config;

        public AccessTokenConfig(IConfiguration config)
        {
            _config = config;
        }
        public AccessTokenConfigDto GetAccessTokenConfig()
        {
            return _config.GetSection
                ("API_Url:AccessToken").Get<AccessTokenConfigDto>();
        }
    }
}
