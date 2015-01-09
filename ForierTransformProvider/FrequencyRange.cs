using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourierTransformProvider
{
    public class FrequencyRange:RangeBase<double>
    {
        public FrequencyRange(double StartFrequency, double RangeBandWidth, double FrequencyStep)
            : base(StartFrequency, RangeBandWidth, FrequencyStep)
        {
        }

        
        
    }
}
