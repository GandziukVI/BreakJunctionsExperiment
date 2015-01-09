using System;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections.Generic;
using System.Windows;
using ComplexExtention;

namespace FourierTransformProvider
{
    public static class FourierDataSetArrayExtentions
    {
        public static bool AllDataSetsEqualLength(this FourierDataSet[] DataSetArray, out int Length)
        {
            Length = 0;
            if (DataSetArray.Length == 0)
                return false;
            Length = DataSetArray[0].Length;
            for (int i = 1; i < DataSetArray.Length; i++)
                if (Length != DataSetArray[i].Length)
                    return false;
            return true;
        }
        public static FourierDataSet AvarageDataSets(this FourierDataSet[] DataSetArray)
        {
            //throw new NotImplementedException();
            int CommonLength = 0;
            if (!DataSetArray.AllDataSetsEqualLength(out CommonLength))
                return null;
            var res = new FourierDataSet(CommonLength);
            Parallel.For(0, CommonLength, i =>
            {
                for (int j = 0; j < DataSetArray.Length; j++)
                {
                    res[i] += DataSetArray[j][i];
                }
                res[i] /= DataSetArray.Length;
            });
            return res;
        }

        public static FourierDataSet AsFourierDataSet(this double[] Data)
        {
            var result = new FourierDataSet(Data.Length);
            Parallel.For(0, Data.Length, (i) =>
            {
                result[i] = new Complex(Data[i], 0);
            });
            return result;
        }
        public static FourierDataSet AsFourierDataSet(this List<Point> Data)
        {
            var result = new FourierDataSet(Data.Count);
            Parallel.For(0, Data.Count, (i) =>
            {
                result[i] = new Complex(Data[i].Y, 0); // possible issue of what should be here Data[i].X or Data[i].Y
            });
            return result;
        }
        public static FourierDataSet AsFourierDataSet(this Complex[] Data)
        {
            var result = new FourierDataSet(Data.Length);
            Parallel.For(0, Data.Length, (i) =>
            {
                result[i] = Data[i];
            });
            return result;
        }

        public static double[] PowerSpectralDensity(this FourierDataSet[] DataSetArray)
        {
            int CommonLength = 0;
            if (!DataSetArray.AllDataSetsEqualLength(out CommonLength))
                throw new NotSupportedException();//return null;
            var res = new double[CommonLength];
            double coef = 1.0*DataSetArray.Length * CommonLength * CommonLength;
            Parallel.For(0, CommonLength, i =>
            {
                for (int j = 0; j < DataSetArray.Length; j++)
                { 
                    res[i] += DataSetArray[j][i].AbsSquared();//abs;
                }
                res[i] /= coef; // A=(1/N)SQRT(re^2+im^2)
            });
            return res;
        }
        
    }
}
