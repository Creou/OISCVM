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

        private List<SoftwareInterruptRequest> _softwareIrqList;

        public event EventHandler<InterruptEventArgs> SoftwareInterruptTriggered;

        public Memory()
        {
            // 1,048,576 = 1Mb.
            _memory = new byte[1048576];
            _softwareIrqList = new List<SoftwareInterruptRequest>();
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

            // Check for any triggered interrupts.
            foreach (var irq in _softwareIrqList.Where(irq => irq.InterruptFlagAddress == memoryLocation))
            {
                OnSoftwareInterruptTriggered(irq);
            }
        }

        private void OnSoftwareInterruptTriggered(SoftwareInterruptRequest irq)
        {
            SoftwareInterruptTriggered.SafeTrigger(this, new InterruptEventArgs(irq));
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
