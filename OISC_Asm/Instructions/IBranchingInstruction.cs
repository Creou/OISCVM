using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public interface IBranchingInstruction
    {
        int BranchSourceAddress { get; set; }
        ExecutableInstruction BranchDestination { get; set; }

        void MapBranchAddress(IDictionary<int, ExecutableInstruction> instructionDictionary);
    }
}
