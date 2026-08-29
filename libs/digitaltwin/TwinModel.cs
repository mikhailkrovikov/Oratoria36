using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin
{
    public class TwinModel : IRegister
    {
        private Dictionary<ushort, bool> _boolInputs = new();
        private Dictionary<ushort, double> _doubleInputs = new();
        private Dictionary<ushort, Action<bool>> _boolHandlers = new();
        private Dictionary<ushort, Action<double>> _doubleHandlers = new();

        public event Action<ushort, bool> BoolInputChanged;
        public event Action<ushort, double> DoubleInputChanged;

        public virtual void RegisterDevice<T>(ushort outputPin, ushort inputPin, int timeout)
        {
            if (typeof(T) == typeof(bool))
            {
                _boolHandlers[outputPin] = async (bool value) =>
                {
                    if (value)
                    {
                        SetBoolInput(inputPin, false);
                        await Task.Delay(timeout);
                        SetBoolInput(inputPin, true);
                    }
                    else
                    {
                        await Task.Delay(timeout);
                        SetBoolInput(inputPin, false);
                    }
                };
            }
            else if (typeof(T) == typeof(double))
            {
                _doubleHandlers[outputPin] = (double value) =>
                {
                    SetDoubleInput(inputPin, value);
                };
            }
        }

        public virtual void RegisterDevice<T>(ushort outputPin, ushort inputPin1, ushort inputPin2, int timeout)
        {
            if (typeof(T) == typeof(bool))
            {
                _boolHandlers[outputPin] = async (bool value) =>
                {
                    if (value)
                    {
                        SetBoolInput(inputPin1, false);
                        SetBoolInput(inputPin2, false);
                        await Task.Delay(timeout);
                        SetBoolInput(inputPin1, true);
                        SetBoolInput(inputPin2, false);
                    }
                    else
                    {
                        SetBoolInput(inputPin1, false);
                        SetBoolInput(inputPin2, false);
                        await Task.Delay(timeout);
                        SetBoolInput(inputPin1, false);
                        SetBoolInput(inputPin2, true);
                    }
                };
            }
        }

        public virtual void RegisterMechanicDevice<T>(ushort[] outputs, ushort[] inputs, int timeout)
        {
            if (outputs.Length > 6 && inputs.Length > 5)
                return;

            var pos1out = outputs[0];
            var pos2out = outputs[1];
            var pos3out = outputs[2];
            var torOut = outputs[3];
            var revOut = outputs[4];
            var act = outputs[5];

            var pos1in = inputs[0];
            var pos2in = inputs[1];
            var pos3in = inputs[2];
            var torIn = inputs[3];
            var revIn = inputs[4];



            //for (int i = 0; i < 4; i++)
            //{
            //    _boolHandlers[outputs[i]] = async (bool value) =>
            //    {
            //        if (value)
            //        {
            //            SetBoolInput(inputs[i], false);
            //            await Task.Delay(timeout);
            //            SetBoolInput(inputs[i], true);
            //        }

            //        else
            //        {
            //            await Task.Delay(timeout);
            //            SetBoolInput(inputs[i], false);
            //        }
            //    };
            //}

            _boolHandlers[torOut] = async (bool value) =>
            {
                if (value)
                {
                    SetBoolInput(torIn, false);
                    await Task.Delay(timeout);
                    SetBoolInput(torIn, true);
                }

                else
                {
                    await Task.Delay(timeout);
                    SetBoolInput(torIn, false);
                }
            };

            _boolHandlers[revOut] = async (bool value) =>
            {
                if (value)
                {
                    SetBoolInput(revIn, false);
                    await Task.Delay(timeout);
                    SetBoolInput(revIn, true);
                }

                else
                {
                    await Task.Delay(timeout);
                    SetBoolInput(revIn, false);
                }
            };

            _boolHandlers[pos1out] = async (bool value) =>
            {
                if (value)
                {
                    SetBoolInput(pos1in, false);
                    SetBoolInput(pos2in, false);
                    SetBoolInput(pos3in, false);
                    await Task.Delay(timeout);
                    SetBoolInput(pos1in, true);
                }

                else
                {
                    await Task.Delay(timeout);
                    //SetBoolInput(pos1in, false);
                }
            };

            _boolHandlers[pos2out] = async (bool value) =>
            {
                if (value)
                {

                    SetBoolInput(pos1in, false);
                    SetBoolInput(pos2in, false);
                    SetBoolInput(pos3in, false);
                    await Task.Delay(timeout);
                    SetBoolInput(pos2in, true);
                }

                else
                {
                    await Task.Delay(timeout);
                    //SetBoolInput(pos2in, false);
                }
            };

            _boolHandlers[pos3out] = async (bool value) =>
            {
                if (value)
                {

                    SetBoolInput(pos1in, false);
                    SetBoolInput(pos2in, false);
                    SetBoolInput(pos3in, false);
                    await Task.Delay(timeout);
                    SetBoolInput(pos3in, true);
                }

                else
                {
                    await Task.Delay(timeout);
                    //SetBoolInput(pos3in, false);
                }
            };
        }

        private void SetBoolInput(ushort pinNumber, bool value)
        {
            //if (_boolInputs.GetValueOrDefault(pinNumber) != value)
            //{
                _boolInputs[pinNumber] = value;
                BoolInputChanged?.Invoke(pinNumber, value);
            //}
        }

        private void SetDoubleInput(ushort pinNumber, double value)
        {
            //if (_doubleInputs.GetValueOrDefault(pinNumber) != value)
            //{
                _doubleInputs[pinNumber] = value;
                DoubleInputChanged?.Invoke(pinNumber, value);
            //}
        }

        /// <summary>
        /// Set value strategy
        /// </summary>
        public void SetOutput<T>(ushort pinNumber, T value)
        {
            if (value is bool b && _boolHandlers.TryGetValue(pinNumber, out var boolHandler))
                boolHandler(b);
            else if (value is double d && _doubleHandlers.TryGetValue(pinNumber, out var doubleHandler))
                doubleHandler(d);
        }

        /// <summary>
        /// Get bool value strategy
        /// </summary>
        public bool GetInputBool(ushort pinNumber) => _boolInputs.GetValueOrDefault(pinNumber, false);

        /// <summary>
        /// Get double value strategy
        /// </summary>
        public double GetInputDouble(ushort pinNumber) => _doubleInputs.GetValueOrDefault(pinNumber, 0.0);


    }
}
