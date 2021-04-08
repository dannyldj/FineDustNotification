using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherNotification.Model.DustValue
{
    public class Item
    {
        public string msurDt { get; set; }
        public string so2Value { get; set; }
        public string coValue { get; set; }
        public string msrstnName { get; set; }
        public string pm10Value { get; set; }
        public string no2Value { get; set; }
        public string o3Value { get; set; }
        public string pm25Value { get; set; }
    }

    public class Body
    {
        public int totalCount { get; set; }
        public List<Item> items { get; set; }
        public int pageNo { get; set; }
        public int numOfRows { get; set; }
    }

    public class Header
    {
        public string resultMsg { get; set; }
        public string resultCode { get; set; }
    }

    public class Response
    {
        public Body body { get; set; }
        public Header header { get; set; }
    }

    public class DustValueModel
    {
        public Response response { get; set; }
    }
}
