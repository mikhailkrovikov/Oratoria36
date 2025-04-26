using Oratoria36.Service.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Devices
{
    public class Pipeline : Device, INotifyPropertyChanged
    {
        private Pipeline _pipelineBefore;
        private Device _deviceToPipeline;
        public override State State
        {
            get
            {
                if (_deviceToPipeline.State == State.Off)
                    return State.Off;

                if (_pipelineBefore != null)
                {
                    if (_pipelineBefore.State == State.Error || _pipelineBefore.State == State.Warning)
                        return _pipelineBefore.State;
                    return _deviceToPipeline.State;
                }
                return _deviceToPipeline.State;
            }
        }
        public Pipeline(Device deviceToPipeline, Pipeline lineBefore = null)
        {

            _deviceToPipeline = deviceToPipeline; 
            _pipelineBefore = lineBefore;

            if (deviceToPipeline.Off != null)
                deviceToPipeline.Off.OnSignalChanged += value => { OnPropertyChanged(nameof(State)); };
            
            if(deviceToPipeline.On != null)   
                deviceToPipeline.On.OnSignalChanged += value => { OnPropertyChanged(nameof(State)); };
            
            if (deviceToPipeline.IsOn != null) 
                deviceToPipeline.IsOn.OnSignalChanged += value => { OnPropertyChanged(nameof(State)); };
            
            if (deviceToPipeline.IsOff != null)            
                deviceToPipeline.IsOff.OnSignalChanged += value => { OnPropertyChanged(nameof(State)); };          
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
