using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public class CommentInstruction : Instruction
    {
        public CommentInstruction(String sourceLine, int sourceLineNumber)
            : base(sourceLine, sourceLineNumber)
        {
        }
    }
}
