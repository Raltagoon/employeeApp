using Microsoft.EntityFrameworkCore;

namespace FlyingCars.EmployeeExample
{
    public class EmployeeRepository
    {
        private readonly CarContext _context;
        private bool _disposed = false;

        public EmployeeRepository(CarContext context)
        {
            _context = context;
        }

        // create
        public async Task AddDepartmentAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
            await SaveAsync();
        }

        public async Task AddPositionAsync(Position position)
        {
            await _context.Positions.AddAsync(position);
            await SaveAsync();
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);

            var histories = new List<History>();
            foreach (var position in employee.Positions)
            {
                foreach (var department in employee.Departments)
                {
                    histories.Add(new History
                    {
                        EmployeeId = employee.Id,
                        CreatedOn = DateTime.Now,
                        PositionId = position.PositionId,
                        DepartmentId = department.DepartmentId
                    });
                }
            }
            await _context.Histories.AddRangeAsync(histories);
            await SaveAsync();
        }

        public async Task AddDocument(Document document)
        {
            await _context.Documents.AddAsync(document);
            await SaveAsync();
        }

        // read
        public async Task<ICollection<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<ICollection<Position>> GetAllPositionsAsync()
        {
            return await _context.Positions.ToListAsync();
        }

        public async Task<ICollection<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<ICollection<Document>> GetAllDocumentsAsync()
        {
            return await _context.Documents.ToListAsync();
        }

        // update
        public async Task ChangeDepartmentAsync(Department department)
        {
            var tmp = await _context.Departments.FindAsync(department.Id);

            if (tmp != null)
            {
                tmp.Title = department.Title;
                await SaveAsync();
            }
            else
            {
                throw new ArgumentException($"There is no department with ID {department.Id}");
            }
        }

        public async Task ChangePositionAsync(Position position)
        {
            var tmp = await _context.Positions.FindAsync(position.Id);

            if (tmp != null)
            {
                tmp.Title = position.Title;
                await SaveAsync();
            }
            else
            {
                throw new ArgumentException($"There is no position with ID {position.Id}");
            }
        }

        public async Task ChangeEmployeeAsync(Employee employee)
        {
            var tmp = await _context.Employees.FindAsync(employee.Id);

            if (tmp != null)
            {
                tmp.FirstName = employee.FirstName;
                tmp.LastName = employee.LastName;
                tmp.MiddleName = employee.MiddleName ?? tmp.MiddleName;
                tmp.DateOfBirth = employee.DateOfBirth;
                await SaveAsync();
            }
            else
            {
                throw new ArgumentException($"There is no employee with ID {employee.Id}");
            }
        }

        public async Task ChangeDocumentAsync(Document document)
        {
            var tmp = await _context.Documents.FindAsync(document.Id);

            if (tmp != null)
            {
                tmp.Type = document.Type;
                tmp.Number = document.Number;
                await SaveAsync();
            }
            else
            {
                throw new ArgumentException($"There is no document with ID {document.Id}");
            }
        }

        public async Task ChangeEmployeePosition(int employeeId, int? newPositionId, int? oldPositionId)
        {
            var employee = await _context.Employees.Include(e => e.Positions).Include(e => e.Departments).FirstAsync(e => e.Id == employeeId);

            if (oldPositionId.HasValue && !await _context.Positions.AnyAsync(p => p.Id == oldPositionId.Value))
            {
                throw new ArgumentException($"There is no position with ID: {oldPositionId}");
            }
            if (newPositionId.HasValue && !await _context.Positions.AnyAsync(p => p.Id == newPositionId.Value))
            {
                throw new ArgumentException($"There is no position with ID: {newPositionId}");
            }

            if (employee == null)
            {
                throw new ArgumentException($"There is no employee with ID: {employeeId}");
            }
            
            if (newPositionId.HasValue && oldPositionId.HasValue && newPositionId == oldPositionId) 
            {
                throw new ArgumentException("New position must differ from old one");
            }

            if (!newPositionId.HasValue && !oldPositionId.HasValue)
            {
                throw new ArgumentException("At least one posiiton id must be provided");
            }

            if (oldPositionId.HasValue)
            {
                var itemToRemove = employee.Positions.Single(p => p.EmployeeId == employeeId && p.PositionId == oldPositionId);
                employee.Positions.Remove(itemToRemove);
                var oldHistories = await _context.Histories.Where(h => h.EmployeeId == employeeId && h.DeletedOn == null && h.PositionId == oldPositionId).ToListAsync();
                foreach (var history in oldHistories)
                {
                    history.DeletedOn = DateTime.Now;
                }
            }

            if (newPositionId.HasValue)
            {
                employee.Positions.Add(new EmployeePositionLink { EmployeeId = employeeId, PositionId = newPositionId.Value });

                var histories = new List<History>();
                foreach (var department in employee.Departments)
                {
                    histories.Add(new History
                    {
                        EmployeeId = employeeId,
                        CreatedOn = DateTime.Now,
                        PositionId = newPositionId.Value,
                        DepartmentId = department.DepartmentId
                    });
                }
                await _context.Histories.AddRangeAsync(histories);
            }

            await SaveAsync();
        }
        
        public async Task ChangeEmployeeDepartment(int employeeId, int? newDepartmentId, int? oldDepartmentId)
        {
            var employee = await _context.Employees.Include(e => e.Positions).Include(e => e.Departments).FirstAsync(e => e.Id == employeeId);

            if (oldDepartmentId.HasValue && !await _context.Departments.AnyAsync(d => d.Id == oldDepartmentId.Value)) 
            {
                throw new ArgumentException($"There is no department with ID: {oldDepartmentId}");
            }

            if (newDepartmentId.HasValue && !await _context.Departments.AnyAsync(d => d.Id == newDepartmentId.Value))
            {
                throw new ArgumentException($"There is no department with ID: {newDepartmentId}");
            }

            if (employee == null)
            {
                throw new ArgumentException($"There is no employee with ID {employeeId}");
            }

            if (newDepartmentId.HasValue && oldDepartmentId.HasValue && newDepartmentId == oldDepartmentId)
            {
                throw new ArgumentException("New department must differ from old one");
            }

            if (!newDepartmentId.HasValue && !oldDepartmentId.HasValue) 
            {
                throw new ArgumentException("At least one department id must be provided");
            }

            if (oldDepartmentId.HasValue)
            {
                var itemToRemove = employee.Departments.Single(p => p.EmployeeId == employeeId && p.DepartmentId == oldDepartmentId);
                employee.Departments.Remove(itemToRemove);
                var oldHistories = await _context.Histories.Where(h => h.EmployeeId == employeeId && h.DeletedOn == null && h.DepartmentId == oldDepartmentId).ToListAsync();
                foreach (var history in oldHistories)
                {
                    history.DeletedOn = DateTime.Now;
                }
            }

            if (newDepartmentId.HasValue)
            {
                employee.Departments.Add(new EmployeeDepartmentLink { EmployeeId = employeeId, DepartmentId = newDepartmentId.Value });

                var histories = new List<History>();
                foreach (var position in employee.Positions)
                {
                    histories.Add(new History
                    {
                        EmployeeId = employeeId,
                        CreatedOn = DateTime.Now,
                        PositionId = position.PositionId,
                        DepartmentId = newDepartmentId.Value
                    });
                }
                await _context.Histories.AddRangeAsync(histories);
            }
            await SaveAsync();
        }

        // delete
        public async Task DeleteDepartmentByIdAsync(int id)
        {
            var tmp = await _context.Departments.FindAsync(id);
            if (tmp == null)
            {
                throw new ArgumentException($"There is no department with ID {id}");
            }
            _context.Departments.Remove(tmp);
            await SaveAsync();
        }

        public async Task DeletePositionByIdAsync(int id)
        {
            var tmp = await _context.Positions.FindAsync(id);
            if (tmp == null)
            {
                throw new ArgumentException($"There is no posiition with ID {id}");
            }
            _context.Positions.Remove(tmp);
            await SaveAsync();
        }

        public async Task DeleteEmployeeByIdAsync(int id)
        {
            var tmp = await _context.Employees.FindAsync(id);
            if (tmp == null)
            {
                throw new ArgumentException($"There is no employee with ID {id}");
            }

            var histories = await _context.Histories.Where(h => h.EmployeeId == id && h.DeletedOn == null).ToListAsync();
            foreach (var history in histories)
            {
                history.DeletedOn = DateTime.Now;
            }

            _context.Employees.Remove(tmp);
            await SaveAsync();
        }

        public async Task DeleteDocumentByIdAsync(int id)
        {
            var tmp = await _context.Documents.FindAsync(id);
            if (tmp == null)
            {
                throw new ArgumentException($"There is no document with ID {id}");
            }
            _context.Documents.Remove(tmp);
            await SaveAsync();
        }

        // service
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
