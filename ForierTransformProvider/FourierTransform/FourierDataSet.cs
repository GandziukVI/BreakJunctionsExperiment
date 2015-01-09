using System.Numerics;
using System;


namespace FourierTransformProvider
{
    

    public class FourierDataSet
    {
        private Complex[] m_DataSet;
        //private FourierTransformRange m_FTRange;
        public FourierDataSet (int RangeLength)//, FourierTransformRange Range)
        {
            m_DataSet = new Complex[RangeLength];
            //m_FTRange = Range;
        }
        public FourierDataSet(Complex[] Data)//, FourierTransformRange Range)
        {
            m_DataSet = Data;
            //m_FTRange = Range;
        }
        public FourierDataSet(double[]Data)//, FourierTransformRange Range)
        {
            m_DataSet = new Complex[Data.Length];
            for (int i = 0; i < Data.Length; i++)
            {
                m_DataSet[i] = new Complex(Data[i], 0);
            }
            //m_FTRange = Range;
        }
        public FourierDataSet(Complex[] Data, IndexRange RangeToCopy)//, FourierTransformRange Range)
        {
            m_DataSet = new Complex[RangeToCopy.RangeCount];
            Array.Copy(Data, RangeToCopy.RangeStart, m_DataSet, 0, RangeToCopy.RangeCount);
            //m_FTRange = Range;
        }
        public FourierDataSet(FourierDataSet Data, IndexRange RangeToCopy)//, FourierTransformRange Range)
        {
            m_DataSet = new Complex[RangeToCopy.RangeCount];
            int j = 0;
            foreach (var index in RangeToCopy)
                m_DataSet[j++] = Data[index];

            //m_FTRange = Range;
        }
        public FourierDataSet(double [] Data, IndexRange RangeToCopy)//, FourierTransformRange Range)
        {
            m_DataSet = new Complex[RangeToCopy.RangeCount];
            int counter = 0;
            foreach (var index in RangeToCopy)
                m_DataSet[counter++] = Data[index];
        }
        //public FourierTransformRange TransformRange
        //{
        //    get { return m_FTRange; }
        //}
        public Complex this[int index]
        {
            get { return m_DataSet[index]; }
            set { m_DataSet[index] = value; }
        }

        public int Length
        {
            get { return m_DataSet.Length; }
        }
        public Complex[] ComplexData { get { return m_DataSet; } }

        public int LengthIsPowerOfTwo
        {
            get
            {
                var PowerOfTwo = -1;
                if (TransformTools.IsPowerOfTwo(Length, out PowerOfTwo))
                    return PowerOfTwo;
                return -1;
            }
        }

    }
}
