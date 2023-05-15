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

        public async Task AddPositionAsync(EmployeeWithNavigationProperties employee, Guid positionId, string positionName)
        {
            // абстрактное бизнес-правило, чтобы оправдать использование Manager
            if (employee.Employee.Positions.Count < 3)
            {
                employee.Employee.Positions.Add(new EmployeePositionLink
                {
                    EmployeeId = employee.Employee.Id,
                    PositionId = positionId
                });

                await _historyRepository.AddAsync(new History(Guid.NewGuid(),
                                                              DateOnly.FromDateTime(DateTime.Now),
                                                              HistoryType.Position,
                                                              employee.Employee.Id,
                                                              positionName));
            }
            else
            {
                throw new ArgumentException("Employee already have 3 positions.");
            }
        }

        public async Task RemovePositionAsync(EmployeeWithNavigationProperties employee, Guid positionId, string positionName)
        {
            // абстрактное бизнес-правило, чтобы оправдать использование Manager
            if (employee.Employee.Positions.Count > 1)
            {
                var positionToRemove = employee.Employee.Positions.Single(p => p.PositionId == positionId && p.EmployeeId == employee.Employee.Id);
                employee.Employee.Positions.Remove(positionToRemove);

                await _historyRepository.SetEndDateAsync(employee.Employee.Id, positionName);
            }
            else
            {
                throw new ArgumentException("Employee must have at least 1 position");
            }
        }
    }
}
