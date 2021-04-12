using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeatherNotification.Manager;
using WeatherNotification.Model;
using WeatherNotification.Properties;

namespace WeatherNotification.ViewModel
{
    public class DustViewModel : INotifyPropertyChanged
    {
        RestManager restManager = new RestManager();

        #region props

        /// <summary>
        /// 지역명
        /// </summary>
        private string _regionName;

        public string RegionName
        {
            get { return _regionName; }
            set { _regionName = value; OnPropertyChanged("RegionName"); }
        }

        /// <summary>
        /// 측정소 이름
        /// </summary>
        private string _measureStation;

        public string MeasureStation
        {
            get { return _measureStation; }
            set { _measureStation = value; OnPropertyChanged("MeasureStation"); }
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
        /// 측정 시각
        /// </summary>
        private string _measureTime;

        public string MeasureTime
        {
            get { return _measureTime; }
            set { _measureTime = value; OnPropertyChanged("MeasureTime"); }
        }

        /// <summary>
        /// 수치 불러오기 커맨드
        /// </summary>
        private DelegateCommand _loadCommand;
        public DelegateCommand LoadCommand =>
            _loadCommand ?? (_loadCommand = new DelegateCommand(OnLoad));

        #endregion

        public DustViewModel()
        {
            // IP 기반 위치 정보
            var request = WebRequest.Create(Settings.Default.ipapiUri + "region_code");
            var response = new System.IO.StreamReader(request.GetResponse().GetResponseStream());
            RegionName = RegionCodeModel.RegionName(int.Parse(response.ReadToEnd().ToString()));

            OnLoad();
        }

        async void OnLoad()
        {
            List<QParamModel> qParams = new List<QParamModel>();
            qParams.Add(new QParamModel { Key = "serviceKey", Value = Settings.Default.dustKey });
            qParams.Add(new QParamModel { Key = "returnType", Value = "json" });
            qParams.Add(new QParamModel { Key = "sidoName", Value = RegionName });
            qParams.Add(new QParamModel { Key = "ver", Value = "1.3" });
            DustInfoModel dustInfoModel = await restManager.RestRequest<DustInfoModel>
                (Settings.Default.dustUri, qParams, "ArpltnInforInqireSvc/getCtprvnRltmMesureDnsty");
            var dustInfo = dustInfoModel.response.body.items[0];

            MeasureStation = dustInfo.stationName;

            int pm10Grade = int.Parse(dustInfo.pm10Grade1h);
            int pm25Grade = int.Parse(dustInfo.pm25Grade1h);
            SetIcon(pm10Grade, grade => Pm10Grade = grade, value => Pm10ImagePath = value);
            SetIcon(pm25Grade, grade => Pm25Grade = grade, value => Pm25ImagePath = value);

            Pm10Value = dustInfo.pm10Value + "㎍/㎥";
            Pm25Value = dustInfo.pm25Value + "㎍/㎥";

            MeasureTime = dustInfo.dataTime;
        }

        void SetIcon(int grade, Action<string> dustGrade, Action<string> imagePath)
        {
            switch (grade)
            {
                case 1:
                    dustGrade("좋음");
                    imagePath("./Source/good.png");
                    break;
                case 2:
                    dustGrade("보통");
                    imagePath("./Source/neutral.png");
                    break;
                case 3:
                    dustGrade("나쁨");
                    imagePath("./Source/bad.png");
                    break;
                case 4:
                    dustGrade("매우 나쁨");
                    imagePath("./Source/worse.png");
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
