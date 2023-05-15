namespace FlyingCars.DTO
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public DepartmentDto(
            Guid id,
            string title)
        {
            Id = id;
            Title = title;
        }
    }
}
