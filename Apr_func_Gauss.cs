using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApproximationGUI
{
    public class Apr_func_Gauss
    {
        public int Basis { get; set; } = 3;
        private double[] Result { get; set; }
        private double[,] XYtable { get; set; }
        private double[,] Matrix { get; set; }
        private List<Point> Data { get; set; }


        public Apr_func_Gauss(List<Point> data, int basis)
        {
            Data = data;
            Basis = basis;
            XYtable = new double[2, data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                XYtable[0, i] = data[i].Arg;
                XYtable[1, i] = data[i].Value;
            }
            Basis++;
            Matrix = MakeSystem(XYtable, Basis);
            Result = Gauss(Matrix, Basis, Basis + 1);
            
        }

        public double Get_y(double x)
        {
            double Y  = 0;
            for (int i = 0; i < Basis; i++)
            {
                Y += Result[i] * Math.Pow(x, i);
            }
            return Y;
        }

        private double[] Gauss(double[,] matrix, int rowCount, int colCount)
            {
            int i;
            int[] mask = new int[colCount - 1];
            for (i = 0; i < colCount - 1; i++) mask[i] = i;
                if (GaussDirectPass(ref matrix, ref mask, colCount, rowCount))
                {
                    double[] answer = GaussReversePass(ref matrix, mask, colCount, rowCount);
                    return answer;
                }
                else return null;
            }

        private bool GaussDirectPass(ref double[,] matrix, ref int[] mask, int colCount, int rowCount)
        {
            int i, j, k, maxId, tmpInt;
            double maxVal, tempDouble;
            for (i = 0; i < rowCount; i++)
            {
                maxId = i;
                maxVal = matrix[i, i];
                for (j = i + 1; j < colCount - 1; j++)
                    if (Math.Abs(maxVal) < Math.Abs(matrix[i, j]))
                    {
                        maxVal = matrix[i, j];
                        maxId = j;
                    }
                if (maxVal == 0) return false;
                if (i != maxId)
                {
                    for (j = 0; j < rowCount; j++)
                    {
                        tempDouble = matrix[j, i];
                        matrix[j, i] = matrix[j, maxId];
                        matrix[j, maxId] = tempDouble;
                    }
                    tmpInt = mask[i];
                    mask[i] = mask[maxId];
                    mask[maxId] = tmpInt;
                }
                for (j = 0; j < colCount; j++) matrix[i, j] /= maxVal;
                for (j = i + 1; j < rowCount; j++)
                {
                    double tempMn = matrix[j, i];
                    for (k = 0; k < colCount; k++)
                        matrix[j, k] -= matrix[i, k] * tempMn;
                }
            }
            return true;
        }

        private double[] GaussReversePass(ref double[,] matrix, int[] mask, int colCount, int rowCount)
        {
            int i, j, k;
            for (i = rowCount - 1; i >= 0; i--)
                for (j = i - 1; j >= 0; j--)
                {
                    double tempMn = matrix[j, i];
                    for (k = 0; k < colCount; k++)
                        matrix[j, k] -= matrix[i, k] * tempMn;
                }
            double[] answer = new double[rowCount];
            for (i = 0; i < rowCount; i++) answer[mask[i]] = matrix[i, colCount - 1];
            return answer;
        }

        private double[,] MakeSystem(double[,] xYtable, int basis)
        {
            double[,] matrix = new double[basis, basis + 1];
            for (int i = 0; i < basis; i++)
            {
                for (int j = 0; j < basis; j++)
                {
                    matrix[i, j] = 0;
                }
            }
            for (int i = 0; i < basis; i++)
            {
                for (int j = 0; j < basis; j++)
                {
                    double sumA = 0, sumB = 0;
                    for (int k = 0; k < Data.Count; k++)
                    {
                        sumA += Math.Pow(XYtable[0, k], i) * Math.Pow(XYtable[0, k], j);
                        sumB += XYtable[1, k] * Math.Pow(XYtable[0, k], i);
                    }
                    matrix[i, j] = sumA;
                    matrix[i, basis] = sumB;
                }
            }
            return matrix;
        }

        public override string ToString()
        {
            string Text = "";            
            Text += "Cередньоквадратичне приближення за поліномом 3 степеня:\nY = ";
            for (int i = 0; i < Basis; i++)
            {
                if (Math.Round(Result[i], 3) != 0)
                    Text += ((Result[i] > 0) ? "+" : "") +
                        Math.Round(Result[i], 3).ToString() + ((i > 0) ? "*x^" + i.ToString() : "") + " ";
            }
            Text += "\n";
            return Text;
        }

    }
}
