using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler
{
    public interface ICompiler
    {
        byte[] Compile();
    }

    public class OISCCompiler :ICompiler
    {
        private String[] _sourceCodeLines;

        public OISCCompiler(String[] sourceCodeLines)
        {
            _sourceCodeLines = sourceCodeLines;
        }

        public byte[] Compile()
        {
            InstructionFactory instructionParser = new InstructionFactory();

            List<Instruction> sourceList = new List<Instruction>();
            Dictionary<int, SubleqInstruction> instructionDictionary = new Dictionary<int, SubleqInstruction>();
            int instructionSourceAddress = 0;

            foreach (String sourceLine in _sourceCodeLines)
            {
                Instruction sourceInstruction = instructionParser.GenerateInstruction(sourceLine, instructionSourceAddress);

                SubleqInstruction executableInstruction = sourceInstruction as SubleqInstruction;
                if (executableInstruction != null)
                {
                    instructionDictionary.Add(instructionSourceAddress, executableInstruction);
                }
                sourceList.Add(sourceInstruction);

                instructionSourceAddress += sourceInstruction.InstructionSourceAddressLength;
            }

            // Loop through each instruction and map each branch to the destination instruction.
            foreach (var instruction in instructionDictionary)
            {
                SubleqInstruction executableInstruction = instruction.Value;
                if (executableInstruction != null)
                {
                    if (executableInstruction.BranchSourceAddress != -1)
                    {
                        var destinationInstruction = instructionDictionary[executableInstruction.BranchSourceAddress] as SubleqInstruction;
                        executableInstruction.MapBranchAddress(destinationInstruction);
                    }
                }
            }

            return new byte[1];
        }
    }
}
