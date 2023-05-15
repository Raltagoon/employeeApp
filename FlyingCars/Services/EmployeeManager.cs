using FlyingCars.Models;
using FlyingCars.Repositories;

namespace FlyingCars.Services
{
    public class EmployeeManager
    {
        private readonly HistoryRepository _historyRepository;

        public EmployeeManager(HistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task Promote(EmployeeWithNavigationProperties employee, Guid newPositionId)
        {
            employee.Employee.Positions.Add(new EmployeePositionLink
            {
                EmployeeId = employee.Employee.Id,
                PositionId = newPositionId
            });
            await _historyRepository.SaveAsync();

            var newPositionName = employee.Positions.FirstOrDefault(p => p.Id == newPositionId).Title;
            await _historyRepository.AddAsync(new History(Guid.NewGuid(),
                                                          DateOnly.FromDateTime(DateTime.Now),
                                                          HistoryType.Position,
                                                          employee.Employee.Id,
                                                          newPositionName));
        }
    }
}
