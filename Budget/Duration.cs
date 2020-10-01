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

        public int OverlappingDays(Duration budgetDuration)
        {
            // 若起迄相反
            if (Start > End)
                return 0;
            
            // 若不在區間內
            if(Start > budgetDuration.End || End < budgetDuration.Start)
                return 0;
            
            // 起始點：誰大用誰, 結束點：誰小用誰
            var overlappingStart = Start > budgetDuration.Start ? Start : budgetDuration.Start;
            var overlappingEnd   = End   < budgetDuration.End ? End : budgetDuration.End;
            
            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}