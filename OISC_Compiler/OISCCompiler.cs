using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler
{
    public interface ICompiler
    {
        byte[] Compile();
    }

    public class OISCCompiler :ICompiler
    {
        private String[] _sourceCodeLines;

        public OISCCompiler(String[] sourceCodeLines)
        {
            _sourceCodeLines = sourceCodeLines;
        }

        public byte[] Compile()
        {
            List<Instruction> instructionList = new List<Instruction>();
            foreach (String sourceLine in _sourceCodeLines)
            {
                Instruction sourceInstruction = GenerateInstruction(sourceLine);
                instructionList.Add(sourceInstruction);
            }

            return new byte[1];
        }

        private Instruction GenerateInstruction(String sourceLine)
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
                return new SubleqInstruction(sourceLine, operand_a, operand_b, operand_c);
            }
        }
    }
}
