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
            List<int> memory = new List<int>();

            // Load the data into the memory.

            // Load the program into memory.
            String[] lines = File.ReadAllLines("Add.txt");
            foreach (var line in lines)
            {
                String[] tokens = line.Split(' ');
                foreach (var token in tokens)                
                {
                    // Rebase each address as it is loaded.
                    //TODO:

                    // Load each instruction into memory.
                    memory.Add(int.Parse(token));
                }
            }
            
            int pc = 0;
            int a;
            int b;
            int c;

            while (pc >= 0)
            {
                a = memory[pc];
                b = memory[pc+1];
                c = memory[pc+2];
                if (a < 0 || b < 0)
                {
                    pc = -1;
                }
                else {
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

            Console.WriteLine("Finished");
            foreach (var col in memory)
            {
                Console.WriteLine(col);
            }
            Console.ReadLine();

        }
    }
}
