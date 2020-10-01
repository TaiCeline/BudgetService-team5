using System;
using System.Linq;

namespace Budget
{
    public class BudgetService
    {
        private readonly IBudgetRepo _repo;

        public BudgetService(IBudgetRepo repo)
        {
            _repo = repo;
        }

        public double Query(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return 0;
            }

            var budgets = _repo.GetAll();
            if (!budgets.Any())
            {
                return 0;
            }

            var totalBudget = 0;
            foreach (var budget in budgets)
            {
                if(start > budget.LastDay() || end < budget.FirstDay()) //若不在區間內
                    continue;
                
                totalBudget += budget.DailyAmount() * new Duration(start, end).OverlappingDays(budget);
            }

            return totalBudget;
        }
    }
}