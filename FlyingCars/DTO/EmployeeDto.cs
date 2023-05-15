namespace FlyingCars.DTO
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public EmployeeDto(
            Guid id,
            string firstName,
            string lastName,
            string? middleName,
            DateOnly dateOfBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            DateOfBirth = dateOfBirth;
        }
    }
}
