using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public abstract class ExecutableInstruction : AddressableInstruction
    {
        public ExecutableInstruction(String sourceLine, int sourceLineNumber)
            : this(sourceLine, sourceLineNumber, String.Empty)
        {
        }

        public ExecutableInstruction(String sourceLine, int sourceLineNumber, String sourceLabel)
            : base(sourceLine, sourceLineNumber, sourceLabel)
        {
        }
    }
}
