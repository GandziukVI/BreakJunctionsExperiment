using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourierTransformProvider
{
    public class IndexRange : RangeBase<int>
    {
        
        public IndexRange(int StartIndex,int RangeLength,int IndexStep=1):base(StartIndex,RangeLength,IndexStep)
        { }
    }
}
