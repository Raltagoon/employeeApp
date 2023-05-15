namespace FlyingCars.DTO
{
    public class EmployeeWithNavigationPropertiesDto
    {
        public EmployeeDto Employee { get; set; }
        public ICollection<PositionDto>? Positions { get; set; }
        public ICollection<DepartmentDto>? Departments { get; set; }
        public ICollection<HistoryDto>? Histories { get; set; }
    }
}
