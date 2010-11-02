using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace OISC_VM
{
    public class Memory : IMemory
    {
        // Create the memory.
        int[] _memory;

        public Memory()
        {
            _memory = new int[255];
        }

        public void LoadProgram(String fileName) 
        {
            LoadProgram(fileName, null);
        }
        public void LoadProgram(String fileName, IEnumerable<String> programArguments)
        {
            // Load the program into memory.
            String[] lines = File.ReadAllLines(fileName);
            int memoryIndex = 0;
            foreach (var line in lines)
            {
                if (!line.StartsWith("//"))
                {
                    String[] tokens = line.Split(' ');
                    foreach (var token in tokens)
                    {
                        // Load each instruction into memory.
                        _memory[memoryIndex] = int.Parse(token);
                        memoryIndex++;
                    }
                }
            }

            // Load the args into memory.
            if (programArguments != null)
            {
                int argIndex = 3;
                foreach (var arg in programArguments)
                {
                    _memory[argIndex] = int.Parse(arg);
                    argIndex++;
                }
            }
        }

        public InstructionOperands FetchInstrucitonOperands(int memoryLocation)
        {
            return new InstructionOperands() { OperandA = _memory[memoryLocation], OperandB = _memory[memoryLocation + 1], OperandC = _memory[memoryLocation + 2] };
        }

        public int[] ReadDataRange(int rangeStart, int rangeLength)
        {
            return _memory.Skip(rangeStart).Take(rangeLength).ToArray();
        }
        
        public int ReadData(int memoryLocation)
        {
            return _memory[memoryLocation];
        }

        public void WriteData(int memoryLocation, int value)
        {
            _memory[memoryLocation] = value;
        }

        [Conditional("DEBUG")]
        internal void DebugWrite()
        {
            for (int i = 0; i < _memory.Length; i++)
            {
                if (i % 3 == 2)
                {
                    Console.WriteLine(_memory[i]);
                }
                else
                {
                    Console.Write(_memory[i] + " ");
                }

            }
        }
    }
}
