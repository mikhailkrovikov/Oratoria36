using Oratoria36.Models.Settings;
using Oratoria36.Models.Signals;
using Oratoria36.Service.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Oratoria36.Models.Devices
{
    public class RRG : Device, INotifyPropertyChanged
    {
        public InputSignal<ushort> RRGRealValue { get; }
        public OutputSignal<ushort> RRGSetPoint { get; }
        public Setting<ushort> RRGDifference { get; }
        public ICommand Command { get; }
        public override State State
        {
            get
            {
                if (RRGSetPoint.Value - RRGRealValue.Value >= RRGDifference.Value ||
                    RRGRealValue.Value - RRGSetPoint.Value >= RRGDifference.Value)
                    return State.Warning;

                else if (RRGSetPoint.Value - RRGRealValue.Value >= RRGDifference.Value * 2 ||
                    RRGRealValue.Value - RRGSetPoint.Value >= RRGDifference.Value * 2)
                    return State.Error;

                else if (RRGSetPoint.Value - RRGRealValue.Value < RRGDifference.Value &&
                    RRGRealValue.Value == 0)
                    return State.Off;

                else if (RRGSetPoint.Value - RRGRealValue.Value < RRGDifference.Value &&
                    RRGRealValue.Value != 0)
                    return State.On;

                else return State.Transition;
            }
        }
        public RRG(InputSignal<ushort> rrgRealValue, OutputSignal<ushort> rrgSetPoint, ICommand command)
        {
            RRGRealValue = rrgRealValue;
            RRGSetPoint = rrgSetPoint;
            Command = command;
            RRGDifference = CommonDeviceSettings.RRGDifference;

            if(RRGRealValue!=null)
                RRGRealValue.OnSignalChanged += value => { OnPropertyChanged(nameof(State)); };

            if (RRGSetPoint != null)
                RRGSetPoint.OnSignalChanged += value => { OnPropertyChanged(nameof(State)); };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
