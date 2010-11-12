using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public abstract class BranchingInstruction : ExecutableInstruction, IBranchingInstruction
    {
        public Address BranchAddress { get; set; }
        public ExecutableInstruction BranchDestination { get; set; }
        public bool AutoBranchNext { get; private set; }

        public BranchingInstruction(String sourceLine, int sourceLineNumber, int sourceAddress, Address operand_branch)
            : this(sourceLine, sourceLineNumber, sourceAddress, String.Empty, operand_branch, false)
        {
        }

        public BranchingInstruction(String sourceLine, int sourceLineNumber, int sourceAddress, String sourceLabel, Address operand_branch, bool autoBranchNext)
            : base(sourceLine, sourceLineNumber, sourceAddress, sourceLabel)
        {
            this.AutoBranchNext = autoBranchNext;
            this.BranchAddress = operand_branch;
        }

        private void MapBranchAddress(ExecutableInstruction destinationInstruction)
        {
            this.BranchDestination = destinationInstruction;
        }

        public void MapBranchAddress(IDictionary<int, AddressableInstruction> instructionDictionary, IDictionary<String, AddressableInstruction> labeledInstructionDictionary)
        {
            if (this.AutoBranchNext)
            {
                // Auto branch instructions just need to branch to the next instruction in the source code.
                int nextInstructionSourceAddress = this.Address.SourceAddress + this.SourceLength;
                ExecutableInstruction destinationInstruction = instructionDictionary[nextInstructionSourceAddress] as ExecutableInstruction;
                this.MapBranchAddress(destinationInstruction);
            }
            else
            {
                if (this.BranchAddress.IsLabelledAddress)
                {
                    // If the branch is a label, resolve the labeled instruction...
                    ExecutableInstruction destinationInstruction = labeledInstructionDictionary[this.BranchAddress.AddressLabel] as ExecutableInstruction;
                    this.MapBranchAddress(destinationInstruction);
                }
                else if (this.BranchAddress.SourceAddress != -1)
                {
                    // ...Otherwise, resolve the destination from the branch source address.
                    ExecutableInstruction destinationInstruction = instructionDictionary[this.BranchAddress.SourceAddress] as ExecutableInstruction;
                    this.MapBranchAddress(destinationInstruction);
                }
            }
        }
    }
}
