using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_VM.Devices
{
    public abstract class InterruptTriggeredMemoryMappedDevice : IMemoryMappedDevice
    {
        private IMemoryBus _memoryBus;

        public InterruptTriggeredMemoryMappedDevice(IMemoryBus memoryBus, InterruptHandler interruptHandler, String irqName, int memoryRangeStart, int memoryRangeLength)
        {
            _memoryBus = memoryBus;
            this.MemoryRangeStart = memoryRangeStart;
            this.MemoryRangeLength = memoryRangeLength;

            interruptHandler.RegisterHardwareInterrupt(irqName, 1048568, 127, true, RefreshInterruptTriggered);

        }

        public void RefreshInterruptTriggered(HardwareInterruptRequest irq, long memoryAddress, long memoryValue)
        {
            RefreshDevice();
        }

        public void RefreshDevice()
        {
            byte[] memory = _memoryBus.ReadDataRange(this.MemoryRangeStart, this.MemoryRangeLength);
            RefreshDevice(memory);
        }

        protected abstract void RefreshDevice(byte[] mappedMemory);

        public int MemoryRangeStart
        {
            get;
            private set;
        }

        public int MemoryRangeLength
        {
            get;
            private set;
        }

        public abstract String Name { get; }
    }
}
