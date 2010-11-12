using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public abstract class ExecutableInstruction : AddressableInstruction
    {
        public ExecutableInstruction(String sourceLine, int sourceLineNumber, int sourceAddress)
            : this(sourceLine, sourceLineNumber, sourceAddress, String.Empty)
        {
        }

        public ExecutableInstruction(String sourceLine, int sourceLineNumber, int sourceAddress, String sourceLabel)
            : base(sourceLine, sourceLineNumber, sourceAddress, sourceLabel)
        {
        }
    }
}
