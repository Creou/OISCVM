using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{

    public abstract class ExecutableInstruction : Instruction
    {
        public abstract int SourceAddressLength { get; }
        public int SourceAddress { get; protected set; }

        public long BinaryAddress { get; set; }
        public abstract long BinaryAddressLength { get; }

        public ExecutableInstruction(String sourceLine, int sourceLineNumber)
            : base(sourceLine, sourceLineNumber)
        {
        }

        internal void SetBinaryAddress(long binaryAddress)
        {
            this.BinaryAddress = binaryAddress;
        }

        public abstract byte[] AssembleBinary();
    }
}
