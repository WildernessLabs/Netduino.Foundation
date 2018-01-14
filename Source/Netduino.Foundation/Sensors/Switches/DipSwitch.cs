using System;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Switches
{
    /// <summary>
    /// Represents a DIP-switch wired in a bus configuration, in which all switches 
    /// are terminated to the same ground/common or high pin.
    /// 
    /// Note: this is untested, as I don't have a dip switches at the moment :D
    /// </summary>
    public class DipSwitch
    {
        public event ArrayEventHandler Changed = delegate { };

        public ISwitch this[int i]
        {
            get
            {
                return _switches[i];
            }
            //protected set;
        } public ISwitch[] _switches = null;

        public DipSwitch(H.Cpu.Pin[] switchPins, CircuitTerminationType type)
        {
            // if we terminate in ground, we need to pull the port high to test for circuit completion, otherwise down.
            var resistorMode = (type == CircuitTerminationType.CommonGround) ? H.Port.ResistorMode.PullUp : H.Port.ResistorMode.PullDown;

            //this.DigitalIns = new H.InterruptPort[switchPins.Length];            
            //this.IsOn = new bool[switchPins.Length];
            this._switches = new ISwitch[switchPins.Length];

            for (int i = 0; i < switchPins.Length; i++)
            {
                //this.DigitalIns[i] = new H.InterruptPort(switchPins[i], true, resistorMode, Microsoft.SPOT.Hardware.Port.InterruptMode.InterruptEdgeBoth);
                this._switches[i] = new SpstSwitch(switchPins[i], type);

                // capture the variable. oh, C#...
                int iCopy = i;

                this._switches[i].Changed += (s,e) =>
                {
                    this.HandleSwitchChange(iCopy);
                };
            }
        }

        protected void HandleSwitchChange(int switchNumber)
        {
            Debug.Print("HandleSwitchChange: " + switchNumber.ToString() + ", total switches: " + (_switches.Length).ToString());
            this.Changed(this, new ArrayEventArgs(switchNumber, this._switches[switchNumber]));
        }
    }
}
