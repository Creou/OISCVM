using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public class Address
    {
        public long BinaryAddress { get; private set; }
        public int SourceAddress { get; private set; }
        public bool IsLabelledAddress { get; private set; }
        public String AddressLabel { get; private set; }

        public Address(int sourceAddress, String sourceLabel) 
        {
            this.SourceAddress = sourceAddress;
            this.BinaryAddress = SourceToBinary(sourceAddress);
            this.AddressLabel = sourceLabel;
        }

        public Address(String sourceAddressCode)
        {
            if (sourceAddressCode.StartsWith(LexicalSymbols.LabelAddress))
            {
                this.IsLabelledAddress = true;
                this.AddressLabel = sourceAddressCode.Replace(LexicalSymbols.LabelAddress, String.Empty);
            }
            else if (sourceAddressCode.StartsWith(LexicalSymbols.BinaryAddress))
            {
                this.BinaryAddress = int.Parse(sourceAddressCode.Replace(LexicalSymbols.BinaryAddress, String.Empty));
                this.SourceAddress = BinaryToSource(this.BinaryAddress);
            }
            else
            {
                this.SourceAddress = int.Parse(sourceAddressCode);
                if (this.SourceAddress >= 0)
                {
                    this.BinaryAddress = SourceToBinary(this.SourceAddress);
                }
                else
                {
                    this.BinaryAddress = this.SourceAddress;
                }
            }
        }


        private long SourceToBinary(int sourceAddress)
        {
            return sourceAddress * 8;
        }

        private int BinaryToSource(long binaryAddress)
        {
            return (int)(binaryAddress / 8);
        }
    }
}
