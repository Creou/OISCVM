using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OISC_Compiler.Instructions;

namespace OISC_Compiler.Instructions
{
    public class AddressableMemoryInstruction : AddressableInstruction
    {
        public String InitialValue { get; private set; }
        public override long BinaryAddressLength { get { return 64 / 8; } }
        public override int SourceAddressLength { get { return 1; } }
        public override byte[] AssembleBinary()
        {
            long initialValue = long.Parse(this.InitialValue);
            return BitConverter.GetBytes(initialValue);
        }

        public AddressableMemoryInstruction(String sourceLine, int sourceLineNumber, String initialValue)
            : this(sourceLine, sourceLineNumber, String.Empty, initialValue)
        {
        }

        public AddressableMemoryInstruction(String sourceLine, int sourceLineNumber, String sourceLabel, String initialValue)
            : base(sourceLine, sourceLineNumber, sourceLabel)
        {
            this.InitialValue = initialValue;
        }
    }
}
