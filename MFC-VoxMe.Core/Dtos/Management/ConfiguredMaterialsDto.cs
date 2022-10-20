namespace MFC_VoxMe_API.Dtos.Management
{
    public class ConfiguredMaterialsDto
    {
        public List<Material> materials { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Material
    {
        public string code { get; set; }
        public bool isCrate { get; set; }
        public bool isPackingMaterial { get; set; }
        public double materialUnitCost { get; set; }
    }
}
