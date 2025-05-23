namespace EHRM.ServiceLayer.Calendar
{
    public class LeaveDetailViewModel
    {
        public int EmpId { get; set; }          // Add EmpId
        public int LeaveApplyId { get; set; }
        public string LeaveType { get; set; }
        public string LeaveStatus { get; set; }
        public int LeaveId { get; set; }
        public string LeaveFrom { get; set; }   // varchar
        public string LeaveTo { get; set; }     // varchar
    }
}