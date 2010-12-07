using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_VM
{
    public class InterruptEventArgs : EventArgs
    {
        public InterruptEventArgs(OISC_VM.SoftwareInterruptRequest irq)
        {
            this.InterruptRequest = irq;
        }
        public SoftwareInterruptRequest InterruptRequest{get;set;}


    }
}
