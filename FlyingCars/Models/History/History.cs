namespace FlyingCars.Models
{
    public enum HistoryType
    {
        Position = 10,
        Department = 20
    }

    public class History
    {
        public Guid Id { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly? DeletedAt { get; set; }
        public HistoryType Type { get; set; }

        public Guid EmployeeId { get; set; }
        public string Description { get; set; }

        public History(
            Guid id,
            DateOnly createdAt,
            HistoryType type,
            Guid employeeId,
            string description)
        {
            Id = id;
            CreatedAt = createdAt;
            Type = type;
            EmployeeId = employeeId;
            Description = description;
        }
    }
}
