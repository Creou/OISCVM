using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_VM
{
    public class HardwareInterruptRequest
    {
        public long InterruptFlagAddress { get; private set; }
        public Action<HardwareInterruptRequest> Interrupt { get; private set; }
        public byte TriggerValue { get; private set; }
        public bool AutoReset { get; private set; }

        public HardwareInterruptRequest(long interruptFlagAddress, byte triggerValue, bool autoReset, Action<HardwareInterruptRequest> interrupt)
        {
            this.InterruptFlagAddress = interruptFlagAddress;
            this.Interrupt = interrupt;
            this.TriggerValue = triggerValue;
            this.AutoReset = autoReset;
        }
    }
}
