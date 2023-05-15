namespace FlyingCars.Models
{
    public class Position
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public ICollection<EmployeePositionLink>? Employees { get; set; }


        public Position(
            Guid id,
            string title)
        {
            Id = id;
            Title = title ?? throw new ArgumentNullException(title);
        }
    }
}