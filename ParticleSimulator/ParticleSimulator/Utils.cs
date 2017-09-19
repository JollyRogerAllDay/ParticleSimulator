using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSimulator
{
    static class Utils
    {

        //Calculates the distance between two points
        public static double Distance(int [] point1, int [] point2)
        {
            int[] delta = new int[point1.Length];
            int sumSquared = 0;
            for(int i=0; i < point1.Length; i++)
            {
                delta[i] = Math.Abs(point2[i] - point1[i]);
                sumSquared = delta[i] + sumSquared;
            }
            
            return Math.Sqrt(sumSquared);
        }

        //Calculates the distance between two points
        public static double Normalize(double x, double min, double max)
        {
            if (max == 0.0)
                return 0.0;
            else
                return (x - min) / max - min;
        }
    }
}
