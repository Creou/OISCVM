using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler
{
    public enum InstructionType
    {
        Comment = 0,
        SUBLEQ = 1
    }

    public abstract class Instruction
    {
        public String SourceLine{get;private set;}

        public Instruction(String sourceLine)
        {
            this.SourceLine = sourceLine;
        }

    }

    public class CommentInstruction : Instruction
    {
        public CommentInstruction(String sourceLine)
            : base(sourceLine)
        {
        }
    }

    public class SubleqInstruction : Instruction
    {
        public String Operand_a { get; private set; }
        public String Operand_b { get; private set; }
        public String Opernad_c { get; private set; }

        public SubleqInstruction(String sourceLine, String operand_a, String operand_b, String operand_c)
            : base(sourceLine)
        {
            this.Operand_a = operand_a;
            this.Operand_b = operand_b;
            this.Opernad_c = operand_c;
        }
    }
}
