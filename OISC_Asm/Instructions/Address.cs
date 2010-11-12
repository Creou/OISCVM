using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public class Address
    {
        private String _sourceAddressCode;
        public int BinaryAddress { get; private set; }
        public int SourceAddress { get; private set; }
        public bool IsLabelledAddress { get; private set; }
        public String LabelledAddress { get; private set; }

        public Address(String sourceAddressCode)
        {
            _sourceAddressCode = sourceAddressCode;
            if (_sourceAddressCode.StartsWith(LexicalSymbols.LabelAddress))
            {
                this.IsLabelledAddress = true;
                this.LabelledAddress = _sourceAddressCode.Replace(LexicalSymbols.LabelAddress, String.Empty);
            }
            else if (_sourceAddressCode.StartsWith(LexicalSymbols.BinaryAddress))
            {
                this.BinaryAddress = int.Parse(_sourceAddressCode.Replace(LexicalSymbols.BinaryAddress, String.Empty));
                this.SourceAddress = (int)(this.BinaryAddress / 8);
            }
            else
            {
                this.SourceAddress = int.Parse(_sourceAddressCode);
                if (this.SourceAddress >= 0)
                {
                    this.BinaryAddress = this.SourceAddress * 8;
                }
                else
                {
                    this.BinaryAddress = this.SourceAddress;
                }
            }
        }
    }
}
