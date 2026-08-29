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
    public class Zatvor : Device, INotifyPropertyChanged
    {
        public string Name { get; }
        public InputSignal<bool> IsOpen { get; }
        public InputSignal<bool> IsClose { get; }
        public OutputSignal<bool> Open { get; }
        public OutputSignal<bool>? Close { get; }
        public ICommand Command { get; }

        public Setting<int>? TimeForWarning;
        public Setting<int>? TimeForError;

        public override State State
        {
            get
            {

                if (IsOpen.Value && !IsClose.Value ||
                   IsOpen.Value && !IsClose.Value)
                    return State.On;

                else if (!IsOpen.Value && IsClose.Value ||
                         !IsOpen.Value && IsClose.Value)
                    return State.Off;

                else if (!Open.Value && !IsOpen.Value && !IsClose.Value ||
                         !Open.Value && IsOpen.Value && !IsClose.Value)
                    return State.Transition;

                else if (Open.Value && !IsOpen.Value && !IsClose.Value ||
                         Open.Value && !IsOpen.Value && IsClose.Value)
                    return State.Transition;

                else return State.Transition;
            }
            protected set
            {

            }
        }
        public Zatvor(string name, InputSignal<bool> isOpen, InputSignal<bool> isClose,
                    OutputSignal<bool> open, OutputSignal<bool> close, ICommand command)
        {
            TimeForError = CommonDeviceSettings.ValveTimeForError;
            TimeForWarning = CommonDeviceSettings.ValveTimeForWarning;
            Name = name;
            IsOpen = isOpen;
            IsClose = isClose;
            Open = open;
            Close = close;
            Command = command;


            if (IsOpen != null)
                IsOpen.OnSignalChanged += value => OnPropertyChanged(nameof(State));
            if (IsClose != null)
                IsClose.OnSignalChanged += value => OnPropertyChanged(nameof(State));
            if (Open != null)
                Open.OnSignalChanged += value => OnPropertyChanged(nameof(State));
            if (Close != null)
                Close.OnSignalChanged += value => OnPropertyChanged(nameof(State));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

