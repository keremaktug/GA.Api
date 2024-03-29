﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GA.Api.Types;

namespace GA.Api.Graphic
{
    public static class GraphicHelper
    {
        public static SolidBrush Hsl2Rgb(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);

            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;

                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;

                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;

                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;

                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;

                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            return new SolidBrush(Color.FromArgb((int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f)));
        }

        public static List<Brush> GenerateColorScheme(int count)
        {
            var r = new List<Brush>();

            var step = 1.0 / count;

            for (double i = 0; i < 1; i += step)
            {
                r.Add(Hsl2Rgb(i, 0.75, 0.5));
            }

            return r;
        }

        public static Dictionary<RectangleF, Brush> GeneratePoolGraph(Population population, List<object> domain_values, int chart_width, int chart_height, float x_offset, float y_offset)
        {
            var r = new Dictionary<RectangleF, Brush>();
            int sw = population.Chromosomes.First().Data.Count;
            int sh = population.Size;
            
            var color_scheme = GenerateColorScheme(domain_values.Count);

            float xstep = (chart_width / sw) + x_offset;
            float ystep = (chart_height / sh) + y_offset;

            for (int i = 0; i < sw; i++)
            {
                float dx = xstep * i;

                for (int j = 0; j < sh; j++)
                {
                    float dy = ystep * j;
                    var individual = population.Chromosomes[j].Data[i];
                    var index = domain_values.IndexOf(individual);
                    r.Add(new RectangleF(dx, dy, xstep, ystep), color_scheme[index]);
                }
            }

            return r;
        }
    }
}
