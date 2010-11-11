using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_VM
{

    public class CPU
    {
        private long _pc;
        private InstructionOperands _instructionOperands;
        private long _aValue;
        private long _bValue;
        private long _cValue;

        private IMemoryBus _memoryBus;

        public CPU(IMemoryBus memoryBus)
        {
            _memoryBus = memoryBus;
        }

        public void Run()
        {
            while (_pc >= 0) 
            {
                Fetch();
                Decode();
                Execute();
            }
        }

        private void Fetch() 
        {
            // Load the operand addresses and their values from memory.
            _instructionOperands = _memoryBus.FetchInstrucitonOperands(_pc);
            _aValue = _memoryBus.ReadData(_instructionOperands.OperandA);
            _bValue = _memoryBus.ReadData(_instructionOperands.OperandB);
        }

        private void Decode() { }
        private void Execute()
        {
            // Check operands a & b for negative values (invalid memory addresses).
            if (_instructionOperands.OperandA < 0 || _instructionOperands.OperandB < 0)
            {
               _pc = -1;
            }
            else
            {
                // Subract
                long newBValue = _bValue - _aValue;
                _memoryBus.WriteData(_instructionOperands.OperandB, newBValue);

                if (newBValue <= 0)
                {
                    // And branch if less than or equal to zero.
                    _pc = _instructionOperands.OperandC;
                }
                else
                {
                    // Otherwise just increment the program counter to the next instruction.
                    _pc += (64/8)*3;
                }
            }
        }
    }
}
