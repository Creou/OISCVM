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
        public Address ValueAddress { get; set; }
        public override long BinaryLength { get { return 64 / 8; } }
        public override int SourceLength { get { return 1; } }

        public bool IsValueAddress { get { return ValueAddress != null; } }

        public override byte[] AssembleBinary()
        {
            if (IsValueAddress) 
            {
                long initialValue = this.ValueAddress.BinaryAddress;
                return BitConverter.GetBytes(initialValue);
            }
            else
            {
                long initialValue = long.Parse(this.InitialValue);
                return BitConverter.GetBytes(initialValue);
            }
        }

        public AddressableMemoryInstruction(String sourceLine, int sourceLineNumber, int sourceAddress, String initialValue)
            : this(sourceLine, sourceLineNumber, sourceAddress, String.Empty, initialValue)
        {
        }

        public AddressableMemoryInstruction(String sourceLine, int sourceLineNumber, int sourceAddress, String sourceLabel, String initialValue)
            : base(sourceLine, sourceLineNumber, sourceAddress, sourceLabel)
        {
            this.InitialValue = initialValue;
            this.ValueAddress = null;
        }

        public AddressableMemoryInstruction(String sourceLine, int sourceLineNumber, int sourceAddress, String sourceLabel, Address valueAddress)
            : base(sourceLine, sourceLineNumber, sourceAddress, sourceLabel)
        {
            this.InitialValue = String.Empty;
            this.ValueAddress = valueAddress;
        }

        internal void MapMemoryValue(Dictionary<int, AddressableInstruction> instructionDictionary, Dictionary<string, AddressableInstruction> labeledInstructionDictionary)
        {
            if (IsValueAddress) 
            {
                if (this.ValueAddress.IsLabelledAddress)
                {
                    // If the branch is a label, resolve the labeled instruction...
                    AddressableInstruction valueLocation = labeledInstructionDictionary[this.ValueAddress.AddressLabel] as AddressableInstruction;
                    this.ValueAddress = valueLocation.Address;
                }
                else if (this.ValueAddress.SourceAddress != -1)
                {
                    // ...Otherwise, resolve the destination from the branch source address.
                    AddressableInstruction valueLocation = instructionDictionary[this.ValueAddress.SourceAddress] as AddressableInstruction;
                    this.ValueAddress = valueLocation.Address;
                }
            }
        }
    }
}
