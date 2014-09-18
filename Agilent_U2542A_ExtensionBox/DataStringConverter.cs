using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aids.Graphics;

namespace Agilent_U2542A_ExtensionBox
{
    public class DataStringConverter
    {
        public short[] ParseDataStringToInt(string dataString)
        {

            try
            {
                dataString = dataString.Substring(10);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return new short[] { };
            }

            byte[] data = Encoding.Default.GetBytes(dataString);
            short[] result = new short[(data.Length / 2)];
            for (int i = 0; i < data.Length; i += 2)
            {
                result[i / 2] = (short)((short)data[i] + (short)256 * ((short)data[i + 1]));

            }
            return result;
        }
        public List<PointD>[] ParseIntArrayIntoChannelData(Int16[] IntArray, int ACQ_Rate)
        {
            List<PointD>[] result = new List<PointD>[4];
            
            for (int i = 0; i < 4; i++) 
                result[i] = new List<PointD>();

            List<AnalogInputChannel> not_workingChannels = new List<AnalogInputChannel>();
            List<AnalogInputChannel> workingChannels = new List<AnalogInputChannel>();

            double value, time;

            foreach (AnalogInputChannel ch in AnalogInputChannels.Instance.ChannelArray)
            {
                if (ch.Enabled) workingChannels.Add(ch);
                else not_workingChannels.Add(ch);
            }
            for (int i = 0; i < IntArray.Length; i += workingChannels.Count)
            {

                time = (double)i / workingChannels.Count / ACQ_Rate;
                int j = i;
                foreach (AnalogInputChannel workingChannel in workingChannels)
                {

                    if (workingChannel.isBiPolarAC)
                        value = IntArray[j] * 2 * workingChannel.AC_Range / 65536;
                    else
                        value = (IntArray[j] / 65536 + 0.5) * workingChannel.AC_Range;
                    result[workingChannel.number - 101].Add(new PointD(time, value));
                    j++;
                }

                foreach (AnalogInputChannel not_workingChannel in not_workingChannels)
                {
                    result[not_workingChannel.number - 101].Add(new PointD(time, 0));
                }
            }
            return result;
        }
    }
}
