using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_VM
{
    public class MemoryChangedEventArgs : EventArgs
    {
        public long MemoryLocation { get; private set; }
        public long NewValue { get; private set; }

        public MemoryChangedEventArgs(long memoryLocation, long newValue)
        {
            this.MemoryLocation = memoryLocation;
            this.NewValue = newValue;
        }
    }
}
