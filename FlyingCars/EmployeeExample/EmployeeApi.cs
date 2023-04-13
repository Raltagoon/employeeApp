using ValidationsCollection;

namespace FlyingCars.EmployeeExample
{
    public class EmployeeApi
    {
        public void Register(WebApplication app)
        {
            // create
            app.MapPost("/department", async (EmployeeRepository repository, Department department) =>
            {
                await repository.AddDepartmentAsync(department);
                return Results.Ok(department);
            })
            .WithName("Create department")
            .WithTags("Employee.Create");

            app.MapPost("/position", async (EmployeeRepository repository, Position position) =>
            {
                await repository.AddPositionAsync(position);
                return Results.Ok(position);
            })
            .WithName("Create position")
            .WithTags("Employee.Create");
            app.MapPost("/employee", async (EmployeeRepository repository, Employee employee) =>
            {
                employee.Validate();
                await repository.AddEmployeeAsync(employee);
                return Results.Ok(employee);
            })
            .WithName("Create employee")
            .WithTags("Employee.Create");

            app.MapPost("/document", async (EmployeeRepository repository, Document document) =>
            {
                await repository.AddDocument(document);
                return Results.Ok(document);
            })
            .WithName("Create document")
            .WithTags("Employee.Create");

            // read
            app.MapGet("/department", async (EmployeeRepository repository) =>
            {
                return Results.Ok(await repository.GetAllDepartmentsAsync());
            })
            .WithName("Get all departments")
            .WithTags("Employee.Read");

            app.MapGet("/position", async (EmployeeRepository repository) =>
            {
                return Results.Ok(await repository.GetAllPositionsAsync());
            })
            .WithName("Get all positions")
            .WithTags("Employee.Read");

            app.MapGet("/employee", async (EmployeeRepository repository) =>
            {
                return Results.Ok(await repository.GetAllEmployeesAsync());
            })
            .WithName("Get all employees")
            .WithTags("Employee.Read");

            app.MapGet("/document", async (EmployeeRepository repository) =>
            {
                return Results.Ok(await repository.GetAllDocumentsAsync());
            })
            .WithName("Get all documents")
            .WithTags("Employee.Read");

            //update
            app.MapPut("/department", async (EmployeeRepository repository, Department department) =>
            {
                await repository.ChangeDepartmentAsync(department);
                return Results.Ok(department);
            })
            .WithName("Update department")
            .WithTags("Employee.Update");

            app.MapPut("/position", async (EmployeeRepository repository, Position position) =>
            {
                await repository.ChangePositionAsync(position);
                return Results.Ok(position);
            })
            .WithName("Update position")
            .WithTags("Employee.Update");

            app.MapPut("/employee", async (EmployeeRepository repository, Employee employee) =>
            {
                await repository.ChangeEmployeeAsync(employee);
                return Results.Ok(employee);
            })
            .WithName("Update employee")
            .WithTags("Employee.Update");

            app.MapPut("/document", async (EmployeeRepository repository, Document document) =>
            {
                await repository.ChangeDocumentAsync(document);
                return Results.Ok(document);
            })
            .WithName("Update document")
            .WithTags("Employee.Update");

            app.MapPut("/employee/changeposition", async (EmployeeRepository repository, int employeeId, int? newPositionId, int? oldPositionId) =>
            {
                await repository.ChangeEmployeePosition(employeeId, newPositionId, oldPositionId);
                return Results.Ok();
            });

            app.MapPut("/employee/transfer", async (EmployeeRepository repository, int employeeId, int? newDepartmentId, int? oldDepartmentId) =>
            {
                await repository.ChangeEmployeeDepartment(employeeId, newDepartmentId, oldDepartmentId);
                return Results.Ok();
            });

            // delete
            app.MapDelete("/department/{id}", async (EmployeeRepository repository, int id) =>
            {
                await repository.DeleteDepartmentByIdAsync(id);
                return Results.Ok();
            })
            .WithName("Delete department")
            .WithTags("Employee.Delete");

            app.MapDelete("/position/{id}", async (EmployeeRepository repository, int id) =>
            {
                await repository.DeletePositionByIdAsync(id);
                return Results.Ok();
            })
            .WithName("Delete position")
            .WithTags("Employee.Delete");

            app.MapDelete("/employee/{id}", async (EmployeeRepository repository, int id) =>
            {
                await repository.DeleteEmployeeByIdAsync(id);
                return Results.Ok();
            })
            .WithName("Delete employee")
            .WithTags("Employee.Delete");

            app.MapDelete("/document/{id}", async (EmployeeRepository repository, int id) =>
            {
                await repository.DeleteDocumentByIdAsync(id);
                return Results.Ok();
            })
            .WithName("Delete document")
            .WithTags("Employee.Delete");
        }
    }
}
