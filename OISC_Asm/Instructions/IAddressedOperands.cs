using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public interface IAddressedOperands
    {
        void MapAddressedOperands(Dictionary<int, AddressableInstruction> instructionDictionary, Dictionary<string, AddressableInstruction> labeledInstructionDictionary);
    }
}
