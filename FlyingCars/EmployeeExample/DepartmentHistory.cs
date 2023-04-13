namespace FlyingCars.EmployeeExample
{
    public class DepartmentHistory
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set;}
    }
}
