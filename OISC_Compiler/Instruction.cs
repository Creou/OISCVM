using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyInt = System.Int64;

namespace OISC_Compiler
{
    public enum InstructionType
    {
        Comment = 0,
        SUBLEQ = 1
    }

    public class InstructionFactory 
    {
        public Instruction GenerateInstruction(String sourceLine, int instructionSourceAddress)
        {
            if (sourceLine.StartsWith("//"))
            {
                // This line is a comment.
                return new CommentInstruction(sourceLine);
            }
            else
            {
                String[] instructionDate = sourceLine.Split(' ');
                String operand_a = instructionDate[0];
                String operand_b = instructionDate[1];
                String operand_c = instructionDate[2];
                return new SubleqInstruction(sourceLine, instructionSourceAddress, operand_a, operand_b, operand_c);
            }
        }
    }

    public abstract class Instruction
    {
        public String SourceLine{get;private set;}

        public Instruction(String sourceLine)
        {
            this.SourceLine = sourceLine;
        }

        public abstract int InstructionSourceAddressLength { get; }

        public virtual int InstructionSourceAddress { get; protected set; }
    }

    public class CommentInstruction : Instruction
    {
        public CommentInstruction(String sourceLine)
            : base(sourceLine)
        {
        }

        public override int InstructionSourceAddressLength
        {
            get { return 0; }
        }

        public override int InstructionSourceAddress
        {
            get
            {
                return -1;
            }
            protected set
            {
                throw new NotSupportedException("Comment instructions do not support source addresses");
            }
        }
    }

    public class SubleqInstruction : Instruction
    {
        public String Operand_a { get; private set; }
        public String Operand_b { get; private set; }
        public String Opernad_c { get; private set; }

        public int BranchSourceAddress { get; set; }
        public SubleqInstruction BranchDestination { get; set; }

        public override int InstructionSourceAddressLength { get { return 3; } }

        public SubleqInstruction(String sourceLine, int instructionSourceAddress, String operand_a, String operand_b, String operand_c)
            : base(sourceLine)
        {
            this.Operand_a = operand_a;
            this.Operand_b = operand_b;
            this.Opernad_c = operand_c;

            this.BranchSourceAddress = int.Parse(this.Opernad_c);

            this.InstructionSourceAddress = instructionSourceAddress;
        }

        public void MapBranchAddress(SubleqInstruction destinationInstruction)
        {
            this.BranchDestination = destinationInstruction;
        }
    }
}
