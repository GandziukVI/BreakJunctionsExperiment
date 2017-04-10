using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aids.Graphics
{
    /// <summary>
    /// Represents Point of double accuracy
    /// </summary>
    public struct PointD
    {
        public double X;
        public double Y;

        public PointD(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
