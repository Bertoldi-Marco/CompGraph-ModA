using System;
using System.Collections.Generic;
using System.Text;

namespace Game2Dprj
{
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(int targetsDestroyed, int clicks)
        {
            TargetsDestroyed = targetsDestroyed;
            Clicks = clicks;
        }

        public int TargetsDestroyed { get; set; }
        public int Clicks { get; set; }
    }
}
