using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public interface IBranchingInstruction
    {
        Address BranchAddress { get; set; }
        ExecutableInstruction BranchDestination { get; set; }

        void MapBranchAddress(IDictionary<int, AddressableInstruction> instructionDictionary, IDictionary<String, AddressableInstruction> labeledInstructionDictionary);
    }
}
