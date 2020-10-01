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
                
                totalBudget += budget.DailyAmount() * OverlappingDays(start , end , budget);
            }

            return totalBudget;
        }

        private static int OverlappingDays(DateTime start , DateTime end , Budget budget)
        {
            var overlappingStart = start;
            var overlappingEnd   = end;

            var isSameDay = start.ToString("yyyyMM") == end.ToString("yyyyMM");
            if (isSameDay)
            {
                if (budget.YearMonth == start.ToString("yyyyMM"))
                {
                    overlappingStart = start;
                    overlappingEnd   = end;
                }
            }
            else
            {
                if (budget.YearMonth == start.ToString("yyyyMM")) //開始查點~Budget最後一天
                {
                    overlappingStart = start;
                    overlappingEnd   = budget.LastDay();
                }
                else if (budget.YearMonth == end.ToString("yyyyMM")) //Budget第一天~結束查點
                {
                    overlappingStart = budget.FirstDay();
                    overlappingEnd   = end;
                }
                else if (budget.FirstDay() >= start && budget.FirstDay() <= end) //跨兩個月區間
                {
                    overlappingStart = budget.FirstDay();
                    overlappingEnd   = budget.LastDay();
                }
            }

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}