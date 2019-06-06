using System.Collections.Generic;
using System.Linq;

namespace GA.Api.Math
{
    public static class MathHelper
    {
        public static int Bin2Dec(string number)
        {
            var r = 0;

            for (var i = 0; i < number.Length; i++)
            {
                if (number[i] == '1')
                {
                    r += (int)System.Math.Pow(2, 8 - i - 1);
                }
            }

            return r;
        }

        public static string Dec2Bin(int number)
        {
            var binary = string.Empty;

            while (number > 0)
            {
                binary = (number & 1) + binary;
                number = number >> 1;
            }

            binary = binary.PadLeft(8, '0');

            return binary;
        }

        public static double Mean(List<int> values)
        {
            return values.Average();
        }

        public static double StandardDeviation(List<int> values)
        {
            double mean = Mean(values);
            double r = 0.0;

            foreach (int value in values)
            {
                r += System.Math.Pow(value - mean, 2.0);
            }

            return System.Math.Sqrt(r / (values.Count - 1));
        }

        public static double Variance(List<int> values)
        {
            double mean = Mean(values);
            double r = 0.0;

            foreach (int value in values)
            {
                r += System.Math.Pow(value - mean, 2.0);
            }

            return r / (values.Count - 1);
        }

        public static double StandardDeviationPopulation(List<int> values)
        {
            double mean = Mean(values);
            double r = 0.0;

            foreach (int value in values)
            {
                r += System.Math.Pow(value - mean, 2.0);
            }

            return System.Math.Sqrt(r / values.Count);
        }

        public static double VariancePopulation(List<int> values)
        {
            double mean = Mean(values);
            double r = 0.0;

            foreach (int value in values)
            {
                r += System.Math.Pow(value - mean, 2.0);
            }

            return r / values.Count;
        }

        public static double DegreeToRadian(double angle)
        {
            return System.Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / System.Math.PI);
        }

        public static double Distance(double x1, double y1, double x2, double y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            return System.Math.Sqrt(System.Math.Pow(dx, 2.0) + System.Math.Pow(dy, 2.0));
        }
    }
}
