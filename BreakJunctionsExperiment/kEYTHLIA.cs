using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions
{
    class kEYTHLIA
    {
        public Channel ChannelA
        {
            get { return new ChannelA(this); }
        }
        public Channel ChannelB
        {
            get { return new ChannelB(this); }
        }

        public void WriteToChA()
        { }
        public void WriteToChB()
        { }
    }
    abstract class Channel
    {
        protected kEYTHLIA m_Keytlya;
        public Channel(kEYTHLIA Key)
        {
            m_Keytlya = Key;
        }
        public abstract void WriteToChannel();
        
    }
    class ChannelA :Channel
    {
        public ChannelA(kEYTHLIA k) : base(k) { }
        public override void WriteToChannel()
        {
            m_Keytlya.WriteToChA();
        }
    }
    class ChannelB : Channel
    {
        public ChannelB(kEYTHLIA k) : base(k) { }
        public override void WriteToChannel()
        {
            m_Keytlya.WriteToChB();
        }
    }


}
