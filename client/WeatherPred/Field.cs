using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherPred
{
    public class Field
    {
        public enum eType { Continuous, Discrete, Boolean }
        public Field(string str)
        {
            string[] split = str.Split(';');
            var cul = CultureInfo.InvariantCulture.NumberFormat;


            this.Name = split[1];
            

            switch (split[0])
            {
                case "cont":
                    Type = eType.Continuous;
                    this.Max = double.Parse(split[2].Split(':')[1], cul);
                    this.Min = double.Parse(split[3].Split(':')[1], cul);
                    break;
                case "disc":
                    Type = eType.Discrete;
                    Map = ParseMap(split[3]);
                    Categories = ParseCategories(Map);
                    break;
                case "bol":
                    Type = eType.Boolean;
                    this.Max = double.Parse(split[2].Split(':')[1], cul);
                    this.Min = double.Parse(split[3].Split(':')[1], cul);
                    break;
            }
        }

        public eType Type { get; set; }
        public string Name { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }


        public Dictionary<string, double> Map { get; set; }
        public List<double> Categories { get; set; }


        public static Dictionary<string, double> ParseMap(string s)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();

            s = s.Remove(0, 1);
            s = s.Remove(s.Length - 1);

            Regex reg = new Regex(@"'(?<K>.+?)':(?<V>.+?\d+?)");
            if (s.Contains("'"))
            {
                foreach (Match match in reg.Matches(s))
                    dic.Add(match.Groups["K"].Value, double.Parse(match.Groups["V"].Value, CultureInfo.InvariantCulture.NumberFormat));
            }
            else
            {
                string[] splitVal = s.Split(',');

                foreach (var val in splitVal)
                {
                    string[] pair = val.Split(':');
                    dic.Add(pair[0].Trim(), double.Parse(pair[1], CultureInfo.InvariantCulture.NumberFormat));
                }
            }
            return dic;
        }

        public static List<double> ParseCategories(Dictionary<string, double> dic)
        {
            return dic.Values.ToList();
        }

    }
}
