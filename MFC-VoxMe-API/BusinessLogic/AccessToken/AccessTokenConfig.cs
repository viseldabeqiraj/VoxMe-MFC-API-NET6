using MFC_VoxMe_API.Dtos.Common;
using Newtonsoft.Json;

namespace MFC_VoxMe_API.BusinessLogic.AccessToken
{
    public class AccessTokenConfig
    {
        private readonly IConfiguration _config;

        public AccessTokenConfig(IConfiguration config)
        {
            _config = config;
        }
        public AccessTokenConfigDto GetAccessTokenConfig()
        {
            var AccessTokenConfig = _config.GetValue<string>("API_Url:AccessToken");

            return JsonConvert.DeserializeObject<AccessTokenConfigDto>(AccessTokenConfig);
        }
    }
}
