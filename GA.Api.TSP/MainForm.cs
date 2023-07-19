using GA.Api.Events;
using GA.Api.Graphic;
using GA.Api.Math;
using GA.Api.Types;
using GA.Api.Types.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GA.Api.TSP
{
    public partial class MainForm : Form
    {
        private static readonly Random Random = new Random();

        private static List<object> cities = null;

        private GeneticSolver gs = null;

        private static Chromosome BestSolution = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Init();
            Clean();

            LoadCircularCities();
            //LoadRandomCities();
        }

        private void Init()
        {
            cmbPopulationSize.SelectedIndex = 4;
            cmbCrossoverType.SelectedIndex = 2;
            cmbMutationType.SelectedIndex = 0;
            cmbElitismRate.SelectedIndex = 3;
        }

        private void Clean()
        {
            chart1.Series.Clear();
            chart1.Series.Add("BestFitness");
            chart1.Series.Add("AverageFitness");
            chart1.Series["BestFitness"].ChartType = SeriesChartType.Spline;
            chart1.Series["AverageFitness"].ChartType = SeriesChartType.Spline;
            chart1.Series["BestFitness"].Points.Clear();
            chart1.Series["AverageFitness"].Points.Clear();
        }

        private static void LoadCircularCities()
        {
            cities = new List<object>();

            float radius = 100.0f;
            int point_count = 24;

            var j = 1;

            for (int i = 0; i < 360; i += 360 / point_count)
            {
                float px = (float)System.Math.Sin(MathHelper.DegreeToRadian(i)) * radius;
                float py = (float)System.Math.Cos(MathHelper.DegreeToRadian(i)) * radius;
                cities.Add(new City(j, px, py));
                j++;
            }
        }

        private static void LoadRandomCities()
        {
            cities = new List<object>();

            var width = 300;
            var height = 300;

            for (int i = 0; i < 100; i++)
            {
                var x = Random.Next(10, 290);
                var y = Random.Next(10, 290);
                x -= (int)(width * 0.5f);
                y -= (int)(height * 0.5f);
                cities.Add(new City(i + 1, x, y));
                i++;
            }
        }

        private void btnEvolve_Click(object sender, EventArgs e)
        {
            Clean();

            int pop_size_factor = Int32.Parse(cmbPopulationSize.Items[cmbPopulationSize.SelectedIndex].ToString());

            CrossoverType crossover_type = CrossoverType.OnePointCrossover;

            switch (cmbCrossoverType.SelectedIndex)
            {
                case 0: crossover_type = CrossoverType.OnePointCrossover; break;
                case 1: crossover_type = CrossoverType.UniformCrossover; break;
                case 2: crossover_type = CrossoverType.PMX; break;
            }

            double elitism_rate = Double.Parse(cmbElitismRate.SelectedItem.ToString());

            gs = new GeneticSolver(1024 * pop_size_factor, 2500, elitism_rate / 10, 0.2, crossover_type);
            gs.GeneratorFunction = PathGenerator;
            gs.FitnessFunction = CalculateFitness_TSPProblem;
            gs.IterationCompleted += Gs_IterationCompleted;

            gs.Evolve();
        }

        private Chromosome PathGenerator()
        {
            var city_indices = new List<int>();

            for (int i = 0; i < cities.Count; i++) city_indices.Add(i);

            var cities_list = city_indices.OrderBy(x => Random.Next()).ToList();

            var ch = new Chromosome();

            for (int i = 0; i < cities_list.Count; i++)
            {
                ch.Data.Add(cities[cities_list[i]]);
            }

            return ch;
        }

        public static double CalculateFitness_TSPProblem(Chromosome chromosome)
        {
            double f = 0.0f;

            for (int i = 0; i < chromosome.Data.Count - 1; i++)
            {
                var ca = (City)chromosome.Data[i];
                var cb = (City)chromosome.Data[i + 1];
                f += MathHelper.Distance(ca.CoordinateX, ca.CoordinateY, cb.CoordinateX, cb.CoordinateY);
            }

            var beg = (City)chromosome.Data.First();
            var end = (City)chromosome.Data.Last();
            f += MathHelper.Distance(beg.CoordinateX, beg.CoordinateY, end.CoordinateX, end.CoordinateY);

            return f;
        }

        private void Gs_IterationCompleted(object sender, IterationCompletedEventArgs e)
        {
            BestSolution = e.BestChromosome;
            AddPointsToChart(e.IterationCount, BestSolution.Fitness, e.AverageFitness);
            
            label1.Text = $"Total Length : {(int)CalculateFitness_TSPProblem(BestSolution)}";

            map.Refresh();
            PopulationChart.Refresh();

            Application.DoEvents();
        }

        private void AddPointsToChart(int iteration, double best_fitness, double average_fitness)
        {
            chart1.Series["BestFitness"].Points.Add(new DataPoint(iteration, best_fitness));
            chart1.Series["AverageFitness"].Points.Add(new DataPoint(iteration, average_fitness));
        }

        private void map_Paint(object sender, PaintEventArgs e)
        {
            draw_cities(e.Graphics);
            draw_lines(e.Graphics);
        }

        private void draw_cities(Graphics g)
        {
            for (int i = 0; i < cities.Count; i++)
            {
                var city = cities[i] as City;
                var p = new PointF(city.CoordinateX + (map.Width / 2), city.CoordinateY + (map.Height / 2));
                g.FillEllipse(Brushes.Red, new RectangleF(p, new SizeF(5, 5)));
                g.DrawString(city.Id.ToString(), SystemFonts.DefaultFont, Brushes.Black, new PointF(p.X + 5, p.Y + 5));
            }
        }

        private void draw_lines(Graphics g)
        {
            if (BestSolution == null) return;

            var hw = map.Width / 2;
            var hh = map.Height / 2;

            for (int i = 0; i < BestSolution.Data.Count - 1; i++)
            {
                var ca = (City)BestSolution.Data[i];
                var cb = (City)BestSolution.Data[i + 1];
                g.DrawLine(Pens.Red, ca.CoordinateX + hw, ca.CoordinateY + hh, cb.CoordinateX + hw, cb.CoordinateY + hh);
            }

            var beg = (City)BestSolution.Data.First();
            var end = (City)BestSolution.Data.Last();
            g.DrawLine(Pens.Red, beg.CoordinateX + hw, beg.CoordinateY + hh, end.CoordinateX + hw, end.CoordinateY + hh);
        }

        private void PopulationChart_Paint(object sender, PaintEventArgs e)
        {
            if (gs == null) return;
            if (gs.Population.Chromosomes.Count == 0) return;

            var gene_bars = GraphicHelper.GeneratePoolGraph(gs.Population, cities, PopulationChart.Width, PopulationChart.Height, 0.6f, 0.4f);

            foreach (KeyValuePair<RectangleF, Brush> kvp in gene_bars)
            {
                e.Graphics.FillRectangle(kvp.Value, kvp.Key);
            }
        }
    }
}
