using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApproximationGUI
{
    public static class Apr_func_Lagr
    {
        public static double InterpolateLagrangePolynomial(double x, List<double> xValues, List<double> yValues, int size)
        {
            double lagrangePol = 0;

            for (int i = 0; i < size; i++)
            {
                double basicsPol = 1;
                for (int j = 0; j < size; j++)
                {
                    if (j != i)
                    {
                        basicsPol *= (x - xValues[j]) / (xValues[i] - xValues[j]);
                    }
                }
                lagrangePol += basicsPol * yValues[i];
            }

            return lagrangePol;
        }
        
    }
}
