using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherNotification.Model.DustGrade
{
    public class Item
    {
        public string imageUrl4 { get; set; }
        public string informCode { get; set; }
        public string imageUrl5 { get; set; }
        public string imageUrl6 { get; set; }
        public object actionKnack { get; set; }
        public string informCause { get; set; }
        public string informOverall { get; set; }
        public string informData { get; set; }
        public string informGrade { get; set; }
        public string dataTime { get; set; }
        public string imageUrl3 { get; set; }
        public string imageUrl2 { get; set; }
        public string imageUrl1 { get; set; }
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

    public class DustGradeModel
    {
        public Response response { get; set; }
    }
}
