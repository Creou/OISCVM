using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_VM
{

    public class CPU
    {
        private int _pc;
        private InstructionOperands _instructionOperands;
        private int _aValue;
        private int _bValue;
        private int _cValue;

        private IMemory _memoryBus;

        public CPU(IMemory memoryBus)
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
                int newBValue = _bValue - _aValue;
                _memoryBus.WriteData(_instructionOperands.OperandB, newBValue);

                if (newBValue <= 0)
                {
                    // And branch if less than or equal to zero.
                    _pc = _instructionOperands.OperandC;
                }
                else
                {
                    // Otherwise just increment the program counter to the next instruction.
                    _pc += 3;
                }
            }
        }
    }
}
