namespace FlyingCars.EmployeeExample
{
    public class Position
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<EmployeePositionLink>? Employees { get; set; }
        public ICollection<PositionHistory>? Histories { get; set; }


        public Position(string title)
        {
            Title = title ?? throw new ArgumentNullException(title);
        }
    }
}