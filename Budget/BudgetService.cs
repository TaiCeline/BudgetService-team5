using System;
using System.Linq;
using NSubstitute;

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
                var overlappingStart = start;
                var overlappingEnd = end;
                
                var isSameDay = start.ToString("yyyyMM") == end.ToString("yyyyMM");
                if (isSameDay)
                {
                    if (budget.YearMonth == start.ToString("yyyyMM"))
                    {
                        overlappingStart =  start;
                        overlappingEnd   =  end;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (budget.YearMonth == start.ToString("yyyyMM"))
                    {
                        overlappingStart =  start;
                        overlappingEnd   =  budget.LastDay();
                    }
                    else if (budget.YearMonth == end.ToString("yyyyMM"))
                    {
                        overlappingStart =  budget.FirstDay();
                        overlappingEnd   =  end;
                    }
                    else if (budget.FirstDay() >= start && budget.FirstDay() <= end) //前空白區間
                    {
                        overlappingStart =  budget.FirstDay();
                        overlappingEnd   =  budget.LastDay();
                    }
                    else
                    {
                        continue;
                    }
                }

                var overlappingDays = ((overlappingEnd - overlappingStart).Days + 1);
                totalBudget += budget.DailyAmount() * overlappingDays;
            }

            return totalBudget;
        }
    }
}