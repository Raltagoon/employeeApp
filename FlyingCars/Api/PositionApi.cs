using FluentValidation;
using FlyingCars.Models;
using FlyingCars.Services;
using FlyingCars.Validation;

namespace FlyingCars.Api
{
    public class PositionApi
    {
        public void Register(WebApplication app)
        {
            // create
            app.MapPost("/position", async (PositionService service, Position position) =>
            {
                var validator = new PositionValidator();
                var validationResult = await validator.ValidateAsync(position);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                await service.AddAsync(position);
                return Results.Ok(position);
            });

            // read
            app.MapGet("/position", async (PositionService service) =>
            {
                return Results.Ok(await service.GetAllAsync());
            });

            // update
            app.MapPut("/position", async (PositionService service, Position position) =>
            {
                var validator = new PositionValidator();
                var validationResult = await validator.ValidateAsync(position);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                await service.UpdateAsync(position);
                return Results.Ok(position);
            });

            // delete
            app.MapDelete("/position/{id}", async (PositionService service, Guid id) =>
            {
                await service.DeleteByIdAsync(id);
                return Results.Ok();
            });
        }
    }
}
