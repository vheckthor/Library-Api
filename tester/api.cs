using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using tester;

namespace CourseLibrary.API.Services
{
    class creator
    {
        public int userId { get; set; }
        public int amount { get; set; }
    }
    public class api
    {
        public  static List<List<int>> txnSummary(int locationId, string transactionType)
        {
            var lister = new List<List<int>>();
            var listOfObject=new List<RootObject>();
            int length = 0;
            string address =
                $"https://jsonmock.hackerrank.com/api/transactions/search?txnType={transactionType}&page={0}";

            using (HttpClient client = new HttpClient())
            {
                var result = client.GetAsync(address).Result;
                var sam = result.Content.ReadAsStringAsync().Result;

                var lime = JsonConvert.DeserializeObject<RootObject>(sam);

                length = lime.total_pages;
                Console.WriteLine(sam);


            }


            var empty = new List<List<int>> { new List<int> { -1, -1 } };

            if (length <= 0)
            {
                return empty;
            }

            for (int i = 1; i <= length; i++)
            {
                string adder =
                    $"https://jsonmock.hackerrank.com/api/transactions/search?txnType={transactionType}&page={i}";

                using (HttpClient client = new HttpClient())
                {
                    var result = client.GetAsync(adder).Result;
                    var sam = result.Content.ReadAsStringAsync().Result;

                    var lime = JsonConvert.DeserializeObject<RootObject>(sam);

                    listOfObject.Add(lime);

                }
            }

            var mol = from s in listOfObject
                from y in s.data
                where (y.location.id == locationId)
                select y;

            var emp = new List<List<int>> { new List<int> { -1, -1 } };

            if (mol.Count() <= 0)
            {
                return emp;
            }

            var  newvalus =
                  // from s in mol
                from v in mol
                group v.amount by v.userId
                into g
                select new {userId = g.Key, amount = g.ToList()};
            

            foreach (var value in newvalus)
            {

                        var som = value.amount.Select(x=>  x.Remove(0, 1)).Select(x=> String.Join("",x.Split(',')));
                        

                   

                    creator val = new creator
                    {
                        userId = value.userId,
                        amount = (int)som.Select(x=> double.Parse(x)).ToList().Sum()
                    }; 
                    var small = new List<int>{ val.userId , val.amount };
                    lister.Add(small);
            }
            var lang = lister.OrderBy(x=>x[0]).ToList();

            return lang;
        }
    }
}