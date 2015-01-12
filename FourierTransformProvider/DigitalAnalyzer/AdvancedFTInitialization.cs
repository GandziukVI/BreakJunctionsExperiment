using FourierTransformProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalAnalyzerNamespace
{
    public enum DigitalAnalyzerSpectralRange
    {
        None,
        Discret499712Freq1_1600Step1Freq1647_249856Step61
    }
    class AdvancedFTInitialization
    {
        private DigitizingInfo m_digitizingInfo;
        private AdvancedFTRangeHandler[] m_Ranges;
        
        public AdvancedFTInitialization()
        {
            
        }
        public AdvancedFTInitialization(DigitalAnalyzerSpectralRange Range)
        {
            Initialize(Range);
        }
        public void Initialize(DigitalAnalyzerSpectralRange Range)
        {
            switch (Range)
            {
                
                case DigitalAnalyzerSpectralRange.Discret499712Freq1_1600Step1Freq1647_249856Step61:
                case DigitalAnalyzerSpectralRange.None:
                default:
                    var sampleRate = 499712;
                        var FreqRange1 = new FrequencyRange(1, 1599, 1);
                        var FreqRange2 = new FrequencyRange(1647, 248209, 61);
                        var samplesPerBlock = sampleRate;//or miminal from freq ranges steps
                        m_digitizingInfo = new DigitizingInfo(sampleRate, samplesPerBlock);
                        m_Ranges = new AdvancedFTRangeHandler[2];
                        m_Ranges[0] = new AdvancedFTRangeHandler(FreqRange1, m_digitizingInfo);
                        m_Ranges[1] = new AdvancedFTRangeHandler(FreqRange2, m_digitizingInfo);
                    break;
            }    
        }
        public DigitizingInfo DigitizingInfo { get { return m_digitizingInfo; } }
        public AdvancedFTRangeHandler[] FourierTransformRanges { get { return m_Ranges; } }


        
        

    }
}
