using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WeatherPred
{
    /// <summary>
    /// Логика взаимодействия для ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        Dictionary<string, List<string[]>> dic;
        static List<string[]> rows;
        HashSet<string> loc;
        public static string localLocTitle;
        public ChartWindow(string locationTitle)
        {
            InitializeComponent();
            localLocTitle = locationTitle;
            Draw();
        }
        void ReadFile()
        {
            rows = new List<string[]>();
            dic = new Dictionary<string, List<string[]>>();
            StreamReader reader = new StreamReader("tr.csv");
            string line = "";
            loc = new HashSet<string>();
            while ((line = reader.ReadLine()) != null)
            {
                string[] elem = line.Split(',');
                loc.Add(elem[1]);
                rows.Add(elem);
            }
            loc.Remove("Location");
            reader.Close();

            foreach (string locTitle in loc)
            {
                dic[locTitle] = new List<string[]>();
                foreach (string[] row in rows)
                {
                    if (row[1] == locTitle)
                    {
                        dic[locTitle].Add(row);
                    }
                }
            }

            Console.Read();
        }
        public void Draw()
        {
            ReadFile();
            DrawMinMaxTemp();
            DrawRainfall();
            DrawWindSpeed();
            DrawMinTempDisp();
            DrawMaxTempDisp();
            DrawRainfallDisp();
            DrawWindSpeedDisp();
        }

       
        void DrawMinMaxTemp()
        {
            chartMinMaxTemp.ChartAreas.Clear();
            chartMinMaxTemp.Series.Clear();
            ChartArea chartArea1 = new ChartArea();
            chartMinMaxTemp.ChartAreas.Add(chartArea1);
            chartMinMaxTemp.Titles.Add("Минимальная и максимальная температура ");
            Series series1 = new Series("Минимальная температура");
            series1.ChartType = SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.Blue;
            series1.MarkerSize = 3;
            series1.MarkerStyle = MarkerStyle.Circle;
            series1.Enabled = true;

            Series series2 = new Series("Максимальная температура");
            series2.ChartType = SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.Red;
            series2.MarkerSize = 3;
            series2.MarkerStyle = MarkerStyle.Circle;
            series2.Enabled = true;

            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.AxisX.ScaleView.Zoomable = true;
            chartArea1.AxisY.ScrollBar.IsPositionedInside = true;

            chartArea1.CursorY.IsUserEnabled = true;
            chartArea1.CursorY.IsUserSelectionEnabled = true;
            chartArea1.AxisY.ScaleView.Zoomable = true;
            chartArea1.AxisY.ScrollBar.IsPositionedInside = true;

            foreach (var item in dic)
            {
                if (item.Key == localLocTitle)
                {
                    foreach (string[] row in item.Value)
                    {
                        series1.Points.AddXY(row[0].ToString(), Convert.ToDouble(row[2].Replace('.', ',')));
                        series2.Points.AddXY(row[0].ToString(), Convert.ToDouble(row[3].Replace('.', ',')));
                    }
                }
            }
            chartMinMaxTemp.Series.Add(series1);
            chartMinMaxTemp.Series.Add(series2);
            chartMinMaxTemp.Legends.Add(new Legend());
            chartMinMaxTemp.Legends[0].Enabled = true;
        }
        void DrawRainfall()
        {
            chartRainfall.ChartAreas.Clear();
            chartRainfall.Series.Clear();
            ChartArea chartArea1 = new ChartArea();
            chartRainfall.ChartAreas.Add(chartArea1);
            chartRainfall.Titles.Add("Количество выпавших осадков");
            Series series1 = new Series("Дни");
            series1.ChartType = SeriesChartType.Column;
            series1.Color = System.Drawing.Color.Aqua;
            series1.MarkerSize = 3;
            series1.MarkerStyle = MarkerStyle.Circle;

            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.AxisX.ScaleView.Zoomable = true;
            chartArea1.AxisY.ScrollBar.IsPositionedInside = true;

            chartArea1.CursorY.IsUserEnabled = true;
            chartArea1.CursorY.IsUserSelectionEnabled = true;
            chartArea1.AxisY.ScaleView.Zoomable = true;
            chartArea1.AxisY.ScrollBar.IsPositionedInside = true;

            foreach (var item in dic)
            {
                if (item.Key == localLocTitle)
                {
                    foreach (string[] row in item.Value)
                    {
                        series1.Points.AddXY(row[0].ToString(), Convert.ToDouble(row[4].Replace('.', ',')));
                    }
                }
            }
            chartRainfall.Series.Add(series1);
        }
        void DrawWindSpeed()
        {
            chartWindSpeed.ChartAreas.Clear();
            chartWindSpeed.Series.Clear();
            ChartArea chartArea1 = new ChartArea();
            chartWindSpeed.ChartAreas.Add(chartArea1);
            chartWindSpeed.Titles.Add("Скорость ветра");
            Series series1 = new Series("Дни");
            series1.ChartType = SeriesChartType.Kagi;
            series1.Color = System.Drawing.Color.Fuchsia;
            series1.MarkerSize = 3;
            series1.MarkerStyle = MarkerStyle.Circle;

            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.AxisX.ScaleView.Zoomable = true;
            chartArea1.AxisY.ScrollBar.IsPositionedInside = true;

            chartArea1.CursorY.IsUserEnabled = true;
            chartArea1.CursorY.IsUserSelectionEnabled = true;
            chartArea1.AxisY.ScaleView.Zoomable = true;
            chartArea1.AxisY.ScrollBar.IsPositionedInside = true;

            foreach (var item in dic)
            {
                if (item.Key == localLocTitle)
                {
                    foreach (string[] row in item.Value)
                    {
                        series1.Points.AddXY(row[0].ToString(), Convert.ToDouble(row[8].Replace('.', ',')));
                    }
                }
            }
            chartWindSpeed.Series.Add(series1);
        }

        void DrawWindSpeedDisp()
        {
            chartWindSpeedDisp.ChartAreas.Clear();
            chartWindSpeedDisp.Series.Clear();
            ChartArea chartArea1 = new ChartArea();
            chartWindSpeedDisp.ChartAreas.Add(chartArea1);


            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.AxisX.ScaleView.Zoomable = true;
            chartArea1.AxisY.ScrollBar.IsPositionedInside = true;

            chartArea1.CursorY.IsUserEnabled = true;
            chartArea1.CursorY.IsUserSelectionEnabled = true;
            chartArea1.AxisY.ScaleView.Zoomable = true;
            chartArea1.AxisY.ScrollBar.IsPositionedInside = true;
            chartWindSpeedDisp.Titles.Add("Скорость ветра");
            Series series1 = new Series("Дни");
            series1.ChartType = SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.Fuchsia;
            series1.BorderWidth = 4;
            series1.MarkerSize = 8;
            series1.MarkerStyle = MarkerStyle.Diamond;

            double min = double.MaxValue;
            double max = double.MinValue;
            double avg = 0;
            int countLocation = 0;

            foreach (var item in dic)
            {
                if (item.Key == localLocTitle)
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        avg += Convert.ToDouble(item.Value[i][8].Replace('.', ','));
                    }
                }
                countLocation = item.Value.Count;
                avg = avg / countLocation;
            }

            foreach (var item in dic)
            {
                if (item.Key == localLocTitle)
                {
                    foreach (string[] row in item.Value)
                    {
                        series1.Points.AddXY(row[0].ToString(), Convert.ToDouble(row[8].Replace('.', ',')) - avg);

                        if (Convert.ToDouble(row[8].Replace('.', ',')) > max)
                            max = Convert.ToDouble(row[8].Replace('.', ','));
                        if (Convert.ToDouble(row[8].Replace('.', ',')) < min)
                            min = Convert.ToDouble(row[8].Replace('.', ','));
                    }

                }
            }
            double range = Math.Abs(max - min);
            double interval = range / 20;

            List<int> dispList = new List<int>();

            for (int i = 0; i < 20; i++)
            {
                dispList.Add(0);

                foreach (var item in dic)
                {
                    if (item.Key == localLocTitle)
                    {
                        for (int j = 0; j < item.Value.Count; j++)
                        {
                            if (Convert.ToDouble(item.Value[j][8].Replace('.', ',')) > min + (i) * interval && Convert.ToDouble(item.Value[j][8].Replace('.', ',')) < min + (i + 1) * interval)
                                dispList[i] += 1;

                        }
                    }

                }
            }
            series1.Points.Clear();

            foreach (int i in dispList)
            {
                series1.Points.Add(Convert.ToDouble(i));

            }

            chartWindSpeedDisp.Series.Add(series1);
        }
        void DrawRainfallDisp()
        {
            chartRainfallDisp.ChartAreas.Clear();
            chartRainfallDisp.Series.Clear();
            ChartArea chartArea1 = new ChartArea();
            chartRainfallDisp.ChartAreas.Add(chartArea1);


            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.AxisX.ScaleView.Zoomable = true;
            chartArea1.AxisY.ScrollBar.IsPositionedInside = true;

            chartArea1.CursorY.IsUserEnabled = true;
            chartArea1.CursorY.IsUserSelectionEnabled = true;
            chartArea1.AxisY.ScaleView.Zoomable = true;
            chartArea1.AxisY.ScrollBar.IsPositionedInside = true;
            chartRainfallDisp.Titles.Add("Количество осадков");
            Series series1 = new Series("Дни");
            series1.ChartType = SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.Aqua;
            series1.BorderWidth = 4;
            series1.MarkerSize = 8;
            series1.MarkerStyle = MarkerStyle.Diamond;

            double min = double.MaxValue;
            double max = double.MinValue;
            double avg = 0;
            int countLocation = 0;

            foreach (var item in dic)
            {
                if (item.Key == localLocTitle)
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        avg += Convert.ToDouble(item.Value[i][4].Replace('.', ','));
                    }
                }
                countLocation = item.Value.Count;
                avg = avg / countLocation;
            }

            foreach (var item in dic)
            {
                if (item.Key == localLocTitle)
                {
                    foreach (string[] row in item.Value)
                    {
                        series1.Points.AddXY(row[0].ToString(), Convert.ToDouble(row[4].Replace('.', ',')) - avg);

                        if (Convert.ToDouble(row[4].Replace('.', ',')) > max)
                            max = Convert.ToDouble(row[4].Replace('.', ','));
                        if (Convert.ToDouble(row[4].Replace('.', ',')) < min)
                            min = Convert.ToDouble(row[4].Replace('.', ','));
                    }

                }
            }
            double range = Math.Abs(max - min);
            double interval = range / 20;

            List<int> dispList = new List<int>();

            for (int i = 0; i < 20; i++)
            {
                dispList.Add(0);

                foreach (var item in dic)
                {
                    if (item.Key == localLocTitle)
                    {
                        for (int j = 0; j < item.Value.Count; j++)
                        {
                            if (Convert.ToDouble(item.Value[j][4].Replace('.', ',')) > min + (i) * interval && Convert.ToDouble(item.Value[j][4].Replace('.', ',')) < min + (i + 1) * interval)
                                dispList[i] += 1;

                        }
                    }

                }
            }
            series1.Points.Clear();

            foreach (int i in dispList)
            {
                series1.Points.Add(Convert.ToDouble(i) );

            }

            chartRainfallDisp.Series.Add(series1);
        }
        void DrawMinTempDisp()
        {
            chartMinMaxTempDisp.ChartAreas.Clear();
            chartMinMaxTempDisp.Series.Clear();
            ChartArea chartArea1 = new ChartArea();
            chartMinMaxTempDisp.ChartAreas.Add(chartArea1);
            chartMinMaxTempDisp.Titles.Add("Минимальная и максимальная температура ");
            Series series1 = new Series("Минимальная температура");
            series1.ChartType = SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.Blue;
            series1.BorderWidth = 4;
            series1.MarkerSize = 8;
            series1.MarkerStyle = MarkerStyle.Diamond;
            series1.Enabled = true;

            double min = double.MaxValue;
            double max = double.MinValue;
            double avg = 0;
            int countLocation = 0;

            foreach (var item in dic)
            {
                if (item.Key == localLocTitle)
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        avg += Convert.ToDouble(item.Value[i][2].Replace('.', ','));
                    }
                }
                countLocation = item.Value.Count;
                avg = avg / countLocation;
            }

            foreach (var item in dic)
            {
                if (item.Key == localLocTitle)
                {
                    foreach (string[] row in item.Value)
                    {
                        series1.Points.AddXY(row[0].ToString(), Convert.ToDouble(row[2].Replace('.', ',')) - avg);

                        if (Convert.ToDouble(row[2].Replace('.', ',')) > max)
                            max = Convert.ToDouble(row[2].Replace('.', ','));
                        if (Convert.ToDouble(row[2].Replace('.', ',')) < min)
                            min = Convert.ToDouble(row[2].Replace('.', ','));
                    }

                }
            }
            double range = Math.Abs(max - min);
            double interval = range / 20;

            List<int> dispList = new List<int>();

            for (int i = 0; i < 20; i++)
            {
                dispList.Add(0);

                foreach (var item in dic)
                {
                    if (item.Key == localLocTitle)
                    {
                        for (int j = 0; j < item.Value.Count; j++)
                        {
                            if (Convert.ToDouble(item.Value[j][2].Replace('.', ',')) > min + (i) * interval && Convert.ToDouble(item.Value[j][2].Replace('.', ',')) < min + (i + 1) * interval)
                                dispList[i] += 1;

                        }
                    }

                }
            }
            series1.Points.Clear();

            foreach (int i in dispList)
            {
                series1.Points.Add(Convert.ToDouble(i) );

            }

            chartMinMaxTempDisp.Series.Add(series1);
        }

        void DrawMaxTempDisp()
        {

            Series series2 = new Series("Максимальная температура");
            series2.ChartType = SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.Red;
            series2.BorderWidth = 4;
            series2.MarkerSize = 8;
            series2.MarkerStyle = MarkerStyle.Diamond;
            series2.Enabled = true;

            double min = double.MaxValue;
            double max = double.MinValue;
            double avg = 0;
            int countLocation = 0;

            foreach (var item in dic)
            {
                if (item.Key == localLocTitle)
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        avg += Convert.ToDouble(item.Value[i][3].Replace('.', ','));
                    }
                    countLocation = item.Value.Count;
                    avg = avg / countLocation;
                }
                
            }

            foreach (var item in dic)
            {
                if (item.Key == localLocTitle)
                {
                    foreach (string[] row in item.Value)
                    {
                        series2.Points.AddXY(row[0].ToString(), Convert.ToDouble(row[3].Replace('.', ',')) - avg);

                        if (Convert.ToDouble(row[3].Replace('.', ',')) > max)
                            max = Convert.ToDouble(row[3].Replace('.', ','));
                        if (Convert.ToDouble(row[3].Replace('.', ',')) < min)
                            min = Convert.ToDouble(row[3].Replace('.', ','));
                    }

                }
            }
            double range = Math.Abs(max - min);
            double interval = range / 20;

            List<int> dispList = new List<int>();

            for (int i = 0; i < 20; i++)
            {
                dispList.Add(0);

                foreach (var item in dic)
                {
                    if (item.Key == localLocTitle)
                    {
                        for (int j = 0; j < item.Value.Count; j++)
                        {
                            if (Convert.ToDouble(item.Value[j][3].Replace('.', ',')) > min + (i) * interval && Convert.ToDouble(item.Value[j][3].Replace('.', ',')) < min + (i + 1) * interval)
                                dispList[i] += 1;

                        }
                    }

                }
            }
            series2.Points.Clear();

            foreach (int i in dispList)
            {
                series2.Points.Add(Convert.ToDouble(i));

            }

            chartMinMaxTempDisp.Series.Add(series2);
            chartMinMaxTempDisp.Legends.Add(new Legend());
            chartMinMaxTempDisp.Legends[0].Enabled = true;
        }
    }


}
