using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OISC_VM
{
    public abstract class AutoRefreshingMemoryMappedDevice : IMemoryMappedDevice
    {
        private IMemoryBus _memoryBus;

        public AutoRefreshingMemoryMappedDevice(IMemoryBus memoryBus, int memoryRangeStart, int memoryRangeLength, int refreshInterval)
        {
            _memoryBus = memoryBus;
            this.MemoryRangeStart = memoryRangeStart;
            this.MemoryRangeLength = memoryRangeLength;

            Timer refreshTimer = new Timer(new TimerCallback((o) => { RefreshDevice(); }), null, 0, 1000);
        }

        public void RefreshDevice()
        {
            byte[] memory = _memoryBus.ReadDataRange(this.MemoryRangeStart, this.MemoryRangeLength);
            RefreshDevice(memory);
        }

        protected abstract void RefreshDevice(byte[] mappedMemory);

        protected void WriteValue(long memoryLocation, long value)
        {
            _memoryBus.WriteData(memoryLocation, value);
        }

        protected long GetValue(long memoryLocation)
        {
            return _memoryBus.ReadData(memoryLocation);
        }


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
        public ConsoleDevice(IMemoryBus memoryBus, int memoryRangeStart, int memoryRangeLength, int refreshInterval)
            : base(memoryBus, memoryRangeStart, memoryRangeLength, refreshInterval)
        {
        }

        protected override void RefreshDevice(byte[] mappedMemory)
        {
            if (OutputFlag()) 
            {
                String data = ASCIIEncoding.Default.GetString(mappedMemory, 0, mappedMemory.Length - 8).Replace("\0", String.Empty);
                Console.Write(data);

                ResetFlag();
            }
        }

        private void ResetFlag()
        {
            WriteValue(MemoryRangeStart + MemoryRangeLength - 8, 0);
        }
        private bool OutputFlag()
        {
            return GetValue(MemoryRangeStart + MemoryRangeLength - 8) == 127;
        }
    }

}
