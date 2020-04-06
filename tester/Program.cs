using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CourseLibrary.API.Services;

namespace tester
{
    class Program
    {
        static void Main(string[] args)
        {

            //var sule = api.txnSummary(1, "debit");
            //for (int i = 0; i < sule.Count; i++)
            //{
            //    Console.WriteLine($"{sule[i][0]},{sule[i][1]}");
            //}
            MeetUp meet = new MeetUp();
            var firstDay = new List<int> { 1, 2, 3, 3, 3 };
            var lastDay = new List<int> { 2, 2, 3, 4, 4 };
            var result = meet.ScheduleDays(firstDay, lastDay);
            Console.WriteLine($"The Result Is ::: {result}");
            Console.ReadKey();
        }
    }
}
