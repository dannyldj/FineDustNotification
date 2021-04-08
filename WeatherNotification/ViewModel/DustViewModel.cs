using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherNotification.Manager;
using WeatherNotification.Model;
using WeatherNotification.Model.DustGrade;
using WeatherNotification.Model.DustValue;
using WeatherNotification.Model.MeasureStation;
using WeatherNotification.Properties;

namespace WeatherNotification.ViewModel
{
    public class DustViewModel : INotifyPropertyChanged
    {
        RestManager restManager = new RestManager();

        /// <summary>
        /// 측정소 이름
        /// </summary>
        private string _measureStationName;

        public string MeasureStationName
        {
            get { return _measureStationName; }
            set { _measureStationName = value; OnPropertyChanged("MeasureStationName"); }
        }

        /// <summary>
        /// 미세먼지 등급
        /// </summary>
        private string _pm10Grade;

        public string Pm10Grade
        {
            get { return _pm10Grade; }
            set { _pm10Grade = value; OnPropertyChanged("Pm10Grade"); }
        }

        /// <summary>
        /// 미세먼지 수치
        /// </summary>
        private string _pm10Value;

        public string Pm10Value
        {
            get { return _pm10Value; }
            set { _pm10Value = value; OnPropertyChanged("Pm10Value"); }
        }

        /// <summary>
        /// 미세먼지 등급 아이콘
        /// </summary>
        private string _pm10ImagePath;

        public string Pm10ImagePath
        {
            get { return _pm10ImagePath; }
            set { _pm10ImagePath = value; OnPropertyChanged("Pm10ImagePath"); }
        }

        /// <summary>
        /// 초미세먼지 등급
        /// </summary>
        private string _pm25Grade;

        public string Pm25Grade
        {
            get { return _pm25Grade; }
            set { _pm25Grade = value; OnPropertyChanged("Pm25Grade"); }
        }

        /// <summary>
        /// 초미세먼지 수치
        /// </summary>
        private string _pm25Value;

        public string Pm25Value
        {
            get { return _pm25Value; }
            set { _pm25Value = value; OnPropertyChanged("Pm25Value"); }
        }

        /// <summary>
        /// 초미세먼지 등급 아이콘
        /// </summary>
        private string _pm25ImagePath;

        public string Pm25ImagePath
        {
            get { return _pm25ImagePath; }
            set { _pm25ImagePath = value; OnPropertyChanged("Pm25ImagePath"); }
        }

        /// <summary>
        /// 수치 불러오기 커맨드
        /// </summary>
        private DelegateCommand _loadCommand;
        public DelegateCommand LoadCommand =>
            _loadCommand ?? (_loadCommand = new DelegateCommand(OnLoad));

        async void OnLoad()
        {
            List<QParamModel> qParams = new List<QParamModel>();

            // 미세먼지 등급 불러오기
            qParams.Add(new QParamModel { Key = "serviceKey", Value = string.Format(Settings.Default.dustKey) });
            qParams.Add(new QParamModel { Key = "returnType", Value = "json" });
            qParams.Add(new QParamModel { Key = "searchDate", Value = DateTime.Today.ToString("yyyy-MM-dd") });
            DustGradeModel dustGradeModel = await restManager.RestRequest<DustGradeModel>
                (Settings.Default.dustUri, qParams, "ArpltnInforInqireSvc/getMinuDustFrcstDspth");
            string dustGrade = dustGradeModel.response.body.items[0].informGrade;
            string[] dustGradeList = dustGrade.Split(',');
            foreach (string str in dustGradeList)
            {
                if (Regex.IsMatch(str, "대구"))
                {
                    Pm10Grade = str.Split(':')[1].Replace(" ", "");
                }
            }
            foreach (Model.DustGrade.Item item in dustGradeModel.response.body.items)
            {
                if (item.informCode == "PM25")
                {
                    dustGrade = item.informGrade;
                    dustGradeList = dustGrade.Split(',');
                    foreach (string str in dustGradeList)
                    {
                        if (Regex.IsMatch(str, "대구"))
                        {
                            Pm25Grade = str.Split(':')[1].Replace(" ", "");
                        }
                    }
                }
            }

            // 미세먼지 등급 아이콘 설정
            if (Pm10Grade == "나쁨")
            {
                Pm10ImagePath = "./Source/bad.png";
            }
            else if (Pm10Grade == "보통")
            {
                Pm10ImagePath = "./Source/neutral.png";
            }
            else
            {
                Pm10ImagePath = "./Source/good.png";
            }

            if (Pm25Grade == "나쁨")
            {
                Pm25ImagePath = "./Source/bad.png";
            }
            else if (Pm25Grade == "보통")
            {
                Pm25ImagePath = "./Source/neutral.png";
            }
            else
            {
                Pm25ImagePath = "./Source/good.png";
            }

            // 측정소 정보 불러오기
            qParams.Clear();
            qParams.Add(new QParamModel { Key = "serviceKey", Value = string.Format(Settings.Default.dustKey) });
            qParams.Add(new QParamModel { Key = "returnType", Value = "json" });
            qParams.Add(new QParamModel { Key = "addr", Value = "대구광역시 달성군" });
            MeasureStationModel measureStationModel = await restManager.RestRequest<MeasureStationModel>
                (Settings.Default.dustUri, qParams, "MsrstnInfoInqireSvc/getMsrstnList");
            MeasureStationName = measureStationModel.response.body.items[0].stationName;

            // 미세먼지 수치 불러오기
            qParams.Clear();
            qParams.Add(new QParamModel { Key = "serviceKey", Value = string.Format(Settings.Default.dustKey) });
            qParams.Add(new QParamModel { Key = "returnType", Value = "json" });
            qParams.Add(new QParamModel { Key = "inqBginDt", Value = DateTime.Today.AddDays(-1).ToString("yyyyMMdd") });
            qParams.Add(new QParamModel { Key = "inqEndDt", Value = DateTime.Today.AddDays(-1).ToString("yyyyMMdd") });
            qParams.Add(new QParamModel { Key = "msrstnName", Value = MeasureStationName });
            DustValueModel dustValueModel = await restManager.RestRequest<DustValueModel>
                (Settings.Default.dustUri, qParams, "ArpltnStatsSvc/getMsrstnAcctoRDyrg");
            Pm10Value = dustValueModel.response.body.items[0].pm10Value + "㎍/㎥";
            Pm25Value = dustValueModel.response.body.items[0].pm25Value + "㎍/㎥";
        }

        public DustViewModel()
        {
            //IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (IPAddress ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        localIp = ip.ToString();
            //    }
            //}

            OnLoad();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
