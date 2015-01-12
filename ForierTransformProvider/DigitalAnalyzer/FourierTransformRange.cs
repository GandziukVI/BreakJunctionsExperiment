using FourierTransformProvider;
using System;
using System.Threading.Tasks;
using ComplexExtention;
using System.Collections.Generic;
using System.Windows;
using System.Numerics;

namespace DigitalAnalyzerNamespace
{
    //public delegate void NewDataEventHandler(object sender, FourierDataSet NewData);


    public class FourierTransformRange// maybe change class name!!!
    {
        private FrequencyRange m_FreqRange;
        private IndexRange m_IndexRange;
        //private int m_SubArraysForFTLength;
        private FourierDataSet[] m_DataSets;
        private int m_RangeCount;
        private FourierTransform m_FT;
        //private List<Point>
        private Dictionary<double, Complex> m_PowerSpectralDensity;

        public FourierTransformRange(FrequencyRange FreqRange, DigitizingInfo DigitInfo)
        {
            m_RangeCount = (int)Math.Floor(DigitInfo.SampleRate / FreqRange.RangeStep);
            var FreqStep = DigitInfo.SampleRate * 1.0 / m_RangeCount;

            var RangeFreqStartIndex = (int)Math.Ceiling(FreqRange.RangeStart / FreqStep);
            var RangeFreqStart = RangeFreqStartIndex * FreqStep;

            var RangeFreqBandWidthCount = (int)Math.Floor(FreqRange.RangeWidth / FreqStep);
            var RangeFreqBandWidth = RangeFreqBandWidthCount * FreqStep;

            m_FreqRange = new FrequencyRange(RangeFreqStart, RangeFreqBandWidth, FreqStep);
            m_IndexRange = new IndexRange(RangeFreqStartIndex, RangeFreqBandWidthCount);
            //m_SubArraysForFTLength = RangeCount;
            m_PowerSpectralDensity = new Dictionary<double, Complex>();

            m_DataSets = new FourierDataSet[DigitInfo.SamplesPerBlock / m_RangeCount];

            m_FT = new FourierTransform();

        }

        public void DataChanged(object sender, FourierDataSet NewData)
        {
            Parallel.For(0, m_DataSets.Length, i =>
            {
                m_DataSets[i] = new FourierDataSet(NewData, new IndexRange(i * m_RangeCount, m_RangeCount));
            });
        }

        public void ProvideFourierTransform()
        {
            Parallel.For(0, m_DataSets.Length, i =>
            {
                m_DataSets[i] = m_FT.UniversalFastFourierTransform(m_DataSets[i], false);
            });
            var PSD = m_DataSets.AvarageDataSets(x => x.AbsSquared());
            var jEnum = m_IndexRange.GetEnumerator();
            foreach (var freq in m_FreqRange)
            {
                if (jEnum.MoveNext())
                    m_PowerSpectralDensity.Add(freq, PSD[jEnum.Current]);
                else break;
            }

        }

        public Dictionary<double, Complex> SpectraData { get { return m_PowerSpectralDensity; } }
    }
}
