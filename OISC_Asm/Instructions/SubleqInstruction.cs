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
        public String Operand_c { get; private set; }

        public override int SourceAddressLength { get { return 3; } }
        public override long BinaryAddressLength { get { return (64*3)/8; } }

        public SubleqInstruction(String sourceLine, int sourceLineNumber, int sourceAddress, String operand_a, String operand_b, String operand_c)
            : this(sourceLine, sourceLineNumber, sourceAddress, String.Empty, operand_a, operand_b, operand_c)
        {

        }
        public SubleqInstruction(String sourceLine, int sourceLineNumber, int sourceAddress, String sourceLabel, String operand_a, String operand_b, String operand_c)
            : base(sourceLine, sourceLineNumber, sourceLabel, operand_c)
        {
            this.Operand_a = operand_a;
            this.Operand_b = operand_b;
            this.Operand_c = operand_c;

            this.SourceAddress = sourceAddress;
        }

        public override byte[] AssembleBinary()
        {
            // Parse the operand addresses into binary values.
            long op_a = long.Parse(Operand_a);
            long op_b = long.Parse(Operand_b);

            // We have to multiple the addresses by 24 because 
            // each source address represents a 64bit(8 byte) value.
            op_a *= 8;
            op_b *= 8;

            byte[] op_a_bin = BitConverter.GetBytes(op_a);
            byte[] op_b_bin = BitConverter.GetBytes(op_b);

            // Copy the binary into a single array for the instruction.
            byte[] instructionBinary = new byte[BinaryAddressLength];
            Array.Copy(op_a_bin, 0, instructionBinary, 0, 64 / 8);
            Array.Copy(op_b_bin, 0, instructionBinary, (64 / 8), 64 / 8);

            // Check for the branch destination address and copy the binary for that address too.
            if (this.BranchDestination != null)
            {
                byte[] op_c_bin = BitConverter.GetBytes(this.BranchDestination.BinaryAddress);
                Array.Copy(op_c_bin, 0, instructionBinary, (64 / 8) * 2, 64 / 8);
            }
            else 
            {
                byte[] terminate_bin = BitConverter.GetBytes((long)-1);
                Array.Copy(terminate_bin, 0, instructionBinary, (64 / 8) * 2, 64 / 8);
            }

            return instructionBinary;
        }
    }
}
