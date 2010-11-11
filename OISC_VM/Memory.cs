using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace OISC_VM
{
    public class Memory : IMemoryBus
    {
        // Create the memory.
        byte[] _memory;

        public Memory()
        {
            _memory = new byte[256];
        }

        public void LoadProgram(String fileName) 
        {
            LoadProgram(fileName, null);
        }
        public void LoadProgram(String fileName, IEnumerable<String> programArguments)
        {
            // Load the program into memory.
            byte[] programData = File.ReadAllBytes(fileName);
            Array.Copy(programData, _memory, programData.Length);

            // Load the args into memory.
            if (programArguments != null)
            {
                int argIndex = (64*3)/8;
                foreach (var arg in programArguments)
                {
                    long argValue = long.Parse(arg);
                    byte[] argBinary = BitConverter.GetBytes(argValue);

                    Array.Copy(argBinary, 0, _memory, argIndex, argBinary.Length);
                    argIndex += 64 / 8;
                }
            }
        }

        //public void LoadProgramSource(String fileName, IEnumerable<String> programArguments)
        //{
        //    // Load the program into memory.
        //    String[] lines = File.ReadAllLines(fileName);
        //    int memoryIndex = 0;
        //    foreach (var line in lines)
        //    {
        //        if (!line.StartsWith("//"))
        //        {
        //            String[] tokens = line.Split(' ');
        //            foreach (var token in tokens)
        //            {
        //                // Load each instruction into memory.
        //                _memory[memoryIndex] = int.Parse(token);
        //                memoryIndex++;
        //            }
        //        }
        //    }

        //    // Load the args into memory.
        //    if (programArguments != null)
        //    {
        //        int argIndex = 3;
        //        foreach (var arg in programArguments)
        //        {
        //            _memory[argIndex] = int.Parse(arg);
        //            argIndex++;
        //        }
        //    }
        //}


        public InstructionOperands FetchInstrucitonOperands(long memoryLocation)
        {
            //TODO_x64: Need to implement a bit converter that can processes 64 bit addresses.

            long operandA = BitConverter.ToInt64(_memory, (int)memoryLocation);
            long operandB = BitConverter.ToInt64(_memory, (int)memoryLocation+8);
            long operandC = BitConverter.ToInt64(_memory, (int)memoryLocation+16);

            return new InstructionOperands() { OperandA = operandA, OperandB = operandB, OperandC = operandC};
        }

        public byte[] ReadDataRange(long rangeStart, long rangeLength)
        {
            byte[] dataRange = new byte[rangeLength];
            Array.Copy(_memory, rangeStart, dataRange, 0, rangeLength);
            return dataRange;
        }
        
        public long ReadData(long memoryLocation)
        {
            //TODO_x64: Need to implement a bit converter that can processes 64 bit addresses.
            long dataValue = BitConverter.ToInt64(_memory, (int)memoryLocation);
            return dataValue;
        }

        public void WriteData(long memoryLocation, long value)
        {
            byte[] valueToWrite = BitConverter.GetBytes(value);
            Array.Copy(valueToWrite, 0, _memory, memoryLocation, valueToWrite.Length);
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
