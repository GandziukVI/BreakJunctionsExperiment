using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ComplexExtention
{
    public static class ComplexExtention
    {
        public static double Real(this Complex compl)
        {
            return compl.Real;
        }

        public static double AbsSquared(this Complex compl)
        {
            return compl.Real * compl.Real + compl.Imaginary * compl.Imaginary;
        }
        public static Complex Power(this Complex compl, int Power)
        {
            Complex result = new Complex(1, 0);
            while(Power>0)
            {
                if ((Power & 1) == 1)
                    result *= compl;
                compl *= compl;
                Power >>= 1;
            }
            return result;
        }
        
    }
}
