namespace MFC_VoxMe_API.Dtos.Transactions
{
    public class AssignStaffDesignateForemanDto
    {
        public List<StaffResourceCode> staffResourceCodes { get; set; }

    public class StaffResourceCode
    {
        public string code { get; set; }
        public bool isForeman { get; set; }
    }
}
}
