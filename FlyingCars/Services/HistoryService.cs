using AutoMapper;
using FlyingCars.DTO;
using FlyingCars.Models;
using FlyingCars.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlyingCars.Services
{
    public class HistoryService
    {
        private readonly IMapper _mapper;
        private readonly HistoryRepository _repository;


        public HistoryService(IMapper mapper,
                              HistoryRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        // read
        public async Task<ICollection<HistoryDto>> GetAllAsync()
        {
            var histories = await _repository.GetAllAsync();
            return _mapper.Map<ICollection<History>, ICollection<HistoryDto>>(histories);
        }
    }
}
