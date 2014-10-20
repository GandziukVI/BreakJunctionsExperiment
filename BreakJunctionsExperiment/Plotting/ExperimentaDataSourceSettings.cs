using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Windows.Threading;
using System.Windows.Controls;

using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;

using BreakJunctions.Events;
using System.Windows;

using Aids.Graphics;

namespace BreakJunctions.Plotting
{
    #region Representing plotting channels

    public enum Channels { Channel_01, Channel_02 }
    public enum Samples { Sample_01, Sample_02 }

    #endregion
}
