namespace MFC_VoxMe_API.Dtos.Materials
{
    public class AssignMaterialsToTransactionDto
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public List<HandedMaterial> handedMaterials { get; set; }

        public class HandedMaterial
        {
            public string code { get; set; }
            public int qty { get; set; }
        }


    }
}
