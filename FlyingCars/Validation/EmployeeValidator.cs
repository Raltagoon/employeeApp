using FluentValidation;
using FlyingCars.Models;

namespace FlyingCars.Validation
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(e => e.FirstName).NotEmpty();
            RuleFor(e => e.LastName).NotEmpty();
            RuleFor(e => e.Documents).Must(d => d.Count == 2);
        }
    }
}
