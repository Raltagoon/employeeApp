using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace FlyingCars.EmployeeExample
{
    public class EmployeeDepartmentLink
    {
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
    }
}
