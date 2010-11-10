using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public class InstructionFactory
    {
        public Instruction GenerateInstruction(String sourceLine, int sourceLineNumber, int sourceAddress)
        {
            if (sourceLine.StartsWith("//"))
            {
                // This line is a comment.
                return new CommentInstruction(sourceLine, sourceLineNumber);
            }
            else
            {
                String[] instructionDate = sourceLine.Split(' ');
                String operand_a = instructionDate[0];
                String operand_b = instructionDate[1];
                String operand_c = instructionDate[2];
                return new SubleqInstruction(sourceLine, sourceLineNumber, sourceAddress, operand_a, operand_b, operand_c);
            }
        }
    }
}
