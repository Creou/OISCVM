using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace OISC_VM
{
    class Program
    {
        public static Memory _mem;
        public static CPU _cpu;
        public static IMemoryMappedDevice _consoleDevice;

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

            // Create a memory mapped console device.
            _consoleDevice = new ConsoleDevice(_mem, interruptHandler, 1048448, 128, 100);

            // Create the CPU and start it running.
            _cpu = new CPU(_mem, interruptHandler);
            Thread.Sleep(1000);
            _cpu.Run();

            Console.ReadLine();

        }
    }
}
