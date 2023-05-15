using FlyingCars.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace FlyingCars.Repositories
{
    public class HistoryRepository
    {
        private readonly CarContext _context;

        public HistoryRepository(CarContext context)
        {
            _context = context;
        }

        // create
        public async Task AddAsync(History history)
        {
            await _context.AddAsync(history);
            await SaveAsync();
        }

        public async Task AddRangeAsync(List<History> histories)
        {
            await _context.Histories.AddRangeAsync(histories);
            await SaveAsync();
        }

        // read
        public async Task<ICollection<History>> GetAllAsync()
        {
            var histories = await _context.Histories.ToListAsync();
            if (!histories.Any())
                throw new ArgumentException("there are no histories");
            return histories;
        }

        // other
        public async Task SetEndDateAsync(Guid id)
        {
            var histories = await _context.Histories.Where(h => h.EmployeeId == id && h.DeletedAt == null)
                                                    .ToListAsync();
            foreach (var history in histories)
            {
                history.DeletedAt = DateOnly.FromDateTime(DateTime.Now);
            }

            await SaveAsync();
        }

        public async Task SetEndDateAsync(Guid employeeId, string positionName)
        {
            var histories = await _context.Histories.Where(h => h.EmployeeId == employeeId && h.Description == positionName && h.DeletedAt == null).ToListAsync();

            foreach (var history in histories)
            {
                history.DeletedAt = DateOnly.FromDateTime(DateTime.Now);
            }

            await SaveAsync();
        }

        // service
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
