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
                
                totalBudget += budget.DailyAmount() * OverlappingDays(new Duration(start, end), budget);
            }

            return totalBudget;
        }

        private static int OverlappingDays(Duration duration , Budget budget)
        {
            var overlappingStart = duration.Start;
            var overlappingEnd   = duration.End;

            var isSameDay = duration.Start.ToString("yyyyMM") == duration.End.ToString("yyyyMM");
            if (isSameDay)
            {
                if (budget.YearMonth == duration.Start.ToString("yyyyMM"))
                {
                    overlappingStart = duration.Start;
                    overlappingEnd   = duration.End;
                }
            }
            else
            {
                if (budget.YearMonth == duration.Start.ToString("yyyyMM")) //開始查點~Budget最後一天
                {
                    overlappingStart = duration.Start;
                    overlappingEnd   = budget.LastDay();
                }
                else if (budget.YearMonth == duration.End.ToString("yyyyMM")) //Budget第一天~結束查點
                {
                    overlappingStart = budget.FirstDay();
                    overlappingEnd   = duration.End;
                }
                else if (budget.FirstDay() >= duration.Start && budget.FirstDay() <= duration.End) //跨兩個月區間
                {
                    overlappingStart = budget.FirstDay();
                    overlappingEnd   = budget.LastDay();
                }
            }

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }

    public class Duration
    {
        public Duration(DateTime start , DateTime end)
        {
            Start = start;
            End   = end;
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

    }
}