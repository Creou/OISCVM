using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OISC_VM.Devices
{
    public class KeyboardDevice : IMemoryMappedDevice
    {
        private IMemoryBus _memoryBus;

        public KeyboardDevice(IMemoryBus memoryBus, InterruptHandler interruptHandler, int memoryRangeStart, int memoryRangeLength)
        {
            this.MemoryRangeStart = memoryRangeStart;
            this.MemoryRangeLength = memoryRangeLength;

            _memoryBus = memoryBus;

            interruptHandler.RegisterSoftwareInterruptQueue("Keyboard request queue", "Keyboard", 1048439, 1048431);
        }

        public void StartDevice()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    ConsoleKeyInfo data = Console.ReadKey(true);
                    char charData = data.KeyChar;
                    _memoryBus.WriteData(1048319, charData);
                    _memoryBus.WriteData(1048431, 1);
                }
            });
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

        public String Name { get { return "Keyboard"; } }
    }
}
