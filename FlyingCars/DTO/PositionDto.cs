namespace FlyingCars.DTO
{
    public class PositionDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public PositionDto(
            Guid id,
            string title)
        {
            Id = id;
            Title = title;
        }
    }
}
