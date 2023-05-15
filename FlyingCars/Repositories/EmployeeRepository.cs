using FlyingCars.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyingCars.Repositories
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
        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await SaveAsync();
        }

        // read
        public async Task<ICollection<Employee>> GetAllAsync()
        {
            var employees = await _context.Employees.ToListAsync();
            if (!employees.Any())
                throw new ArgumentException("there are no employees");
            return employees;
        }

        public async Task<ICollection<Employee>> GetPagedList(int pageSize, int pageNumber)
        {
            return await _context.Employees.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null)
                throw new ArgumentException($"there is no employee with id {id}");
            return employee;
        }

        public async Task<EmployeeWithNavigationProperties> GetByIdWithNavigationPropertiesAsync(Guid id, EmployeeNavigationProperties properties)
        {
            var arePositionsNeeded = properties.HasFlag(EmployeeNavigationProperties.Positions);
            var areDepartmentsNeeded = properties.HasFlag(EmployeeNavigationProperties.Departments);
            var areHistoriesNeeded = properties.HasFlag(EmployeeNavigationProperties.Histories);

            var query = _context.Employees.AsQueryable();

            if (arePositionsNeeded)
            {
                query = query.Include(e => e.Positions);
            }

            if (areDepartmentsNeeded)
            {
                query = query.Include(e => e.Departments);
            }

            var employee = await query
                .Select(e => new EmployeeWithNavigationProperties
                {
                    Employee = e,
                    Positions = arePositionsNeeded
                        ? e.Positions.Join(_context.Positions,
                                           link => link.PositionId,
                                           pos => pos.Id,
                                           (link, pos) => new Position(link.PositionId, pos.Title))
                                     .ToList()
                        : null,
                    Departments = areDepartmentsNeeded
                        ? e.Departments.Join(_context.Departments,
                                             link => link.DepartmentId,
                                             dep => dep.Id,
                                             (link, dep) => new Department(link.DepartmentId, dep.Title))
                                       .ToList()
                        : null,
                    Histories = areHistoriesNeeded
                        ? _context.Histories.Where(h => h.EmployeeId == e.Id).ToList()
                        : null
                })
                .Where(e => e.Employee.Id == id)
                .FirstOrDefaultAsync();

            if (employee != null)
                return employee;
            else
                throw new ArgumentException($"there is no employee with id {id}");
        }

        public async Task<List<EmployeeWithNavigationProperties>> GetAllWithNavigationPropertiesAsync(EmployeeNavigationProperties properties)
        {
            var arePositionsNeeded = properties.HasFlag(EmployeeNavigationProperties.Positions);
            var areDepartmentsNeeded = properties.HasFlag(EmployeeNavigationProperties.Departments);
            var areHistoriesNeeded = properties.HasFlag(EmployeeNavigationProperties.Histories);

            var query = _context.Employees.AsQueryable();

            if (arePositionsNeeded)
            {
                query = query.Include(e => e.Positions);
            }

            if (areDepartmentsNeeded)
            {
                query = query.Include(e => e.Departments);
            }

            return await query
                .Select(e => new EmployeeWithNavigationProperties
                {
                    Employee = e,
                    Positions = arePositionsNeeded
                        ? e.Positions.Join(_context.Positions,
                                           link => link.PositionId,
                                           pos => pos.Id,
                                           (link, pos) => new Position(link.PositionId, pos.Title))
                                     .ToList()
                        : null,
                    Departments = areDepartmentsNeeded
                        ? e.Departments.Join(_context.Departments,
                                             link => link.DepartmentId,
                                             dep => dep.Id,
                                             (link, dep) => new Department(link.DepartmentId, dep.Title))
                                       .ToList()
                        : null,
                    Histories = areHistoriesNeeded
                        ? _context.Histories.Where(h => h.EmployeeId == e.Id).ToList()
                        : null
                })
                .ToListAsync();
        }

        // update
        public async Task UpdateAsync(Employee employee)
        {
            _context.Update(employee);
            await SaveAsync();

            // TODO: а тут только измененнные проперти modified
            //if (await _context.Employees.FindAsync(employee.Id) is Employee tmp)
            //{
            //    _context.Entry(tmp).CurrentValues.SetValues(employee);
            //    await SaveAsync();
            //}
        }

        // delete
        public async Task DeleteByIdAsync(Guid id)
        {
            // TODO: как без поиска entity удалить?
            var tmp = await _context.Employees.FindAsync(id);
            if (tmp == null)
            {
                throw new ArgumentException($"There is no employee with ID {id}");
            }
            
            _context.Employees.Remove(tmp);

            await SaveAsync();
        }

        // other
        //public async Task<ICollection<Employee>> GetByPositionAsync(string position)
        //{
        //    var positionResult = await _context.Positions.Where(p => p.Title == position)
        //                                                 .FirstAsync();
        //    var employees = await _context.Employees.Where(e => e.Positions.Any(epl => epl.PositionId == positionResult.Id))
        //                                            .ToListAsync();
        //    return employees;
        //}
        //
        //public async Task<ICollection<Employee>> GetByDepartmentAsync(string department)
        //{
        //    var departmentResult = await _context.Departments.Where(d => d.Title == department)
        //                                                     .FirstAsync();
        //    var employees = await _context.Employees.Where(e => e.Departments.Any(edl => edl.DepartmentId == departmentResult.Id))
        //                                            .ToListAsync();
        //    return employees;
        //}
        //
        //public async Task AddPositionAsync(Guid employeeId, Guid newPositionId)
        //{
        //    if (!await _context.Positions.AnyAsync(p => p.Id == newPositionId))
        //    {
        //        throw new ArgumentException($"There is no position with ID: {newPositionId}");
        //    }
        //
        //    var employee = await _context.Employees.Include(e => e.Positions).FirstAsync(e => e.Id == employeeId);
        //
        //    if (employee == null)
        //    {
        //        throw new ArgumentException($"There is no employee with ID: {employeeId}");
        //    }
        //
        //    employee.Positions.Add(new EmployeePositionLink { EmployeeId = employeeId, PositionId = newPositionId });
        //
        //    var positionName = await _context.Positions.Where(p => p.Id == newPositionId)
        //                                               .Select(p => p.Title)
        //                                               .FirstAsync();
        //    var history = new History(Guid.NewGuid(),
        //                              DateOnly.FromDateTime(DateTime.Now),
        //                              HistoryType.Position,
        //                              employeeId,
        //                              positionName);
        //    await _context.Histories.AddAsync(history);
        //
        //    await SaveAsync();
        //}
        //
        //public async Task RemovePositionAsync(Guid employeeId, Guid oldPositionId)
        //{
        //    if (!await _context.Positions.AnyAsync(p => p.Id == oldPositionId))
        //    {
        //        throw new ArgumentException($"There is no position with ID: {oldPositionId}");
        //    }
        //
        //    var employee = await _context.Employees.Include(e => e.Positions).FirstAsync(e => e.Id == employeeId);
        //
        //    if (employee == null)
        //    {
        //        throw new ArgumentException($"There is no employee with ID: {employeeId}");
        //    }
        //
        //    var itemToRemove = employee.Positions.Single(p => p.EmployeeId == employeeId && p.PositionId == oldPositionId);
        //    employee.Positions.Remove(itemToRemove);
        //
        //    var positionName = await _context.Positions.Where(p => p.Id == oldPositionId)
        //                                               .Select(p => p.Title)
        //                                               .FirstAsync();
        //    var oldHistory = await _context.Histories.Where(h => h.EmployeeId == employeeId
        //                                                         && h.DeletedAt == null
        //                                                         && h.Description == positionName)
        //                                             .FirstAsync();
        //    oldHistory.DeletedAt = DateOnly.FromDateTime(DateTime.Now);
        //
        //    await SaveAsync();
        //}
        //
        //public async Task AddDepartmentAsync(Guid employeeId, Guid newDepartmentId)
        //{
        //    if (!await _context.Departments.AnyAsync(p => p.Id == newDepartmentId))
        //    {
        //        throw new ArgumentException($"There is no department with ID: {newDepartmentId}");
        //    }
        //
        //    var employee = await _context.Employees.Include(e => e.Departments).FirstAsync(e => e.Id == employeeId);
        //
        //    if (employee == null)
        //    {
        //        throw new ArgumentException($"There is no employee with ID: {employeeId}");
        //    }
        //
        //    employee.Departments.Add(new EmployeeDepartmentLink { EmployeeId = employeeId, DepartmentId = newDepartmentId });
        //
        //    var departmentName = await _context.Departments.Where(p => p.Id == newDepartmentId)
        //                                                   .Select(p => p.Title)
        //                                                   .FirstAsync();
        //    var history = new History(Guid.NewGuid(),
        //                              DateOnly.FromDateTime(DateTime.Now),
        //                              HistoryType.Department,
        //                              employeeId,
        //                              departmentName);
        //    await _context.Histories.AddAsync(history);
        //
        //    await SaveAsync();
        //}
        //
        //public async Task RemoveDepartmentAsync(Guid employeeId, Guid oldDepartmentId)
        //{
        //    if (!await _context.Departments.AnyAsync(p => p.Id == oldDepartmentId))
        //    {
        //        throw new ArgumentException($"There is no department with ID: {oldDepartmentId}");
        //    }
        //
        //    var employee = await _context.Employees.Include(e => e.Departments).FirstAsync(e => e.Id == employeeId);
        //
        //    if (employee == null)
        //    {
        //        throw new ArgumentException($"There is no employee with ID: {employeeId}");
        //    }
        //
        //    var itemToRemove = employee.Departments.Single(p => p.EmployeeId == employeeId && p.DepartmentId == oldDepartmentId);
        //    employee.Departments.Remove(itemToRemove);
        //
        //    var departmentName = await _context.Departments.Where(p => p.Id == oldDepartmentId)
        //                                                   .Select(p => p.Title)
        //                                                   .FirstAsync();
        //    var oldHistory = await _context.Histories.Where(h => h.EmployeeId == employeeId
        //                                                         && h.DeletedAt == null
        //                                                         && h.Description == departmentName)
        //                                             .FirstAsync();
        //    oldHistory.DeletedAt = DateOnly.FromDateTime(DateTime.Now);
        //
        //    await SaveAsync();
        //}

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
