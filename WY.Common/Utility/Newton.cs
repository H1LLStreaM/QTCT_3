using System;
using System.Collections.Generic;
using System.Text;

namespace WY.Common.Utility
{
    public class Newton
    {
        public Newton() { }

        /// <summary>
        /// Newton插值计算
        /// </summary>
        /// <param name="length">坐标点的个数</param>
        /// <param name="X">X轴数组</param>
        /// <param name="Y">Y轴数组</param>
        /// <param name="inputValue">插值</param>
        //public static double getNewtonResult(int length, double[] X, double[] Y, double inputValue)
        //{
        //    //double result = 0;
        //    double[] Difference = Y;//存放差商的数组
           
        //    for (int k = 0; k < length; k++)
        //    {
        //        for (int j = length - 1; j > k; j--)
        //        {
        //            Difference[j] = (Difference[j] - Difference[j - 1]) / (X[j] - X[j - 1 - k]);
        //        }
        //    }
        //    double temp = 1;
        //    double newton = Difference[0];
        //    for (int i = 0; i < length; i++)
        //    {
        //        temp = temp * (inputValue - X[i]);
        //        newton = newton + temp * Difference[i + 1];
        //    }
        //    return newton;
        //}

        public static double NewtonCount(double pointx, double[] X, double[] Y, int n)
        {

            double[] Difference;//存放差商的数组
            Difference = Y;
            for (int k = 0; k < n; k++)
            {
                for (int j = n - 1; j > k; j--)
                {
                    Difference[j] = (Difference[j] - Difference[j - 1]) / (X[j] - X[j - 1 - k]);
                }

            }
            double temp = 1;
            double newton = Difference[0];
            for (int i = 0; i < n - 1; i++)
            {
                temp = temp * (pointx - X[i]);
                newton = newton + temp * Difference[i + 1];
            }
            return newton;
        }
    }
}
