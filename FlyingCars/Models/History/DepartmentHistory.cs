namespace FlyingCars.Models.History
{
    public enum DepartmentEvent
    {
        Hiring,
        Transfer,
        Dismissal
    }

    public class DepartmentHistory
    {
        public int Id { get; set; }
        public DateOnly CreatedOn { get; set; }
        public DateOnly? DeletedOn { get; set; }
        public DepartmentEvent DepEvent { get; set; }

        public int EmployeeId { get; set; }
        public string DepartmentName { get; set; }

        public DepartmentHistory(DateOnly createdOn, DepartmentEvent depEvent, int employeeId, string departmentName)
        {
            CreatedOn = createdOn;
            DepEvent = depEvent;
            EmployeeId = employeeId;
            DepartmentName = departmentName;
        }
    }
}
