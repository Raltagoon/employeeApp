using AutoMapper;
using FlyingCars.DTO;
using FlyingCars.Models;
using FlyingCars.Repositories;

namespace FlyingCars.Services
{
    public class PositionService
    {
        private readonly IMapper _mapper;
        private readonly PositionRepository _repository;

        public PositionService(IMapper mapper, PositionRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        // create
        public async Task AddAsync(Position position)
        {
            position.Id = Guid.NewGuid();
            await _repository.AddAsync(position);
        }

        // read
        public async Task<ICollection<PositionDto>> GetAllAsync()
        {
            var positions = await _repository.GetAllAsync();
            return _mapper.Map<ICollection<Position>, ICollection<PositionDto>>(positions);
        }

        // update
        public async Task UpdateAsync(Position position)
        {
            await _repository.UpdateAsync(position);
        }

        // delete
        public async Task DeleteByIdAsync(Guid id)
        {
            await _repository.DeleteByIdAsync(id);
        }
    }
}
