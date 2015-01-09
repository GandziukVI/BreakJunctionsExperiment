using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using ZedGraph;
namespace FANS.classes
{
    public class Complex
    {
        public double Real, Imaginary;
        public Complex(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        }
        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.Real - c2.Real, c1.Imaginary - c2.Imaginary);
        }

        public static Complex operator *(Complex c1, double c2)
        {
            return new Complex(c1.Real * c2, c1.Imaginary * c2);
        }

        public static Complex operator /(Complex c1, double c2)
        {
            return new Complex(c1.Real / c2, c1.Imaginary / c2);
        }

        public static Complex operator *(Complex c1, Complex c2)
        {
            return new Complex(c1.Real * c2.Real - c1.Imaginary * c2.Imaginary, c1.Real * c2.Imaginary + c1.Imaginary * c2.Real);
        }
        public Complex fromExp(double amplitude, double phase)
        {
            this.Real = amplitude * Math.Cos(phase);
            this.Imaginary = amplitude * Math.Sin(phase);
            return this;
        }
        public Complex fromPhase(double phase)
        {
            this.Real = Math.Cos(phase);
            this.Imaginary = Math.Sin(phase);
            return this;
        }
        public Complex clone()
        {
            return new Complex(this.Real, this.Imaginary);
        }
        public double SquareAmplitude()
        {
            return (this.Real * this.Real + this.Imaginary * this.Imaginary);
        }

    }
    class BinaryReverter
    {
        private int[] _powersOfTwo;
        private byte[] _bitMask;
        public BinaryReverter(int Order = 19)
        {
            _bitMask = new byte[Order];
            _powersOfTwo = new int[Order];
            for (int i = 0; i < _powersOfTwo.Length; i++)
            {
                _powersOfTwo[i] = (int)Math.Pow(2, i);
            }
        }
        private void parseNumberToBitMask(int number)
        {
            for (byte i = 0; i < _bitMask.Length; i++)
            {
                if ((this._powersOfTwo[i] & number) == this._powersOfTwo[i])
                    _bitMask[i] = 1;
                else
                    _bitMask[i] = 0;
            }

        }
        private void reverseBitMask()
        {

            for (int i = 0; i < _bitMask.Length / 2; i++)
            {
                byte tmp = _bitMask[i];
                _bitMask[i] = _bitMask[_bitMask.Length - i - 1];
                _bitMask[_bitMask.Length - i - 1] = tmp;
            }

        }
        private int BitMaskToNumber()
        {

            int result = 0;
            int i = 0;
            foreach (byte bit in _bitMask)
            {
                result += bit * _powersOfTwo[i];
                i++;
            }
            return result;

        }
        public int getPosition(int OriginalPosition)
        {
            this.parseNumberToBitMask(OriginalPosition);
            this.reverseBitMask();
            return this.BitMaskToNumber();
        }

    }
    public class FFT
    {
        private Complex[] _W;//rotator matrix
        private int _Order;
        public int Order
        {
            set
            {
                _Order = value;
                BinaryReverter reverter = new BinaryReverter(_Order);
                _OrderForFFT = new int[(int)Math.Pow(2, _Order)];
                for (int i = 0; i < _OrderForFFT.Length; i++)
                    _OrderForFFT[i] = reverter.getPosition(i);
                Fill_W(this._Order);
            }
            get { return _Order; }
        }
        private int[] _OrderForFFT;

        public FFT(int order)
        {
            this.Order = order;

        }
        public Complex[] makeFFT(Complex[] data)
        {
            //if (data.Count != (int)Math.Pow(2, this.Order)) throw new Exception("The data has incorrect number of points" + data.Count);

            //double[] doubleData = data.Select(p => p.Y).ToArray();
            Complex[] AcquiredYData = data;//= new Complex[doubleData.Length];
            // you can get rid of this cycle and speed up!
            // except creating new array and filling it
            // you can already here create reorderedData array...
            //for (int i = 0; i < doubleData.Length; i++)
            //    AcquiredYData[i] = new Complex(doubleData[i], 0);

            Complex[] reorderedData = new Complex[AcquiredYData.Length];
            Complex[] FFT = new Complex[AcquiredYData.Length];
            for (int j = 0; j < AcquiredYData.Length; j++)
            {
                reorderedData[j] = AcquiredYData[_OrderForFFT[j]];
            }

            for (int i = 1; i <= this.Order; i++)
            {

                int increment = (int)Math.Pow(2, i);
                int halfIncrement = increment / 2;
                int _WMultiplier = _W.Length / increment;
                for (int j = 0; j < reorderedData.Length; j += increment)
                {
                    //here can be 1 cycle for both values
                    for (int k = 0; k < halfIncrement; k++)
                        FFT[j + k] = reorderedData[k + j] + _W[k * _WMultiplier] * reorderedData[j + k + halfIncrement];
                    for (int k = halfIncrement; k < increment; k++)
                        FFT[j + k] = reorderedData[k + j - halfIncrement] + _W[k * _WMultiplier] * reorderedData[j + k];
                }
                for (int j = 0; j < reorderedData.Length; j++)
                    reorderedData[j] = FFT[j].clone();
            }
            // don`t need to create new list... waste of memory. You can use the same from argument list!!!
            Complex[] result = new Complex[FFT.Length];
            for (int j = 0; j < FFT.Length; j++)
            {
                result[j] = FFT[j] / FFT.Length;
                //FFT[j] = FFT[j] / FFT.Length;
                //result.Add(data[j].X, FFT[j].SquareAmplitude());
            }

            return result;

        }

        private bool IsPowerOfTwo(int x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }
        private void Fill_W(int NumberOfIteration)
        {
            _W = new Complex[(int)Math.Pow(2, NumberOfIteration)];
            int N = (int)Math.Pow(2, NumberOfIteration);
            for (int k = 1; k < _W.Length; k++)
            {

                _W[k] = new Complex(0, 0);
                _W[k].fromExp(1, -2 * Math.PI * k / N);

            }
            _W[0] = new Complex(1, 0);
        }
    }
}
