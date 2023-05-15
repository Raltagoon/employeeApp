using FlyingCars.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyingCars.Repositories
{
    public class PositionRepository
    {
        private readonly CarContext _context;

        public PositionRepository(CarContext context)
        {
            _context = context;
        }

        // create
        public async Task AddAsync(Position position)
        {
            await _context.Positions.AddAsync(position);
            await SaveAsync();
        }

        // read
        public async Task<ICollection<Position>> GetAllAsync()
        {
            var positions = await _context.Positions.ToListAsync();
            if (!positions.Any())
                throw new ArgumentException("there are no positions");
            return positions;
        }

        public async Task<string> GetTitleByIdAsync(Guid id)
        {
            var title = await _context.Positions.Where(p => p.Id == id)
                                                .Select(p => p.Title)
                                                .FirstOrDefaultAsync();
            if (title == null)
                throw new ArgumentException("there is no position with this id");
            return title;
        }

        // update
        public async Task UpdateAsync(Position position)
        {
            _context.Update(position);
            await SaveAsync();
        }

        // delete
        public async Task DeleteByIdAsync(Guid id)
        {
            var tmp = await _context.Positions.FindAsync(id);
            if (tmp == null)
            {
                throw new ArgumentException($"There is no posiition with ID {id}");
            }
            _context.Positions.Remove(tmp);
            await SaveAsync();
        }

        // service
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
