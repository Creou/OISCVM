using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using OISC_VM.Devices;

namespace OISC_VM
{
    class Program
    {
        public static Memory _mem;
        public static CPU _cpu;
        
        public static List<IMemoryMappedDevice> _mappedDevices;

        static void Main(string[] args)
        {
            // Create memory and load the program and arguments (if present).
            _mem = new Memory();
            if (args.Length >= 1)
            {
                _mem.LoadProgram(args[0], args.Skip(1));
            }
            else if (args.Length == 1)
            {
                _mem.LoadProgram(args[0]);
            }

            InterruptHandler interruptHandler = new InterruptHandler(_mem);

            _mappedDevices = new List<IMemoryMappedDevice>();

            // Create a memory mapped console device.
            ConsoleDevice consoleDevice = new ConsoleDevice(_mem, interruptHandler, 1048448, 128);
            _mappedDevices.Add(consoleDevice);

            KeyboardDevice keyboardDevice = new KeyboardDevice(_mem, interruptHandler, 1048319, 128);
            _mappedDevices.Add(keyboardDevice);

            DisplayDevices(_mem, _mappedDevices, interruptHandler);

            Console.WriteLine();
            Console.WriteLine("Ready. Press enter to begin");
            Console.ReadLine();
            Console.Clear();

            // Create the CPU and start it running.
            _cpu = new CPU(_mem, interruptHandler);
            Thread.Sleep(1000);
            _cpu.Run();

            Console.ReadLine();
        }

        private static void DisplayDevices(Memory memory, List<IMemoryMappedDevice> devices, InterruptHandler interruptHandler)
        {
            Console.WriteLine("Memory {0} bytes", memory.Size );
            Console.WriteLine();
            Console.WriteLine("Devices");
            Console.WriteLine("=============================================");
            foreach (var mappedDevice in devices.OrderBy(d=>d.MemoryRangeStart))
            {
                Console.WriteLine("{0} - {1} bytes [{2}-{3}]", mappedDevice.Name.PadRight(15), mappedDevice.MemoryRangeLength, mappedDevice.MemoryRangeStart, mappedDevice.MemoryRangeStart + mappedDevice.MemoryRangeLength);
            }
            Console.WriteLine("=============================================");

            String irqList = interruptHandler.GetIrqList();
            Console.WriteLine();
            Console.WriteLine(irqList);
        }
    }
}
