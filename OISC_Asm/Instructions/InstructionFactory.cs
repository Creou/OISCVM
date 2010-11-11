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
                String[] instructionDate = sourceLine.Trim().Split(' ');

                String sourceLabel = String.Empty;
                String operand_a = String.Empty;
                String operand_b = String.Empty;
                String operand_c = String.Empty;

                if (instructionDate[0].EndsWith(LexicalSymbols.Label))
                {
                    sourceLabel = instructionDate[0].Replace(LexicalSymbols.Label, String.Empty);
                    operand_a = instructionDate[1];
                    operand_b = instructionDate[2];
                    operand_c = instructionDate[3];

                }
                else
                {
                    operand_a = instructionDate[0];
                    operand_b = instructionDate[1];
                    operand_c = instructionDate[2];
                }
                return new SubleqInstruction(sourceLine, sourceLineNumber, sourceAddress, sourceLabel, operand_a, operand_b, operand_c);
            }
        }
    }
}
