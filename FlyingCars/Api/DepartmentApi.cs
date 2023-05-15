using FluentValidation;
using FlyingCars.Models;
using FlyingCars.Services;
using FlyingCars.Validation;

namespace FlyingCars.Api
{
    public class DepartmentApi
    {
        public void Register(WebApplication app)
        {
            // create
            app.MapPost("/department", async (DepartmentService service, Department department) =>
            {
                var validator = new DepartmentValidator();
                var validationResult = await validator.ValidateAsync(department);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                await service.AddAsync(department);
                return Results.Ok(department);
            });

            // read
            app.MapGet("/department", async (DepartmentService service) =>
            {
                return Results.Ok(await service.GetAllAsync());
            });

            // update
            app.MapPut("/department", async (DepartmentService service, Department department) =>
            {
                var validator = new DepartmentValidator();
                var validationResult = await validator.ValidateAsync(department);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                await service.UpdateAsync(department);
                return Results.Ok(department);
            });

            // delete
            app.MapDelete("/department/{id}", async (DepartmentService service, Guid id) =>
            {
                await service.DeleteByIdAsync(id);
                return Results.Ok();
            });
        }
    }
}
