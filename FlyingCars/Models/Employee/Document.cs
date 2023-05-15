using ValidationsCollection;

namespace FlyingCars.Models
{
    public enum DocumentType
    {
        Inn = 10,
        Snils = 20
    }

    public class Document
    {
        public Guid Id { get; set; }
        public DocumentType Type { get; set; }
        public string Number { get; set; }

        public Guid EmployeeId { get; set; }

        public Document(
            Guid id,
            DocumentType type,
            string number,
            Guid employeeId)
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
            Id = id;
            Type = type;
            EmployeeId = employeeId;
        }
    }
}
