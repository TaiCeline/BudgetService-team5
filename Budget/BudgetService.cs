using System;
using System.Linq;
using NSubstitute;

namespace Budget
{
    public class InClassName
    {
        public InClassName(DateTime start , int daysInMonth)
        {
            Start       = start;
            DaysInMonth = daysInMonth;
        }

        public DateTime Start       { get; private set; }
        public int      DaysInMonth { get; private set; }
    }

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
                var isSameDay = start.ToString("yyyyMM") == end.ToString("yyyyMM");
                if (isSameDay)
                {
                    if (budget.YearMonth == start.ToString("yyyyMM"))
                    {
                        totalBudget += budget.DailyAmount() * ((end - start).Days + 1);
                    }
                }
                else
                {
                    if (budget.YearMonth == start.ToString("yyyyMM"))
                    {
                        totalBudget += budget.DailyAmount() * ((budget.LastDays() - start).Days + 1);
                    }
                    else if (budget.YearMonth == end.ToString("yyyyMM"))
                    {
                        totalBudget += budget.DailyAmount() * ((end - budget.FirstDay()).Days + 1);
                    }
                    else if (budget.FirstDay() >= start && budget.FirstDay() <= end)
                    {
                        totalBudget += budget.Amount;
                    }
                }
            }

            return totalBudget;
        }
    }
}