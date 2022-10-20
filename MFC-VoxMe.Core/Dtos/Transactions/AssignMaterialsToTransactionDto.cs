
namespace MFC_VoxMe_API.Dtos.Transactions
{
    public class AssignMaterialsToTransactionDto
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public List<HandedMaterial> handedMaterials { get; set; }

        public class HandedMaterial
        {
            public string code { get; set; }
            public double qty { get; set; }
        }


    }
}
