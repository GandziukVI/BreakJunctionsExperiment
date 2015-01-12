using FourierTransformProvider;
using System;
using System.Threading.Tasks;
using System.Windows;
using ComplexExtention;
using System.Collections.Generic;

namespace DigitalAnalyzerNamespace
{
    public class AdvancedFTRangeHandler// maybe change class name!!!
    {
        private FrequencyRange m_FreqRange;
        private IndexRange m_IndexRange;

        private FourierDataSet[] m_DataSets;
        private int m_RangeWindowCount;
        private FourierTransform m_FT;


        public AdvancedFTRangeHandler(FrequencyRange FreqRange, DigitizingInfo DigitInfo)
        {
            m_RangeWindowCount = (int)Math.Floor(DigitInfo.SampleRate / FreqRange.RangeStep);
            var FreqStep = DigitInfo.SampleRate * 1.0 / m_RangeWindowCount;

            var RangeFreqStartIndex = (int)Math.Ceiling(FreqRange.RangeStart / FreqStep);
            var RangeFreqStart = RangeFreqStartIndex * FreqStep;

            var RangeFreqBandWidthCount = (int)Math.Floor(FreqRange.RangeWidth / FreqStep);
            var RangeFreqBandWidth = RangeFreqBandWidthCount * FreqStep;

            m_FreqRange = new FrequencyRange(RangeFreqStart, RangeFreqBandWidth, FreqStep);
            m_IndexRange = new IndexRange(RangeFreqStartIndex, RangeFreqBandWidthCount);
            m_DataSets = new FourierDataSet[DigitInfo.SamplesPerBlock / m_RangeWindowCount];
            m_FT = new FourierTransform();

        }

        public void DataChanged(object sender, FourierDataSet NewData)
        {
            Parallel.For(0, m_DataSets.Length, i =>
            {
                m_DataSets[i] = new FourierDataSet(NewData, new IndexRange(i * m_RangeWindowCount, m_RangeWindowCount));
            });
        }

        public void ProcessRangeData()
        {
            Parallel.For(0, m_DataSets.Length, i =>
            {
                m_DataSets[i] = m_FT.UniversalFastFourierTransform(m_DataSets[i], false);
            });
        }


        public List<Point> AveragePowerSpectralDensity
        {
            get
            {
                var PSD = m_DataSets.PowerSpectralDensity();
                var list = new List<Point>();
                var jEnum = m_IndexRange.GetEnumerator();
                foreach (var freq in m_FreqRange)
                {
                    if (jEnum.MoveNext())
                        list.Add(new Point(freq, PSD[jEnum.Current]));
                    else break;
                }
                return list;
            }
        }

        public int SamplesNumber
        {
            get { return m_RangeWindowCount; }
        }

        public IEnumerable<Point> FrequencyRange
        {
            get
            {
                foreach (var freq in m_FreqRange)
                {
                    yield return new Point(freq, 0);
                }
            }

        }
    }
}
