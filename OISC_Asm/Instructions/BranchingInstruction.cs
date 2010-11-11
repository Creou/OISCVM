using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public static class LexicalSymbols
    {
        public  const String LabelAddress = "$";
        public const String Label = ":";
    }

    public abstract class BranchingInstruction : ExecutableInstruction, IBranchingInstruction
    {
        public int BranchSourceAddress { get; set; }
        public String BranchSourceLabel { get; set; }
        public ExecutableInstruction BranchDestination { get; set; }

        public BranchingInstruction(String sourceLine, int sourceLineNumber, String operand_branch)
            : this(sourceLine, sourceLineNumber, String.Empty, operand_branch)
        {
        }

        public BranchingInstruction(String sourceLine, int sourceLineNumber, String sourceLabel, String operand_branch)
            : base(sourceLine, sourceLineNumber, sourceLabel)
        {
            if (operand_branch.StartsWith(LexicalSymbols.LabelAddress))
            {
                this.BranchSourceLabel = operand_branch.Replace(LexicalSymbols.LabelAddress, String.Empty);
            }
            else
            {
                this.BranchSourceAddress = int.Parse(operand_branch);
            }
        }

        private void MapBranchAddress(ExecutableInstruction destinationInstruction)
        {
            this.BranchDestination = destinationInstruction;
        }

        public void MapBranchAddress(IDictionary<int, ExecutableInstruction> instructionDictionary, IDictionary<String, ExecutableInstruction> labeledInstructionDictionary)
        {
            if (!String.IsNullOrEmpty(this.BranchSourceLabel))
            {
                // If the branch is a label, resolve the labeled instruction...
                ExecutableInstruction destinationInstruction = labeledInstructionDictionary[this.BranchSourceLabel];
                this.MapBranchAddress(destinationInstruction);
            }
            else if (this.BranchSourceAddress != -1)
            {
                // ...Otherwise, resolve the destination from the branch source address.
                ExecutableInstruction destinationInstruction = instructionDictionary[this.BranchSourceAddress];
                this.MapBranchAddress(destinationInstruction);
            }

        }
    }
}
