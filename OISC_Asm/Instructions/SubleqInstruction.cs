using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public class SubleqInstruction : BranchingInstruction
    {
        public String Operand_a { get; private set; }
        public String Operand_b { get; private set; }
        public String Opernad_c { get; private set; }

        public override int SourceAddressLength { get { return 3; } }

        public SubleqInstruction(String sourceLine, int sourceLineNumber, int sourceAddress, String operand_a, String operand_b, String operand_c)
            : base(sourceLine, sourceLineNumber, operand_c)
        {
            this.Operand_a = operand_a;
            this.Operand_b = operand_b;
            this.Opernad_c = operand_c;

            this.SourceAddress = sourceAddress;
        }
    }
}
