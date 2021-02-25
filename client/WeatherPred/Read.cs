using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace WeatherPred
{
    public static class Read
    {
        public static List<Field> ReadFields()
        {
            List<Field> fields = new List<Field>();
            string[] result = File.ReadAllLines("result.txt");

            foreach (var item in result)
            {
                fields.Add(new Field(item));
            }
            return fields;
        }
    }
}
