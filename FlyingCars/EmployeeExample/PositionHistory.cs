namespace FlyingCars.EmployeeExample
{
    public class PositionHistory
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set;}
    }
}
