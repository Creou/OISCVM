using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OISC_VM
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the memory.
            int[] memory = new int[255];

            // Load the program into memory.
            String[] lines = File.ReadAllLines(args[0]);
            int memoryIndex = 0;
            foreach (var line in lines)
            {
                String[] tokens = line.Split(' ');
                foreach (var token in tokens)                
                {
                    // Load each instruction into memory.
                    memory[memoryIndex] = int.Parse(token);
                    memoryIndex++;
                }
            }

            // Load the args into memory.
            for (int i = 1; i < args.Length; i++)
            {
                String param = args[i];
                memory[i] = int.Parse(param);
            }

            // Initialise the CPU status.
            int pc = 0;
            int a;
            int b;
            int c;

            // Load the program counter from the first memory value.
            pc = memory[0];

            // Execute the program.
            while (pc >= 0)
            {
                a = memory[pc];
                b = memory[pc+1];
                c = memory[pc+2];
                if (a < 0 || b < 0)
                {
                    pc = -1;
                }
                else
                {
                    memory[b] = memory[b] - memory[a];
                    if (memory[b] > 0)
                    {
                        pc = pc + 3;
                    }
                    else
                    {
                        pc = c;
                    }
                }
            }

            // Just dump out the whole memory.
            Console.WriteLine("Finished");
            foreach (var col in memory)
            {
                Console.Write(col + " ");
            }
            Console.ReadLine();

        }
    }
}
