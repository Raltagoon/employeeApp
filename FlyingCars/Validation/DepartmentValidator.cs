using FluentValidation;
using FlyingCars.Models;

namespace FlyingCars.Validation
{
    public class DepartmentValidator : AbstractValidator<Department>
    {
        public DepartmentValidator()
        {
            RuleFor(d => d.Title).NotEmpty();
        }
    }
}
