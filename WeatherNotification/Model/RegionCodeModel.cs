using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherNotification.Model
{
    public class RegionCodeModel
    {
        public string RegionName(int code)
        {
            switch (code)
            {
                case 11:
                    return "서울";
                case 26:
                    return "부산";
                case 27:
                    return "대구";
                case 28:
                    return "인천";
                case 29:
                    return "광주";
                case 30:
                    return "대전";
                case 31:
                    return "울산";
                case 50:
                    return "세종";
                case 41:
                    return "경기";
                case 42:
                    return "강원";
                case 43:
                    return "충북";
                case 44:
                    return "충남";
                case 45:
                    return "전북";
                case 46:
                    return "전남";
                case 47:
                    return "경북";
                case 48:
                    return "경남";
                case 49:
                    return "제주";
                default:
                    return null;
            }
        }
    }
}
