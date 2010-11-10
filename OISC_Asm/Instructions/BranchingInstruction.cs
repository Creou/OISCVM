using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{


    public abstract class BranchingInstruction : ExecutableInstruction, IBranchingInstruction
    {
        public int BranchSourceAddress { get; set; }
        public ExecutableInstruction BranchDestination { get; set; }

        public BranchingInstruction(String sourceLine, int sourceLineNumber, String operand_branch)
            : base(sourceLine, sourceLineNumber)
        {
            this.BranchSourceAddress = int.Parse(operand_branch);
        }

        private void MapBranchAddress(ExecutableInstruction destinationInstruction)
        {
            this.BranchDestination = destinationInstruction;
        }

        public void MapBranchAddress(IDictionary<int, ExecutableInstruction> instructionDictionary)
        {
            if (this.BranchSourceAddress != -1)
            {
                ExecutableInstruction destinationInstruction = instructionDictionary[this.BranchSourceAddress];
                this.MapBranchAddress(destinationInstruction);
            }
        }
    }
}
