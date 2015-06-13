using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using csound6netlib;

namespace CSoundBridgeTests {
    public class CsoundHandler : ICsound6Runnable {

        public Csound6NetRealtime Csound;
        public bool Done;

        public CsoundHandler() : this(new Csound6NetRealtime()) { }
        public CsoundHandler(string[] args) : this(new Csound6NetRealtime(), args) { }
        public CsoundHandler(Csound6NetRealtime csound, string[] args = null) {
            Csound = csound;
            Done = false;
            if (args != null) {
                var result = Csound.Compile(args);
                if (result != CsoundStatus.Success)
                    throw new Exception("[CsoundHandler] Fail to compile arguments.");
            }
        }

        public object this[string channel] {
            get { return Csound.GetSoftwareBus()[channel]; }
            set { Csound.GetSoftwareBus()[channel] = value; }
        }

        /// <summary>
        /// The method which will be executing when this CsoundHandler object is used as
        /// an ICsoundRunnable object for a Csound6NetThread instance.
        /// The method completes either when an active score completes or a holder of an
        /// instance of this class sets the done property to true.
        /// </summary>
        public uint Run(object csound_hanlder) {
            CsoundHandler handler = csound_hanlder as CsoundHandler;
            if (handler != null) {
                while (!handler.Done && !handler.Csound.PerformKsmps()) ;
            }
            return 0;
        }

    }
}
