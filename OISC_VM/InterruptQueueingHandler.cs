using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_VM
{
    public class InterruptQueueingHandler
    {
        InterruptHandler _interruptHandler;
        private long _interruptFlagAddress;
        private String _interruptName;

        public InterruptQueueingHandler(InterruptHandler interruptHandler, String interruptQueueName, String interruptName, long interruptQueueingAddress, long interruptFlagAddress)
        {
            _interruptHandler = interruptHandler;
            _interruptFlagAddress = interruptFlagAddress;
            _interruptName = interruptName;

            _interruptHandler.RegisterHardwareInterrupt(interruptQueueName, interruptQueueingAddress, true, AddToInterruptQueue);
        }

        private void AddToInterruptQueue(HardwareInterruptRequest interruptRequest, long memoryAddress, long memoryValue)
        {
            _interruptHandler.RegisterSoftwareInterrupt(_interruptName, _interruptFlagAddress, memoryValue);
        }
    }
}
