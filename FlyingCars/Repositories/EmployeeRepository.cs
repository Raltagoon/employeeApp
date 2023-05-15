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
        public async Task<ICollection<Employee>> GetAllByPositiodIdAsync(Guid id)
        {
            var employees = await _context.Employees.Where(e => e.Positions.Any(p => p.PositionId == id)).ToListAsync();
            if (!employees.Any())
                throw new ArgumentException("there are no employees with this position");
            return employees;
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
