using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace weather
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("C:\\Users\\mbright\\Downloads\\weather.dat");
            var headerline = lines.First();
            List<Tuple<string, int, int>> header = new List<Tuple<string, int, int>>();
            int? fieldTextStart = null;
            int fieldStart = 0;
            string fieldText;
            for(var i = 0; i < headerline.Length; ++i)
            {
                if(headerline[i] != ' ')
                {
                    fieldTextStart = i;
                }
                if(fieldTextStart != null && char.IsWhiteSpace(headerline[i]))
                {
                    int fieldLength = i - fieldStart;
                    fieldText = headerline.Substring(fieldStart, fieldLength);
                    fieldTextStart = null;
                    header.Add(new Tuple<string, int, int>(fieldText.Trim(), fieldLength, fieldStart));
                    fieldStart = i;
                }
            }
            fieldText = headerline.Substring(fieldStart);
            header.Add(new Tuple<string, int, int>(fieldText.Trim(), headerline.Length - fieldStart, fieldStart));
            List<List<string>> table = new List<List<string>>();

            int prevextend = 0;
            foreach(var line in lines.Skip(2))
            {
                List<string> row = new List<string>();
                int extend = 0;
                foreach(var c in header)
                {
                    int start = c.Item3 + prevextend;
                    int fieldlen = c.Item2;
                    if (start > line.Length)
                    {
                        row.Add("");
                    }
                    else
                    {
                        if (line.Length - 1 > start + fieldlen  && char.IsWhiteSpace(line[start + fieldlen - 1]))
                                extend = 0;
                        else if(line.Length-1 > start + fieldlen && !char.IsWhiteSpace(line[start+fieldlen]))
                            while (line.Length-1>start+fieldlen+extend && !char.IsWhiteSpace(line[start + fieldlen + extend])) extend++;
                        var val = line.Length > start + fieldlen ? line.Substring(start, fieldlen + extend) : line.Substring(start);
                        row.Add(val.Trim());
                        prevextend = extend;
                    }
                }
                table.Add(row);
            }

            //foreach (var row in table)
            //{
            //    foreach (var itm in row)
            //    {
            //        Console.Write(itm + " * ");
            //    }
            //    Console.WriteLine("");
            //}

            float minspread = 1000; // unreasonably large spread
            string minspreadday = "";
            foreach(var row in table)
            {
                string max = row[1].Replace("*","");
                string min = row[2].Replace("*", "");
                float spread = float.Parse(max) - float.Parse(min);
                if (spread < minspread)
                {
                    minspread = spread;
                    minspreadday = row[0];
                }
            }
            Console.WriteLine("Day with min temp spread: " + minspreadday);

        }
    }
}
