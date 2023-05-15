using AutoMapper;
using FlyingCars.DTO;
using FlyingCars.Models;
using FlyingCars.Repositories;

namespace FlyingCars.Services
{
    public class DepartmentService
    {
        private readonly IMapper _mapper;
        private readonly DepartmentRepository _repository;

        public DepartmentService(IMapper mapper, DepartmentRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        // create
        public async Task AddAsync(Department department)
        {
            try
            {
                department.Id = Guid.NewGuid();
                await _repository.AddAsync(department);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // read
        public async Task<ICollection<DepartmentDto>> GetAllAsync()
        {
            try
            {
                var departments = await _repository.GetAllAsync();
                return _mapper.Map<ICollection<Department>, ICollection<DepartmentDto>>(departments);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        // update
        public async Task UpdateAsync(Department department)
        {
            try
            {
                await _repository.UpdateAsync(department);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // delete
        public async Task DeleteByIdAsync(Guid id)
        {
            try
            {
                await _repository.DeleteByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
