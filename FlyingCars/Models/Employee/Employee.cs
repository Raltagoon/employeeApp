namespace FlyingCars.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public ICollection<EmployeeDepartmentLink> Departments { get; set; } = new List<EmployeeDepartmentLink>();
        public ICollection<EmployeePositionLink> Positions { get; set; } = new List<EmployeePositionLink>();

        protected Employee()
        {
        }

        public Employee(
            Guid id,
            string firstName,
            string lastName,
            DateOnly dateOfBirth,
            string? middleName = null)
        {
            Id = id;
            FirstName = firstName ?? throw new ArgumentNullException(firstName);
            LastName = lastName ?? throw new ArgumentNullException(lastName);
            MiddleName = middleName;
            DateOfBirth = dateOfBirth;
        }

        public void RemovePosition(Guid positiodId)
        {
            var positionToRemove = Positions.FirstOrDefault(p => p.PositionId == positiodId);
            if (positionToRemove != null)
                Positions.Remove(positionToRemove);
            else
                throw new ArgumentException("employee doesnt have such a position");
        }
    }
}
