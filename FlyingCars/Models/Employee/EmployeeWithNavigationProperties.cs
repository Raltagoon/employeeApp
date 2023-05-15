namespace FlyingCars.Models
{
    [Flags]
    public enum EmployeeNavigationProperties
    {
        None = 0,
        Positions = 1,
        Departments = 2,
        Histories = 4,
        All = ~None
    }

    public class EmployeeWithNavigationProperties
    {
        public Employee Employee { get; set; }
        public ICollection<Position>? Positions { get; set; }
        public ICollection<Department>? Departments { get; set; }
        public ICollection<History>? Histories { get; set; }
    }
}
