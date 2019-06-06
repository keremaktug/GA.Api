using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GA.Api.Types;

namespace GA.Api.Graphic
{
    public class GraphicGenerator
    {
        #region Properties
        
        private Population population;

        public int ChromosomeLength
        {
            get;
            set;
        }

        public int ChromosomeCount
        {
            get;
            set;
        }

        #endregion

        #region Constructor       

        public GraphicGenerator(Population pop)
        {
            this.population = pop;
            ChromosomeLength = pop.Chromosomes.First().Data.Count;
            ChromosomeCount = pop.Chromosomes.Count;
        }

        #endregion

        #region Functions

        public Dictionary<RectangleF, Brush> GenerateFitnessBars(int canvas_width, int canvas_height)
        {
            //var r = new Dictionary<RectangleF, Brush>();

            //var bar_width = canvas_width / GA.PoolSize;

            //var min_fitness = GA.Pool.GetMinimumFitness();
            //var max_fitness = GA.Pool.GetMaximumFitness();
            //var fitness_interval = max_fitness - min_fitness;

            //for (int i = 0; i < GA.PoolSize; i++)
            //{
            //    Chromosome chromosome = GA.Pool.Chromosomes[i];
            //    var fitness_ratio = chromosome.Fitness / fitness_interval;
            //    var bar_height = canvas_height * fitness_ratio;

            //    var rect = new Rectangle((int)(bar_width * i), canvas_height - (int)bar_height, (int)bar_width, (int)bar_height);
            //    r.Add(rect, Brushes.DarkBlue);
            //}

            //return r;

            return null;
        }

        public Dictionary<RectangleF, Brush> GeneratePoolGraph(int canvas_width, int canvas_height)
        {
            var r = new Dictionary<RectangleF, Brush>();
            int sw = population.Chromosomes.First().Data.Count;
            int sh = population.Size;

            var color_scheme = GenerateColorScheme(sw);

            float xstep = (float)(canvas_width / sw) + 0.1f;
            float ystep = (float)(canvas_height / sh) + 0.4f;

            for (int i = 0; i < sw; i++)
            {
                float dx = xstep * i;

                for (int j = 0; j < sh; j++)
                {
                    float dy = ystep * j;
                    //var gene = MathHelper.Bin2Dec(pool.Chromosomes[j].Genes[i].GeneData);
                    var gene_data = population.Chromosomes[j].Data[i];

                    

                    //var gene_index = population.Chromosomes[j].Data.IndexOf(gene);
                    //var gene_index = GA.InputValues.IndexOf(gene);
                    
                    r.Add(new RectangleF(dx, dy, xstep, ystep), color_scheme[i]);
                }
            }

            return r;

        }

        public Brush GetGeneColor(int i)
        {
            var brushes = new List<Brush>();
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#eef64a")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#fffa08")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#ffd107")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#fc9700")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#ee6b00")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#e43b01")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#d21300")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#a60c00")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#740800")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#000088")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#0000b9")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#0008de")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#0058f7")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#119cff")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#0f9aff")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#1ec0ff")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#2be9ff")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#70f1ec")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#8defaa")));
            brushes.Add(new SolidBrush(ColorTranslator.FromHtml("#97eea0")));
            return brushes[i];
        }

        #endregion

        #region Helper Functions

        public List<Brush> GenerateColorScheme(int count)
        {
            var r = new List<Brush>();

            var step = 1.0 / count;

            for(double i = 0; i < 1; i += step)
            {
                r.Add(Hsl2Rgb(i, 0.75, 0.5));
            }

            return r;
        }

        public SolidBrush Hsl2Rgb(double h, double sl, double l)
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

        #endregion
    }
}
