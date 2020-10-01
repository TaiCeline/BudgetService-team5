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

            var duration    = new Duration(start, end);
            var totalBudget = 0;
            foreach (var budget in budgets)
            {
                totalBudget += budget.OverlappingAmount(duration);
            }

            return totalBudget;
        }
    }
}