
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherPred
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Random r = new Random();
        static string jsonFull = "{\"days\":[";
        static int windowCount =1;
        static int locationIndex;
        public static string locationTitle;
        public static List<double> listMaxTemp = new List<double>();
        public static List<double> listMinTemp = new List<double>();
        Dictionary<string, Field> fieldsDic = new Dictionary<string, Field>();
        Dictionary<string, double> dayWeather = new Dictionary<string, double>();
      
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
            RandomValues();
            FillValues(dayWeather);
        }
        public MainWindow(Dictionary<string, double> day)
        {
            dayWeather = day;
            InitializeComponent();
            Initialize();
            FillValues(day);
        }
        void Initialize()
        {
            ComboBoxFill();

            List<Field> fields = Read.ReadFields();
            foreach (var f in fields)
            {
                fieldsDic.Add(f.Name, f);
            }
            buttonChart.Visibility = Visibility.Visible;
            buttonPred.Visibility = Visibility.Hidden;

            if (windowCount > 1)
            {
                comboBoxLoc.IsEnabled = false;
            }

            if (windowCount == 4)
            {


                buttonPred.Visibility = Visibility.Visible;
                buttonNext.Visibility = Visibility.Hidden;
            }
        }
        void RandomValues()
        {
            dayWeather.Clear();
            foreach(var f in fieldsDic.Keys)
            {
                if (fieldsDic[f].Name != "Location" && fieldsDic[f].Name != "RainToday")
                    dayWeather.Add(fieldsDic[f].Name, Convert.ToDouble(r.Next(Convert.ToInt32(fieldsDic[f].Min), Convert.ToInt32(fieldsDic[f].Max) - 1)) + r.NextDouble());
                if (fieldsDic[f].Name == "Location")
                {
                    dayWeather.Add(fieldsDic[f].Name, r.Next(1, 48));
                    
                }
                if (fieldsDic[f].Name == "RainToday")
                    dayWeather.Add(fieldsDic[f].Name, r.Next(0, 2));
            }
            dayWeather["MaxTemp"] = (dayWeather["MinTemp"] + r.NextDouble() * 5.0);
            dayWeather["Temp9am"] = (dayWeather["MinTemp"] + r.NextDouble() * 3.0);
            dayWeather["Temp3pm"] = (dayWeather["MaxTemp"] - r.NextDouble() * 5.0);
        }

        void FillValues(Dictionary<string, double> day)
        {
            Dictionary<string, double> dayWeather = new Dictionary<string, double>();

            foreach (var f in fieldsDic.Keys)
            {
                if (fieldsDic[f].Name != "Location" && fieldsDic[f].Name != "RainToday")
                {
                    int sign = r.Next(0, 2);
                    
                    dayWeather.Add(fieldsDic[f].Name,day[f]+ r.NextDouble() * 3.0 * (sign == 0 ? -1.0 : 1.0));
                }
                if (fieldsDic[f].Name == "Location")
                {
                    dayWeather.Add("Location", day[f]);

                }
                if (fieldsDic[f].Name == "RainToday")
                    dayWeather.Add(fieldsDic[f].Name, r.Next(0, 2));
            }

            if (windowCount == 1)
                comboBoxLoc.SelectedIndex = (int)dayWeather["Location"];
            else
                comboBoxLoc.SelectedIndex = locationIndex;

            textBoxMinTemp.Text = dayWeather["MinTemp"].ToString("F2");
            textBoxMaxTemp.Text = dayWeather["MaxTemp"].ToString("F2");
            textBoxRainfall.Text = dayWeather["Rainfall"].ToString("F2");
            textBoxEvaporation.Text = dayWeather["Evaporation"].ToString("F2");
            textBoxSunshine.Text = dayWeather["Sunshine"].ToString("F2");
            textBoxWindGustDir.Text = dayWeather["WindGustDir"].ToString("F2");
            textBoxWindGustSpeed.Text = dayWeather["WindGustSpeed"].ToString("F2");
            textBoxWindDir9am.Text = dayWeather["WindDir9am"].ToString("F2");
            textBoxWindDir3pm.Text = dayWeather["WindDir3pm"].ToString("F2");
            textBoxWindSpeed9am.Text = dayWeather["WindSpeed9am"].ToString("F2");
            textBoxWindSpeed3pm.Text = dayWeather["WindSpeed3pm"].ToString("F2");
            textBoxHumidity9am.Text = dayWeather["Humidity9am"].ToString("F2");
            textBoxHumidity3pm.Text = dayWeather["Humidity3pm"].ToString("F2");
            textBoxPressure9am.Text = dayWeather["Pressure9am"].ToString("F2");
            textBoxPressure3pm.Text = dayWeather["Pressure3pm"].ToString("F2");
            textBoxCloud9am.Text = dayWeather["Cloud9am"].ToString("F2");
            textBoxCloud3pm.Text = dayWeather["Cloud3pm"].ToString("F2");
            textBoxTemp9am.Text = dayWeather["Temp9am"].ToString("F2");
            textBoxTemp3pm.Text = dayWeather["Temp3pm"].ToString("F2");

            textBoxRainToday.Text = dayWeather["RainToday"].ToString("F0");

        }
        void ComboBoxFill()
        {
            string[] s = new string[] { "Albury", "BadgerysCreek", "Cobar", "CoffsHarbour", "Moree", "Newcastle", "-1rahHead", "-1rfolkIsland", "Penrith", "Richmond", "Sydney", "SydneyAirport", "WaggaWagga", "Williamtown", "Wollongong", "Canberra", "Tuggera-1ng", "MountGinini", "Ballarat", "Bendigo", "Sale", "MelbourneAirport", "Melbourne", "Mildura", "Nhil", "Portland", "Watsonia", "Dartmoor", "Brisbane", "Cairns", "GoldCoast", "Townsville", "Adelaide", "MountGambier", "Nuriootpa", "Woomera", "Albany", "Witchcliffe", "PearceRAAF", "PerthAirport", "Perth", "SalmonGums", "Walpole", "Hobart", "Launceston", "AliceSprings", "Darwin", "Katherine", "Uluru"};
            foreach (string x in s)
                comboBoxLoc.Items.Add(x);

        }
        private void ButtonGener_Click(object sender, RoutedEventArgs e)
        {
            FillValues(dayWeather);
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            string json = "{\"Location\": \"" + comboBoxLoc.SelectedItem.ToString() + "\", \"MinTemp\": \"" + textBoxMinTemp.Text.Replace(',', '.') + "\", \"MaxTemp\": \"" + textBoxMaxTemp.Text.Replace(',', '.') + "\", \"Rainfall\": \"" + textBoxRainfall.Text.Replace(',', '.') + "\", \"Evaporation\": \"" + textBoxEvaporation.Text.Replace(',', '.') + "\", \"Sunshine\": \"" + textBoxSunshine.Text.Replace(',', '.') + "\", \"WindGustDir\": \"" + textBoxWindGustDir.Text.Replace(',', '.') + "\", \"WindGustSpeed\": \"" + textBoxWindGustSpeed.Text.Replace(',', '.') + "\", \"WindDir9am\": \"" + textBoxWindDir9am.Text.Replace(',', '.') + "\", \"WindDir3pm\": \"" + textBoxWindDir3pm.Text.Replace(',', '.') + "\", \"WindSpeed9am\": \"" + textBoxWindSpeed9am.Text.Replace(',', '.') + "\", \"WindSpeed3pm\": \"" + textBoxWindSpeed3pm.Text.Replace(',', '.') + "\", \"Humidity9am\": \"" + textBoxHumidity9am.Text.Replace(',', '.') + "\", \"Humidity3pm\": \"" + textBoxHumidity3pm.Text.Replace(',', '.') + "\", \"Pressure9am\": \"" + textBoxPressure9am.Text.Replace(',', '.') + "\", \"Pressure3pm\": \"" + textBoxPressure3pm.Text.Replace(',', '.') + "\", \"Cloud9am\": \"" + textBoxCloud9am.Text.Replace(',', '.') + "\", \"Cloud3pm\": \"" + textBoxCloud3pm.Text.Replace(',', '.') + "\", \"Temp9am\": \"" + textBoxTemp9am.Text.Replace(',', '.') + "\", \"Temp3pm\": \"" + textBoxTemp9am.Text.Replace(',', '.') + "\", \"RainToday\": \"" + textBoxRainToday.Text + "\"}";

            if (windowCount == 1)
            {
                locationIndex = comboBoxLoc.SelectedIndex;
                
            }
            if (windowCount >= 1 && windowCount <= 3)
            {
                json += ", ";
                
            }
            listMinTemp.Add(Convert.ToDouble(textBoxMinTemp.Text));
            listMaxTemp.Add(Convert.ToDouble(textBoxMaxTemp.Text));

            jsonFull += json;
            windowCount++;
            MainWindow mw = new MainWindow(dayWeather);
            mw.Show();
            this.Close();
        }

        private void ButtonPred_Click(object sender, RoutedEventArgs e)
        {
            string json = "{\"Location\": \"" + comboBoxLoc.SelectedItem.ToString() + "\", \"MinTemp\": \"" + textBoxMinTemp.Text.Replace(',', '.') + "\", \"MaxTemp\": \"" + textBoxMaxTemp.Text.Replace(',', '.') + "\", \"Rainfall\": \"" + textBoxRainfall.Text.Replace(',', '.') + "\", \"Evaporation\": \"" + textBoxEvaporation.Text.Replace(',', '.') + "\", \"Sunshine\": \"" + textBoxSunshine.Text.Replace(',', '.') + "\", \"WindGustDir\": \"" + textBoxWindGustDir.Text.Replace(',', '.') + "\", \"WindGustSpeed\": \"" + textBoxWindGustSpeed.Text.Replace(',', '.') + "\", \"WindDir9am\": \"" + textBoxWindDir9am.Text.Replace(',', '.') + "\", \"WindDir3pm\": \"" + textBoxWindDir3pm.Text.Replace(',', '.') + "\", \"WindSpeed9am\": \"" + textBoxWindSpeed9am.Text.Replace(',', '.') + "\", \"WindSpeed3pm\": \"" + textBoxWindSpeed3pm.Text.Replace(',', '.') + "\", \"Humidity9am\": \"" + textBoxHumidity9am.Text.Replace(',', '.') + "\", \"Humidity3pm\": \"" + textBoxHumidity3pm.Text.Replace(',', '.') + "\", \"Pressure9am\": \"" + textBoxPressure9am.Text.Replace(',', '.') + "\", \"Pressure3pm\": \"" + textBoxPressure3pm.Text.Replace(',', '.') + "\", \"Cloud9am\": \"" + textBoxCloud9am.Text.Replace(',', '.') + "\", \"Cloud3pm\": \"" + textBoxCloud3pm.Text.Replace(',', '.') + "\", \"Temp9am\": \"" + textBoxTemp9am.Text.Replace(',', '.') + "\", \"Temp3pm\": \"" + textBoxTemp9am.Text.Replace(',', '.') + "\", \"RainToday\": \"" + textBoxRainToday.Text + "\"}]}";

            
            // отправляем запрос на сервер
            try
            {
                // создаем запрос
                WebRequest request = WebRequest.Create("http://31.132.153.13:5000/predict");
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "POST";
                request.Timeout = 5000;
                dynamic jsonObject = new JObject();
                jsonObject = jsonFull+json;

                byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonObject));
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                // получаем ответ
                WebResponse response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                // парсим JSON строку
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseFromServer);
                if (values["success"] == "true")
                {
                    MessageBox.Show("Вероятность выпадения осадков: " + values["RainToday"] + "\n Максимальная температура: " + values["MaxTemp"] + "\n Минимальная температура: " + values["MinTemp"], "Result", MessageBoxButton.OK);
                }
                else if (values["success"] == "false")
                {
                    MessageBox.Show("Ошибка: " + values["reason"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                Console.WriteLine(responseFromServer);
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch
            {
                MessageBox.Show("Сервер недоступен", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            buttonChart.Visibility = Visibility.Visible;
        }

        private void ButtonChart_Click(object sender, RoutedEventArgs e)
        {
            locationTitle = comboBoxLoc.SelectedValue.ToString();
            ChartWindow ch = new ChartWindow(locationTitle);
            ch.ShowDialog();
        }
    }
}
