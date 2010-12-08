using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_VM
{
    public interface IMemoryBus
    {
        long ReadData(long memoryLocation);
        void WriteData(long memoryLocation, long value);
        void ResetData(long memoryLocation);
        byte[] ReadDataRange(long rangeStart, long rangeLength);

        event EventHandler<MemoryChangedEventArgs> MemoryChanged;

        InstructionOperands FetchInstrucitonOperands(long memoryLocation);

        
    }
}
