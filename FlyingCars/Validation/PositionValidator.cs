using FluentValidation;
using FlyingCars.Models;

namespace FlyingCars.Validation
{
    public class PositionValidator : AbstractValidator<Position>
    {
        public PositionValidator() 
        {
            RuleFor(p => p.Title).NotEmpty();
        }
    }
}
