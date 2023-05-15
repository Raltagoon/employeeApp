using FluentValidation;
using FlyingCars.DTO;
using FlyingCars.Models;
using FlyingCars.Services;
using FlyingCars.Validation;

namespace FlyingCars.EmployeeExample
{
    public class EmployeeApi
    {
        public void Register(WebApplication app)
        {
            // create
            app.MapPost("/employee", async (EmployeeService service, Employee employee) =>
            {
                var validator = new EmployeeValidator();
                var validationResult = await validator.ValidateAsync(employee);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                await service.AddAsync(employee);
                return Results.Ok(employee);
            });

            // read
            app.MapGet("/employee", async (EmployeeService service) =>
            {
                return Results.Ok(await service.GetAllAsync());
            });

            app.MapGet("/employee/paged", async (EmployeeService service, int pageSize, int pageNumber) =>
            {
                return Results.Ok(await service.GetPagedList(pageSize, pageNumber));
            });

            app.MapGet("/employee/{id}", async (EmployeeService service, Guid id) =>
            {
                return Results.Ok(await service.GetByIdAsync(id));
            });

            app.MapGet("/employeewithnavigation", async (EmployeeService service, EmployeeNavigationProperties properties) =>
            {
                return Results.Ok(await service.GetAllWithNavigationPropertiesAsync(properties));
            });

            app.MapGet("/employeewithnavigation/{id}", async (EmployeeService service, Guid id, EmployeeNavigationProperties properties) =>
            {
                return Results.Ok(await service.GetByIdWithNavigationPropertiesAsync(id, properties));
            });

            //update
            app.MapPut("/employee", async (EmployeeService service, Employee employee) =>
            {
                await service.UpdateAsync(employee);
                return Results.Ok(employee);
            });

            // delete
            app.MapDelete("/employee/{id}", async (EmployeeService service, Guid id) =>
            {
                await service.DeleteByIdAsync(id);
                return Results.Ok();
            });

            // other
            //app.MapPut("/employee/addposition", async (EmployeeService service, Guid employeeId, Guid newPositionId, Guid? oldPositionId) =>
            //{
            //    await service.ChangePositionAsync(employeeId, newPositionId, oldPositionId);
            //});

            //other
            //app.MapGet("/employee/search/position", async (EmployeeService service, string position) =>
            //{
            //    if (await service.GetEmployeesByPositionAsync(position) is ICollection<EmployeeDto> employees)
            //    {
            //        return Results.Ok(employees);
            //    }
            //    else
            //    {
            //        return Results.NotFound();
            //    }
            //});
            //
            //app.MapGet("/employee/search/department", async (EmployeeService service, string department) =>
            //{
            //    if (await service.GetEmployeesByDepartmentAsync(department) is ICollection<EmployeeDto> employees)
            //    {
            //        return Results.Ok(employees);
            //    }
            //    else
            //    {
            //        return Results.NotFound();
            //    }
            //});
            //
            //app.MapPut("/employee/addposition", async (EmployeeService service, Guid employeeId, Guid newPositionId) =>
            //{
            //    await service.AddPositionAsync(employeeId, newPositionId);
            //});
            //
            //app.MapPut("/employee/removeposition", async (EmployeeService service, Guid employeeId, Guid oldPositionId) =>
            //{
            //    await service.RemovePositionAsync(employeeId, oldPositionId);
            //});
            //
            //app.MapPut("/employee/adddepartment", async (EmployeeService service, Guid employeeId, Guid newDepartmentId) =>
            //{
            //    await service.AddDepartmentAsync(employeeId, newDepartmentId);
            //});
            //
            //app.MapPut("/employee/removedepartment", async (EmployeeService service, Guid employeeId, Guid oldDepartmentId) =>
            //{
            //    await service.RemoveDepartmentAsync(employeeId, oldDepartmentId);
            //});
        }
    }
}
