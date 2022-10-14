namespace MFC_VoxMe_API.Dtos.Common
{
    public class AccessTokenConfigDto
    {
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string scope { get; set; }
    }
}
