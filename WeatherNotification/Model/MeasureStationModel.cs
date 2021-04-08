using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherNotification.Model.MeasureStation
{
    public class Item
    {
        public string dmX { get; set; }
        public string item { get; set; }
        public string mangName { get; set; }
        public string year { get; set; }
        public string addr { get; set; }
        public string stationName { get; set; }
        public string dmY { get; set; }
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

    public class MeasureStationModel
    {
        public Response response { get; set; }
    }
}
