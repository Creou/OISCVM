using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OISC_VM.Extensions;
using System.Threading;

namespace OISC_VM
{
    /*Interrupts:
     * There are two types of interrupts:
     * 
     * 1) Hardware interrupts:
     *      When triggered, hardware code within a device is run.
     *      
     * 2) Software interrupts:
     *      When triggered the interrupt jump address is added to an interrupt queue which is then processed by the CPU.
     *      The current context is popped and the program counter jumps to the interrupts jump address and the software is run.
     *      
     * Both types of interrupt are triggered by a memory write to a flag address.
     * Both types of interrupt can only be created by the hardware interrupt handler.
     * The memory write can originate from either software or hardware.
     * 
     * Software interrupt queues (SIQ):
     *      This is a hardware interrupt, and when triggered it creates a software interrupt for the specified address.
     *      
     *      For example: 
     *      The keyboard device creates an SIQ at queue address X for flag address Y. 
     *      Software then writes address Z into the queueing address X.
     *      This triggers the hardware interrupt that creates a new software interrupt triggered on address Y that jumps to address Z.
     *      
     *      Following this pattern, multiple software jump addresses can be assigned (by software) to the single flag address.
     *      SIQs allow software to subscribe to software interrupts without the creation code to be included in the hardware interrupt handler.
     *      
     * For the purposes of this code/explanation "Hardware" refers to code within the OISCVM and "Software" refers to OISC binaries running on the VM.
     */
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

        public string GetIrqList()
        {
            StringBuilder irqList = new StringBuilder();
            irqList.AppendLine("Interrupts");
            irqList.AppendLine("==========================================================");
            irqList.AppendFormat("{0} {1} {2} {3} {4}\n", "Name".PadRight(30), "Address".PadRight(8), "Trig?".PadRight(5), "Trig".PadRight(5), "Reset?");
            foreach (HardwareInterruptRequest hIrq in _hardwareIrqList)
            {
                irqList.AppendFormat("{0} {1} {2} {3} {4}\n", hIrq.Name.PadRight(30), hIrq.InterruptFlagAddress.ToString().PadRight(8), hIrq.SpecificTrigger.ToString().PadRight(5), hIrq.TriggerValue.ToString().PadRight(5), hIrq.AutoReset);
            }

            if (_softwareIrqList.Count > 0)
            {
                irqList.AppendLine();
                foreach (SoftwareInterruptRequest sIrq in _softwareIrqList)
                {
                    irqList.AppendFormat("{0}\n", sIrq.Name.PadRight(30), sIrq.InterruptFlagAddress.ToString().PadRight(8), sIrq.JumpAddress.ToString().PadRight(8), sIrq.Priority);
                }
            }
            irqList.AppendLine("==========================================================");

            return irqList.ToString();
        }

        public void RegisterHardwareInterrupt(String name, long interruptFlagAddress, bool autoReset, Action<HardwareInterruptRequest, long, long> interrupt)
        {
            _hardwareIrqList.Add(new HardwareInterruptRequest(name, interruptFlagAddress, autoReset, interrupt));
        }

        public void RegisterHardwareInterrupt(String name, long interruptFlagAddress, long triggerValue, bool autoReset, Action<HardwareInterruptRequest, long, long> interrupt)
        {
            _hardwareIrqList.Add(new HardwareInterruptRequest(name, interruptFlagAddress, triggerValue, autoReset, interrupt));
        }

        public void RegisterSoftwareInterruptQueue(String interruptQueueName, String interruptName, long interruptQueueingAddress, long interruptFlagAddress)
        {
            InterruptQueueingHandler queue = new InterruptQueueingHandler(this, interruptQueueName, interruptName, interruptQueueingAddress, interruptFlagAddress);
        }

        public void RegisterSoftwareInterrupt(String name, long interruptFlagAddress, long jumpAddress)
        {
            _softwareIrqList.Add(new SoftwareInterruptRequest(name, 0, jumpAddress, interruptFlagAddress));
        }

        private void memoryNotifyer_MemoryChanged(object sender, MemoryChangedEventArgs e)
        {
            // Check for any triggered hardware interrupts.
            foreach (var hardwareIrq in _hardwareIrqList.Where(irq => irq.InterruptFlagAddress == e.MemoryLocation))
            {
                if (!hardwareIrq.SpecificTrigger || e.NewValue == hardwareIrq.TriggerValue)
                {
                    if (hardwareIrq.AutoReset)
                    {
                        _memoryBus.ResetData(hardwareIrq.InterruptFlagAddress);
                    }
                    Task.Factory.StartNew(() => { hardwareIrq.Interrupt(hardwareIrq, e.MemoryLocation, e.NewValue); });
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
