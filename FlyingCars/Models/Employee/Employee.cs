using System.ComponentModel.DataAnnotations;
using FlyingCars.Models.Linkers;

namespace FlyingCars.Models.Employee
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public ICollection<Document> Documents { get; set; }
        public ICollection<EmployeeDepartmentLink> Departments { get; set; }
        public ICollection<EmployeePositionLink> Positions { get; set; }

        public Employee(string firstName, string lastName, DateOnly dateOfBirth, string? middleName = null)
        {
            FirstName = firstName ?? throw new ArgumentNullException(firstName);
            LastName = lastName ?? throw new ArgumentNullException(lastName);
            MiddleName = middleName;
            DateOfBirth = dateOfBirth;
        }
        public void Validate()
        {
            if (Documents == null || Documents.Count != 2)
            {
                throw new ArgumentException("Inn and Snils must be provided.");
            }
        }
    }
}
