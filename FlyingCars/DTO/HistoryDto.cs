using FlyingCars.Models;

namespace FlyingCars.DTO
{
    public class HistoryDto
    {
        public Guid Id { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly? DeletedAt { get; set; }
        public HistoryType Type { get; set; }

        public Guid EmployeeId { get; set; }
        public string Description { get; set; }

        public HistoryDto(
            HistoryType type,
            Guid employeeId,
            string description,
            DateOnly createdAt,
            DateOnly? deletedAt,
            Guid id)
        {
            CreatedAt = createdAt;
            DeletedAt = deletedAt;
            Type = type;
            EmployeeId = employeeId;
            Description = description;
            Id = id;
        }
    }
}
