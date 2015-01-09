using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FourierTransformProvider
{
    public enum FourierType
    {
        DFT,
        FFT
    }
    internal class TransformTools
    {
        public static bool AssertDataArray(Complex[] DataArray, out int PowerOfTwo)
        {
            if (IsPowerOfTwo(DataArray.Length, out PowerOfTwo))
                return true;
            return false;
        }

        public static bool IsPowerOfTwo(int Number, out int PowerOfTwo)
        {
            PowerOfTwo = 0;
            if ((Number != 0) && ((Number & (Number - 1)) == 0))
            {
                PowerOfTwo = (int)Math.Log(Number, 2);
                return true;
            }
            return false;

        }

        public static int ReverseBits(int Number, int BitsNumberToRevese)
        {
            if (BitsNumberToRevese <= 0)
                return Number;
            int result = 0x00000000;
            int mask = 0x00000001;
            for (int i = 0; i < BitsNumberToRevese; i++)
            {
                result = (result | (((Number >> i) & mask) << (BitsNumberToRevese - 1 - i)));
            }
            return result;
        }

        public static int[] GetIndexRevercedArray(int ArrayLength,int PowerOfTwo)
        {
            if ((int)Math.Pow(2, PowerOfTwo) != ArrayLength)
                throw new NotSupportedException("Wrong array length");
            var arr = new int[ArrayLength];
            for (int i = 0; i < ArrayLength; i++)
            {
                
                var reverced_i = ReverseBits(i, PowerOfTwo);
                if (i < reverced_i)
                    arr[i] = reverced_i;
                else
                    arr[i] = i;
            }
            return arr;
        }
        public static int[] GetIndexRevercedArray(int ArrayLength, int PowerOfTwo, int Offset=0)
        {
            if ((int)Math.Pow(2, PowerOfTwo) != ArrayLength)
                throw new NotSupportedException("Wrong array length");
            var arr = new int[ArrayLength];
            for (int i = 0; i < ArrayLength; i++)
            {

                var reverced_i = ReverseBits(i, PowerOfTwo);
                if (i < reverced_i)
                    arr[i] = reverced_i;
                else
                    arr[i] = i;
                arr[i] += Offset;
            }
            return arr;
        }
        public static void BinaryReverseSwapData(double[] DataArray, int PowerOfTwo)
        {
            for (int i = 0; i < DataArray.Length; i++)
            {
                var reverced_i = ReverseBits(i, PowerOfTwo);
                if (i < reverced_i)
                {
                    var buf = DataArray[i];
                    DataArray[i] = DataArray[reverced_i];
                    DataArray[reverced_i] = buf;
                }
            }
        }
        public static void BinaryReverseSwapData(Complex[] DataArray, int PowerOfTwo)
        {
            for (int i = 0; i < DataArray.Length; i++)
            {
                var reverced_i = ReverseBits(i, PowerOfTwo);
                if (i < reverced_i)
                {
                    var buf = DataArray[i];
                    DataArray[i] = DataArray[reverced_i];
                    DataArray[reverced_i] = buf;
                }
            }
        }

        internal static bool AssertDataArray(FourierDataSet Data, out int PowerOfTwo)
        {
            if (IsPowerOfTwo(Data.Length, out PowerOfTwo))
                return true;
            return false;
        }

        internal static void BinaryReverseSwapData(FourierDataSet Data, int PowerOfTwo)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                var reverced_i = ReverseBits(i, PowerOfTwo);
                if (i < reverced_i)
                {
                    var buf = Data[i];
                    Data[i] = Data[reverced_i];
                    Data[reverced_i] = buf;
                }
            }
        }
    }

}
