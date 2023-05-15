using AutoMapper;
using FlyingCars.DTO;
using FlyingCars.Models;
using FlyingCars.Repositories;

namespace FlyingCars.Services
{
    public class EmployeeService
    {
        private readonly IMapper _mapper;
        private readonly EmployeeRepository _repository;
        private readonly EmployeeManager _manager;
        private readonly HistoryRepository _historyRepository;
        private readonly PositionRepository _positionRepository;
        private readonly DepartmentRepository _departmentRepository;

        public EmployeeService(IMapper mapper,
                               EmployeeRepository repository,
                               HistoryRepository historyRepository,
                               PositionRepository positionRepository,
                               DepartmentRepository departmentRepository,
                               EmployeeManager manager)
        {
            _mapper = mapper;
            _repository = repository;
            _historyRepository = historyRepository;
            _positionRepository = positionRepository;
            _departmentRepository = departmentRepository;
            _manager = manager;
        }

        // create
        public async Task AddAsync(Employee employee)
        {
            try
            {
                employee.Id = Guid.NewGuid();
                foreach (var document in employee.Documents)
                {
                    document.Id = Guid.NewGuid();
                }
                await _repository.AddAsync(employee);

                var histories = new List<History>();

                foreach (var position in employee.Positions)
                {
                    var positionName = await _positionRepository.GetTitleByIdAsync(position.PositionId);

                    histories.Add(new History(Guid.NewGuid(),
                                              DateOnly.FromDateTime(DateTime.Now),
                                              HistoryType.Position,
                                              employee.Id,
                                              positionName));
                }

                foreach (var department in employee.Departments)
                {
                    var departmentName = await _departmentRepository.GetTitleByIdAsync(department.DepartmentId);

                    histories.Add(new History(Guid.NewGuid(),
                                              DateOnly.FromDateTime(DateTime.Now),
                                              HistoryType.Department,
                                              employee.Id,
                                              departmentName));
                }

                await _historyRepository.AddRangeAsync(histories);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // read
        public async Task<ICollection<EmployeeDto>> GetAllAsync()
        {
            try
            {
                var employees = await _repository.GetAllAsync();
                return _mapper.Map<ICollection<Employee>, ICollection<EmployeeDto>>(employees);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICollection<EmployeeDto>> GetPagedList(int pageSize, int pageNumber)
        {
            try
            {
                var employees = await _repository.GetPagedList(pageSize, pageNumber);
                return _mapper.Map<ICollection<Employee>, ICollection<EmployeeDto>>(employees);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EmployeeDto> GetByIdAsync(Guid id)
        {
            try
            {
                var employee = await _repository.GetByIdAsync(id);
                return _mapper.Map<Employee, EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EmployeeWithNavigationPropertiesDto> GetByIdWithNavigationPropertiesAsync(Guid id, EmployeeNavigationProperties properties)
        {
            try
            {
                var employee = await _repository.GetByIdWithNavigationPropertiesAsync(id, properties);
                return _mapper.Map<EmployeeWithNavigationProperties, EmployeeWithNavigationPropertiesDto>(employee);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICollection<EmployeeWithNavigationPropertiesDto>> GetAllWithNavigationPropertiesAsync(EmployeeNavigationProperties properties)
        {
            try
            {
                var employees = await _repository.GetAllWithNavigationPropertiesAsync(properties);
                return _mapper.Map<ICollection<EmployeeWithNavigationProperties>, ICollection<EmployeeWithNavigationPropertiesDto>>(employees);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // update
        public async Task UpdateAsync(Employee employee)
        {
            try
            {
                await _repository.UpdateAsync(employee);
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
                await _historyRepository.SetEndDateAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       //other
       public async Task AddPositionAsync(Guid employeeId, Guid positionId)
       {
            var employee = await _repository.GetByIdWithNavigationPropertiesAsync(employeeId, EmployeeNavigationProperties.Positions);
            // не бизнесс правило, поэтому в сервисе?
            if (employee.Positions.Any(p => p.Id == positionId))
            {
                throw new ArgumentException("Employee already has this position.");
            }
            var positionName = await _positionRepository.GetTitleByIdAsync(positionId);

            await _manager.AddPositionAsync(employee, positionId, positionName);
            await _repository.SaveAsync();
       }

        public async Task RemovePositionAsync(Guid employeeId, Guid positionId)
        {
            var employee = await _repository.GetByIdWithNavigationPropertiesAsync(employeeId, EmployeeNavigationProperties.Positions);
            // не бизнесс правило, поэтому в сервисе?
            if (!employee.Positions.Any(p => p.Id == positionId))
            {
                throw new ArgumentException("Employee doesnt have this position.");
            }
            var positionName = await _positionRepository.GetTitleByIdAsync(positionId);

            await _manager.RemovePositionAsync(employee, positionId, positionName);
            await _repository.SaveAsync();
        }



        // other
        //public async Task<ICollection<EmployeeDto>> GetByPositionAsync(string position)
        //{
        //    var employees = await _repository.GetByPositionAsync(position);
        //    return _mapper.Map<ICollection<Employee>, ICollection<EmployeeDto>>(employees);
        //}
        //
        //public async Task<ICollection<EmployeeDto>> GetByDepartmentAsync(string department)
        //{
        //    var employees = await _repository.GetByDepartmentAsync(department);
        //    return _mapper.Map<ICollection<Employee>, ICollection<EmployeeDto>>(employees);
        //}
        //
        //public async Task AddPositionAsync(Guid employeeId, Guid positionId)
        //{
        //    await _repository.AddPositionAsync(employeeId, positionId);
        //}
        //
        //public async Task RemovePositionAsync(Guid edeployeeId, Guid oldPositionId)
        //{
        //    await _repository.RemovePositionAsync(edeployeeId, oldPositionId);
        //}
        //
        //public async Task AddDepartmentAsync(Guid employeeId, Guid newDepartmentId)
        //{
        //    await _repository.AddDepartmentAsync(employeeId, newDepartmentId);
        //}
        //
        //public async Task RemoveDepartmentAsync(Guid employeeId, Guid oldDepartmentId)
        //{
        //    await _repository.RemoveDepartmentAsync(employeeId, oldDepartmentId);
        //}
    }
}
