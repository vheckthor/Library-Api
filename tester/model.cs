using System.Collections.Generic;

namespace tester
{
public class Location
{
    public int id { get; set; }
    public string address { get; set; }
    public string city { get; set; }
    public int zipCode { get; set; }
}

public class Datum
{
    public int id { get; set; }
    public int userId { get; set; }
    public string userName { get; set; }
    public long timestamp { get; set; }
    public string txnType { get; set; }
    public string amount { get; set; }
    public Location location { get; set; }
    public string ip { get; set; }
}

public class RootObject
{
    public string page { get; set; }
    public int per_page { get; set; }
    public int total { get; set; }
    public int total_pages { get; set; }
    public List<Datum> data { get; set; }
}
}