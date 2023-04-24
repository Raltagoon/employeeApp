using FlyingCars.EmployeeExample;
using FlyingCars.Models.Linkers;

namespace FlyingCars.Models.Position
{
    public class Position
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<EmployeePositionLink>? Employees { get; set; }


        public Position(string title)
        {
            Title = title ?? throw new ArgumentNullException(title);
        }
    }
}