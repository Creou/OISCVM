using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OISC_VM
{
    public class ConsoleDevice : InterruptTriggeredMemoryMappedDevice
    {
        public ConsoleDevice(IMemoryBus memoryBus, InterruptHandler interruptHandler, int memoryRangeStart, int memoryRangeLength, int refreshInterval)
            : base(memoryBus, interruptHandler, memoryRangeStart, memoryRangeLength, refreshInterval)
        {
        }

        protected override void RefreshDevice(byte[] mappedMemory)
        {
            String data = ASCIIEncoding.Default.GetString(mappedMemory, 0, mappedMemory.Length - 8).Replace("\0", String.Empty);
            Console.Clear();
            Console.Write(data);
        }
    }

}
