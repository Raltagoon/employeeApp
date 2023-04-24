namespace FlyingCars.Models.History
{
    public enum PositionEvent
    {
        Hiring,
        Promotion,
        Demotion,
        Dismissal
    }

    public class PositionHistory
    {
        public int Id { get; set; }
        public DateOnly CreatedOn { get; set; }
        public DateOnly? DeletedOn { get; set;}
        public PositionEvent PosEvent { get; set; }

        public int EmployeeId { get; set; }
        public string PositionName { get; set; }

        public PositionHistory(DateOnly createdOn, PositionEvent posEvent, int employeeId, string positionName)
        {
            CreatedOn = createdOn;
            PosEvent = posEvent;
            EmployeeId = employeeId;
            PositionName = positionName;
        }
    }
}
