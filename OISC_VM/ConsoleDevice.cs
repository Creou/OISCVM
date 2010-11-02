using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OISC_VM
{
    public abstract class AutoRefreshingMemoryMappedDevice : IMemoryMappedDevice
    {
        private IMemory _memoryBus;

        public AutoRefreshingMemoryMappedDevice(IMemory memoryBus, int memoryRangeStart, int memoryRangeLength, int refreshInterval)
        {
            _memoryBus = memoryBus;
            this.MemoryRangeStart = memoryRangeStart;
            this.MemoryRangeLength = memoryRangeLength;

            Timer refreshTimer = new Timer(new TimerCallback((o) => { RefreshDevice(); }), null, 0, 1000);
        }

        public void RefreshDevice()
        {
            int[] memory = _memoryBus.ReadDataRange(this.MemoryRangeStart, this.MemoryRangeLength);
            RefreshDevice(memory);
        }

        protected abstract void RefreshDevice(int[] mappedMemory);


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
    }
    public class ConsoleDevice : AutoRefreshingMemoryMappedDevice
    {
        public ConsoleDevice(IMemory memoryBus, int memoryRangeStart, int memoryRangeLength, int refreshInterval)
            : base(memoryBus, memoryRangeStart, memoryRangeLength, refreshInterval)
        {
        }

        protected override void RefreshDevice(int[] mappedMemory)
        {
            Console.Clear();
            for (int i = 0; i < mappedMemory.Length; i++)
            {
                Console.Write(mappedMemory[i] + " ");
            }
            Console.WriteLine();
        }
    }

}
