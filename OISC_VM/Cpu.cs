using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_VM
{
    public class SoftwareInterruptRequest
    {
        public SoftwareInterruptRequest(int priority, long jumpAddress, long interruptFlagAddress)
        {
            this.Priority = priority;
            this.JumpAddress = jumpAddress;
            this.InterruptFlagAddress = interruptFlagAddress;
        }

        public int Priority { get; set; }
        public long JumpAddress { get; set; }
        public long InterruptFlagAddress { get; set; }
    }

    public class CPU
    {
        Stack<long> _interruptReturnAddress;
        List<SoftwareInterruptRequest> _interruptJump;

        private long _pc;
        private SoftwareInterruptRequest _currentInterrupt;
        private InstructionOperands _instructionOperands;
        private long _aValue;
        private long _bValue;
        private long _cValue;

        private Memory _memoryBus;

        public CPU(Memory memoryBus)
        {
            _memoryBus = memoryBus;
            _memoryBus.SoftwareInterruptTriggered += new EventHandler<InterruptEventArgs>(_memoryBus_InterruptTriggered);

            _interruptReturnAddress = new Stack<long>();
            _interruptJump = new List<SoftwareInterruptRequest>();
        }

        public void Run()
        {
            while (_pc >= 0) 
            {
                Fetch();
                Decode();
                Execute();

                if (_interruptJump.Count > 0)
                {
                    SoftwareInterruptRequest request = _interruptJump.OrderBy(i=>i.Priority).FirstOrDefault();
                    if (request != null && (_currentInterrupt == null || (_currentInterrupt.Priority < request.Priority)))
                    {
                        PushExecutionStack();

                        _interruptJump.Remove(request);

                        _currentInterrupt = request;
                        _pc = request.JumpAddress;
                    }
                }

                if (_pc == -1 && _interruptReturnAddress.Count > 0) 
                {
                    PopExecutionStack();
                }
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

                if (newBValue <= 0)
                {
                    // And branch if less than or equal to zero.
                    _pc = _instructionOperands.OperandC;
                }
                else
                {
                    // Otherwise just increment the program counter to the next instruction.
                    _pc += (64 / 8) * 3;
                }

                // Write the result to memory.
                _memoryBus.WriteData(_instructionOperands.OperandB, newBValue);
            }
        }

        void _memoryBus_InterruptTriggered(object sender, InterruptEventArgs e)
        {
            _interruptJump.Add(e.InterruptRequest);
        }

        private void PushExecutionStack()
        {
            _interruptReturnAddress.Push(_pc);
            _pc = 0;
        }

        private void PopExecutionStack()
        {
            _pc = _interruptReturnAddress.Pop();
        }
    }
}
