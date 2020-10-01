
using System;
using System.Globalization;

namespace Budget
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int    Amount    { get; set; }
        
        public DateTime FirstDay()
        {
            var dateTime = DateTime.ParseExact($"{YearMonth}", "yyyyMM", CultureInfo.InvariantCulture);
            return dateTime;
        }

        public int Days()
        {
            var daysInMonth = DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
            return daysInMonth;
        }

        public int DailyAmount()
        {
            return Amount / Days();
        }

        public DateTime LastDay()
        {
            return new DateTime(FirstDay().Year, FirstDay().Month, Days());
        }

        public int OverlappingAmount(Duration duration)
        {
            return DailyAmount() * duration.OverlappingDays(CreateBudgetDuration());
        }

        public Duration CreateBudgetDuration()
        {
            return new Duration(FirstDay(), LastDay());
        }
    }
}