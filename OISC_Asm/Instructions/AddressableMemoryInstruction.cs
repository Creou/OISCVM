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
        public override long BinaryLength { get { return 64 / 8; } }
        public override int SourceLength { get { return 1; } }
        public override byte[] AssembleBinary()
        {
            long initialValue = long.Parse(this.InitialValue);
            return BitConverter.GetBytes(initialValue);
        }

        public AddressableMemoryInstruction(String sourceLine, int sourceLineNumber, int sourceAddress, String initialValue)
            : this(sourceLine, sourceLineNumber, sourceAddress, String.Empty, initialValue)
        {
        }

        public AddressableMemoryInstruction(String sourceLine, int sourceLineNumber, int sourceAddress, String sourceLabel, String initialValue)
            : base(sourceLine, sourceLineNumber, sourceAddress, sourceLabel)
        {
            this.InitialValue = initialValue;
        }
    }
}
