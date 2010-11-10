using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{

    public abstract class ExecutableInstruction : Instruction
    {
        public abstract int SourceAddressLength { get; }
        public virtual int SourceAddress { get; protected set; }

        public ExecutableInstruction(String sourceLine, int sourceLineNumber)
            : base(sourceLine, sourceLineNumber)
        {
        }
    }
}
