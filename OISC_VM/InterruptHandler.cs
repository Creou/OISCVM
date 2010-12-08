using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OISC_VM.Extensions;

namespace OISC_VM
{
    public class InterruptHandler
    {
        private IMemoryBus _memoryBus;

        private List<HardwareInterruptRequest> _hardwareIrqList;
        private List<SoftwareInterruptRequest> _softwareIrqList;

        public event EventHandler<InterruptEventArgs> SoftwareInterruptTriggered;

        public InterruptHandler(IMemoryBus memoryBus)
        {
            _hardwareIrqList = new List<HardwareInterruptRequest>();
            _softwareIrqList = new List<SoftwareInterruptRequest>();

            _memoryBus = memoryBus;
            _memoryBus.MemoryChanged += new EventHandler<MemoryChangedEventArgs>(memoryNotifyer_MemoryChanged);
        }

        public void RegisterHardwareInterrupt(long interruptFlagAddress, byte triggerValue, bool autoReset, Action<HardwareInterruptRequest> interrupt)
        {
            _hardwareIrqList.Add(new HardwareInterruptRequest(interruptFlagAddress, triggerValue, autoReset, interrupt));
        }

        private void memoryNotifyer_MemoryChanged(object sender, MemoryChangedEventArgs e)
        {
            // Check for any triggered hardware interrupts.
            foreach (var hardwareIrq in _hardwareIrqList.Where(irq => irq.InterruptFlagAddress == e.MemoryLocation))
            {
                if (e.NewValue == hardwareIrq.TriggerValue)
                {
                    if (hardwareIrq.AutoReset)
                    {
                        _memoryBus.ResetData(hardwareIrq.InterruptFlagAddress);
                    }
                    Task.Factory.StartNew(() => { hardwareIrq.Interrupt(hardwareIrq); });
                }
            }

            // Check for any triggered software interrupts.
            foreach (var irq in _softwareIrqList.Where(irq => irq.InterruptFlagAddress == e.MemoryLocation))
            {
                OnSoftwareInterruptTriggered(irq);
            }
        }

        private void OnSoftwareInterruptTriggered(SoftwareInterruptRequest irq)
        {
            SoftwareInterruptTriggered.SafeTrigger(this, new InterruptEventArgs(irq));
        }
    }
}
