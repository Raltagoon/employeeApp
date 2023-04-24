using ValidationsCollection;

namespace FlyingCars.Models.Employee
{
    public enum DocumentType
    {
        Inn,
        Snils
    }

    public class Document
    {
        public int Id { get; set; }
        public DocumentType Type { get; set; }
        public string Number { get; set; }

        public int EmployeeId { get; set; }

        public Document(DocumentType type, string number, int employeeId)
        {
            switch (type)
            {
                case DocumentType.Inn:
                    Number = Validations.IsValidInnForIndividual(number) ? number : throw new ArgumentException(number);
                    break;
                case DocumentType.Snils:
                    Number = Validations.IsValidSnils(number) ? number : throw new ArgumentException(number);
                    break;
                default:
                    throw new ArgumentNullException(nameof(type));
            }
            Type = type;
            EmployeeId = employeeId;
        }
    }
}
