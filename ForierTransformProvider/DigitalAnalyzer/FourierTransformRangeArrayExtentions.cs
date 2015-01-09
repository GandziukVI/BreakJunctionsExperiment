using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DigitalAnalyzerNamespace
{
    public static class FourierTransformRangeArrayExtentions
    {
        public static List<Point> GetWholePSDfromRanges(this AdvancedFTRangeHandler[] FTRangesArray)
        {
            var list = new List<Point>();
            for (int i = 0; i < FTRangesArray.Length; i++)
                list.AddRange(FTRangesArray[i].AveragePowerSpectralDensity);
                //throw new NotImplementedException();
                //list.AddRange(FTRangesArray[i].SpectraData);
            return list;
        }

        public static int GetSamplesNumberInWholeSpectra(this AdvancedFTRangeHandler[] FTRangesArray)
        {
            var counter = 0;
            for (int i = 0; i < FTRangesArray.Length; i++)
            {
                counter += FTRangesArray[i].SamplesNumber;
            }
            return counter;
        }
        public static List<Point> GetWholeFrequencyRangeList(this AdvancedFTRangeHandler[] FTRangesArray)
        {
            var list = new List<Point>();
            for (int i = 0; i < FTRangesArray.Length; i++)
            {
                list.AddRange(FTRangesArray[i].FrequencyRange);
            }
            return list;
        }
    }
}
