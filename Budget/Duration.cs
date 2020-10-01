using System;

namespace Budget
{
    public class Duration
    {
        public Duration(DateTime start , DateTime end)
        {
            Start = start;
            End   = end;
        }

        public DateTime Start { get; set; }
        public DateTime End   { get; set; }

        public int OverlappingDays(Budget budget)
        {
            // 起始點：誰大用誰, 結束點：誰小用誰
            var overlappingStart = Start > budget.FirstDay() ? Start : budget.FirstDay();
            var overlappingEnd = End < budget.LastDay() ? End : budget.LastDay();
            
            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}