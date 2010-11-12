using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public abstract class AddressableInstruction : Instruction
    {
        public abstract int SourceLength { get; }
        public abstract long BinaryLength { get; }

        public Address Address { get; private set; }


        public AddressableInstruction(String sourceLine, int sourceLineNumber, int sourceAddress)
            : this(sourceLine, sourceLineNumber, sourceAddress, String.Empty)
        {
        }

        public AddressableInstruction(String sourceLine, int sourceLineNumber, int sourceAddress, String sourceLabel)
            : base(sourceLine, sourceLineNumber)
        {
            this.Address = new Address(sourceAddress, sourceLabel);
        }

        public abstract byte[] AssembleBinary();
    }
}
