using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace FlyingCars.Models
{
    public class EmployeeDepartmentLink
    {
        public Guid EmployeeId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
