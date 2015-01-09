using DigitalAnalyzerNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FourierTransformProvider
{
    public class AdvancedFourierTransform:FourierTransform
    {
        private AdvancedFTRangeHandler[] m_RangeHandlers;
        private DigitizingInfo m_digitizingInfo;

        public AdvancedFourierTransform(DigitalAnalyzerSpectralRange range):base()
        {
            var creator = new AdvancedFTInitialization(range);
            m_RangeHandlers = creator.FourierTransformRanges;
            m_digitizingInfo = creator.DigitizingInfo;

        }

        public DigitizingInfo DigitizingInfo { get { return m_digitizingInfo; } }

        public List<Point> AdvancedFFT(List<Point> data)
        {
            Parallel.For(0, m_RangeHandlers.Length, i =>
                {
                    m_RangeHandlers[i].DataChanged(this, data.AsFourierDataSet());
                    m_RangeHandlers[i].ProcessRangeData();
                });
            return m_RangeHandlers.GetWholePSDfromRanges();
        }

        public int SamplesNumberInWholeSpectra
        {
            get
            {
                return m_RangeHandlers.GetSamplesNumberInWholeSpectra();
            }
        }

        public List<Point> GetFrequencyList()
        {
            return m_RangeHandlers.GetWholeFrequencyRangeList();
        }
    }
}
