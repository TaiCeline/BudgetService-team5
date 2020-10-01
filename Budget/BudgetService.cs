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
            var duration = new Duration(start, end);
            return _repo.GetAll()
                        .Sum(budget => budget.OverlappingAmount(duration));
        }
    }
}