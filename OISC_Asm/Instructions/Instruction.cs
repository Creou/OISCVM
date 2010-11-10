using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public abstract class Instruction
    {
        public String SourceLine { get; private set; }
        public int SourceLineNumber { get; set; }

        public Instruction(String sourceLine, int sourceLineNumber)
        {
            this.SourceLine = sourceLine;
            this.SourceLineNumber = sourceLineNumber;
        }
    }
}
