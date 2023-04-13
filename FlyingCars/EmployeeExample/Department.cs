namespace FlyingCars.EmployeeExample
{
    public class Department
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public ICollection<EmployeeDepartmentLink> Employees { get; set;}

        public Department(string title)
        {
            Title = title ?? throw new ArgumentNullException(title);
        }
    }
}