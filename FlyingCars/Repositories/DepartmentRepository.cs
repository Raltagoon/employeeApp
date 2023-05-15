using FlyingCars.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyingCars.Repositories
{
    public class DepartmentRepository
    {
        private readonly CarContext _context;

        public DepartmentRepository(CarContext context)
        {
            _context = context;
        }

        // create
        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
            await SaveAsync();
        }

        // read
        public async Task<ICollection<Department>> GetAllAsync()
        {
            var departments = await _context.Departments.ToListAsync();
            if (!departments.Any())
                throw new ArgumentException("there are no departments");
            return departments;
        }

        public async Task<string> GetTitleByIdAsync(Guid id)
        {
            var title = await _context.Departments.Where(d => d.Id == id)
                                                  .Select(d => d.Title)
                                                  .FirstOrDefaultAsync();
            if (title == null)
                throw new ArgumentException("there is no department with this id");
            return title;
        }

        // update
        public async Task UpdateAsync(Department department)
        {
            _context.Update(department);
            await SaveAsync();
        }

        // delete
        public async Task DeleteByIdAsync(Guid id)
        {
            var tmp = await _context.Departments.FindAsync(id);
            if (tmp == null)
            {
                throw new ArgumentException($"There is no department with ID {id}");
            }
            _context.Departments.Remove(tmp);
            await SaveAsync();
        }

        // service
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
