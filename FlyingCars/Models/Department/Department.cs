using FlyingCars.Models;

namespace FlyingCars.Models
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public ICollection<EmployeeDepartmentLink>? Employees { get; set; }

        public Department(
            Guid id,
            string title)
        {
            Id = id;
            Title = title ?? throw new ArgumentNullException(title);
        }
    }
}