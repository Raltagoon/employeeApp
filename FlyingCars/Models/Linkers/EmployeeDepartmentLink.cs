using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace FlyingCars.Models.Linkers
{
    public class EmployeeDepartmentLink
    {
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
    }
}
