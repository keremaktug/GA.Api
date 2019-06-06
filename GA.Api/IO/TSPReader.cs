using System;
using System.Collections.Generic;
using System.IO;

namespace GA.Api.IO
{
    public class TSPReader
    {
        public Tuple<List<int>, List<double>, List<double>> Read(string filename)
        {
            var ids = new List<int>();
            var x = new List<double>();
            var y = new List<double>();
            
            using (var sr = new StreamReader(filename))
            {
                bool node_coord_section = false;

                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();

                    if(node_coord_section)
                    {
                        var coord_line = sr.ReadLine();
                        string[] parts = coord_line.Split(' ');

                        ids.Add(Int32.Parse(parts[0].Trim()));
                        x.Add(Double.Parse(parts[1].Trim()));
                        y.Add(Double.Parse(parts[2].Trim()));
                    }

                    if(line.Contains("NODE_COORD_SECTION"))
                    {
                        node_coord_section = true;
                    }
                }
            }

            return new Tuple<List<int>, List<double>, List<double>>(ids, x, y);
        }
    }
}
