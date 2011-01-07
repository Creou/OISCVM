using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using OISC_VM.Extensions;

namespace OISC_VM
{
    public class Memory : IMemoryBus
    {
        // 1,048,576 = 1Mb.
        private long _memorySize = 1048576;
        private byte[] _memory;

        public event EventHandler<MemoryChangedEventArgs> MemoryChanged;

        public Memory()
        {
            _memory = new byte[_memorySize];
        }

        public long Size { get { return _memorySize; } }

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
            WriteData(memoryLocation, value, true);
        }

        private void WriteData(long memoryLocation, long value, bool notifyMemoryChanged)
        {
            byte[] valueToWrite = BitConverter.GetBytes(value);
            Array.Copy(valueToWrite, 0, _memory, memoryLocation, valueToWrite.Length);

            if (notifyMemoryChanged)
            {
                OnMemoryChanged(memoryLocation, value);
            }

            //Console.WriteLine("{0}: {1}", memoryLocation, value);
        }

        public void ResetData(long memoryLocation)
        {
            WriteData(memoryLocation, 0, false);
        }

        private void OnMemoryChanged(long memoryLocation, long value)
        {
            MemoryChanged.SafeTrigger(this, new MemoryChangedEventArgs(memoryLocation, value));
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
