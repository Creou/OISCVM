using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_VM
{

    public interface IMemoryMappedDevice 
    {
         int MemoryRangeStart { get; }
         int MemoryRangeLength { get; }
    }
}
