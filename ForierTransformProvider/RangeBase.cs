using System.Collections.Generic;

namespace FourierTransformProvider
{
    public class RangeBase<T>:IEnumerable<T>
    {
        private dynamic m_RangeStart;
        private dynamic m_RangeWidth;
        private dynamic m_RangeStep;
        private dynamic m_RangeEnd;
        //private int m_RangeCount;
        //private double m_BandWidth;
        public RangeBase(T RangeStart,T RangeWidth,T RangeStep)
        {
            dynamic RangeWidth_ = RangeWidth;
            RefreshData(RangeStart, RangeWidth,RangeStart+RangeWidth_, RangeStep);
            //m_RangeStart = RangeStart;
            //m_RangeWidth = RangeWidth;
            //m_RangeStep = RangeStep;
            
        }

        public T RangeStart { 
            get { return m_RangeStart; }
            set
            {
                RefreshData(value, m_RangeWidth,value+m_RangeWidth, m_RangeStep);
                //m_RangeStart = value; }
            }
        }
        public T RangeWidth {
            get { return m_RangeWidth; }
            set {
                RefreshData(m_RangeStart, value,m_RangeStart+value, m_RangeStep);
                //m_RangeWidth = value; 
            }
        }

        private void RefreshData(T RangeStart, T RangeWidth,T RangeEnd, T RangeStep)
        {
            m_RangeStart = RangeStart;
            m_RangeWidth = RangeWidth;
            m_RangeStep = RangeStep;
            m_RangeEnd = RangeEnd;
        }

        public T RangeStep { 
            get { return m_RangeStep; } 
            set {
                //m_RangeStep = value; 
                RefreshData(m_RangeStart, m_RangeWidth,m_RangeEnd, value);
            }
        }
        public T RangeEnd
        {
            get { return m_RangeEnd; }
            set { RefreshData(m_RangeStart, value - m_RangeStart, value, m_RangeStep); }
        }

        public int RangeCount
        {
            get { return (int)(m_RangeWidth / RangeStep); }
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (dynamic i = RangeStart; i < RangeWidth; i+=RangeStep)
            {
                yield return i;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
