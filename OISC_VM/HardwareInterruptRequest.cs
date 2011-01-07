using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_VM
{
    public class HardwareInterruptRequest
    {
        public string Name { get; private set; }
        public long InterruptFlagAddress { get; private set; }
        public Action<HardwareInterruptRequest, long, long> Interrupt { get; private set; }
        public long TriggerValue { get; private set; }
        public bool AutoReset { get; private set; }
        public bool SpecificTrigger { get; private set; }

        public HardwareInterruptRequest(String name, long interruptFlagAddress, bool autoReset, Action<HardwareInterruptRequest, long, long> interrupt)
        {
            this.Name = name;
            this.InterruptFlagAddress = interruptFlagAddress;
            this.Interrupt = interrupt;
            this.AutoReset = autoReset;
            this.SpecificTrigger = false;
        }

        public HardwareInterruptRequest(String name, long interruptFlagAddress, long triggerValue, bool autoReset, Action<HardwareInterruptRequest, long, long> interrupt)
        {
            this.Name = name;
            this.InterruptFlagAddress = interruptFlagAddress;
            this.Interrupt = interrupt;
            this.TriggerValue = triggerValue;
            this.AutoReset = autoReset;
            this.SpecificTrigger = true;
        }
    }
}
