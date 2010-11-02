using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_VM
{

    public interface IMemory
    {
        InstructionOperands FetchInstrucitonOperands(int memoryLocation);
        int ReadData(int memoryLocation);
        void WriteData(int memoryLocation, int value);
        int[] ReadDataRange(int rangeStart, int rangeLength);
    }

}
