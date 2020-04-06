using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tester
{
    class MeetUp
    {

        public int ScheduleDays(List<int> firstDay, List<int> lastDay)
        {
            if (firstDay.Count != lastDay.Count)
            {
                return firstDay.Distinct().Count();
            }

            var result = new List<int>();
            for (int day = 0; day < firstDay.Count; day++)
            {
                for (int index = firstDay[day]; index <= lastDay[day]; index++)
                {
                    if (!result.Contains(index))
                    {
                        result.Add(index);
                        break;
                    }
                }
            }
            return result.Count;
        }
    }

    class UnMatchingDaysException : Exception
    {

    }


}
