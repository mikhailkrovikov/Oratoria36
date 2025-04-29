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
        public InputSignal<ushort> RRGRealValueSignal { get; }
        public OutputSignal<ushort> RRGSetPointSignal { get; }
        public double RRGRealValue
        {
            get => RRGRealValueSignal.Value;
        }
        public double RRGSetPointValue
        {
            get => RRGSetPointSignal.Value / 10.0;
        }
        public Setting<ushort> RRGDifference { get; }
        public ICommand Command { get; }
        public override State State
        {
            get
            {
                if (RRGSetPointSignal.Value - RRGRealValueSignal.Value >= RRGDifference.Value ||
                    RRGRealValueSignal.Value - RRGSetPointSignal.Value >= RRGDifference.Value)
                    return State.Warning;

                else if (RRGSetPointSignal.Value - RRGRealValueSignal.Value >= RRGDifference.Value * 2 ||
                    RRGRealValueSignal.Value - RRGSetPointSignal.Value >= RRGDifference.Value * 2)
                    return State.Error;

                else if (RRGSetPointSignal.Value - RRGRealValueSignal.Value < RRGDifference.Value &&
                    RRGRealValueSignal.Value == 0)
                    return State.Off;

                else if (RRGSetPointSignal.Value - RRGRealValueSignal.Value < RRGDifference.Value &&
                    RRGRealValueSignal.Value != 0)
                    return State.On;

                else return State.Transition;
            }
        }
        public RRG(InputSignal<ushort> rrgRealValue, OutputSignal<ushort> rrgSetPoint, ICommand command)
        {
            RRGRealValueSignal = rrgRealValue;
            RRGSetPointSignal = rrgSetPoint;
            Command = command;
            RRGDifference = CommonDeviceSettings.RRGDifference;

            if (RRGRealValueSignal != null)
                RRGRealValueSignal.OnSignalChanged += value =>
                {
                    OnPropertyChanged(nameof(State));
                    OnPropertyChanged(nameof(RRGRealValue));
                };

            if (RRGSetPointSignal != null)
                RRGSetPointSignal.OnSignalChanged += value =>
                {
                    OnPropertyChanged(nameof(State));
                    OnPropertyChanged(nameof(RRGSetPointValue));
                };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
