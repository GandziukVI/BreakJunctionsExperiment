
namespace DigitalAnalyzerNamespace
{
    public class DigitizingInfo
    {
        private int m_SampleRate;
        public int SampleRate { get { return m_SampleRate; } }

        private int m_SamplesPerBlock;
        public int SamplesPerBlock { get { return m_SamplesPerBlock; } }

        public DigitizingInfo(int SampleRate, int SamplesPerBlock)
        {
            m_SampleRate = SampleRate;
            m_SamplesPerBlock = SamplesPerBlock;
        }
    }
}
