using NLog;
using NLog.Config;
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

namespace Oratoria36.Models.Devices
{
    public class Manipulator : Device, INotifyPropertyChanged
    {
        //TODO:
        //Проверки
        //Положение манипулятора не определено
        //Неоднозначное положение манипулятора

        readonly OutputSignal<bool> ManipulatorPrivod1;
        readonly OutputSignal<bool> PlatePrivod3;
        readonly OutputSignal<bool> TormosOut;
        readonly OutputSignal<bool> ReversOut;
        readonly OutputSignal<bool> Position1Out;
        readonly OutputSignal<bool> Position2Out;
        readonly OutputSignal<bool> Position3Out;

        readonly InputSignal<bool> Position1In;
        readonly InputSignal<bool> Position2In;
        readonly InputSignal<bool> Position3In;
        readonly InputSignal<bool> TormosIn;
        readonly InputSignal<bool> ReversIn;

        public event EventHandler<State> StateChanged;
        public event EventHandler<ManipulatorErrors> ErrorStateChanged;

        public Setting<int> ManipulatorActionTime;

        Logger _logger = LogManager.GetLogger("Манипулятор");
        private CancellationTokenSource token = new CancellationTokenSource();

        private ManipulatorErrors _errorState = ManipulatorErrors.None;
        public ManipulatorErrors ErrorState
        {
            get => _errorState;
            private set
            {
                if (_errorState != value)
                {
                    _errorState = value;
                    OnPropertyChanged(nameof(ErrorState));
                    ErrorStateChanged?.Invoke(this, value);
                }
            }
        }

        private ManipulatorPosition _position;
        public ManipulatorPosition Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged(nameof(Position));
                }
            }
        }
    
        private State _state = State.On;
        public override State State
        {
            get => _state;
            protected set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged(nameof(State));
                    StateChanged?.Invoke(this, value);
                }
            }
        }
        public Manipulator(OutputSignal<bool> manipulatorPrivod1,
                           OutputSignal<bool> platePrivod3,
                           OutputSignal<bool> tormosOut,
                           OutputSignal<bool> reversOut,
                           OutputSignal<bool> position1Out,
                           OutputSignal<bool> position2Out,
                           OutputSignal<bool> position3Out,

                           InputSignal<bool> position1In,
                           InputSignal<bool> position2In,
                           InputSignal<bool> position3In,
                           InputSignal<bool> tormosIn,
                           InputSignal<bool> reversIn)

        {
            ManipulatorPrivod1 = manipulatorPrivod1;
            PlatePrivod3 = platePrivod3;
            TormosOut = tormosOut;
            ReversOut = reversOut;
            Position1Out = position1Out;
            Position2Out = position2Out;
            Position3Out = position3Out;
            Position1In = position1In;
            Position2In = position2In;
            Position3In = position3In;
            TormosIn = tormosIn;
            ReversIn = reversIn;
            ManipulatorActionTime = CommonDeviceSettings.ManipulatorActionTime;

            Position1In.OnSignalChanged += value =>
            {
                OnPropertyChanged(nameof(State));
                OnPropertyChanged(nameof(Position));
            };
            Position2In.OnSignalChanged += value =>
            {
                OnPropertyChanged(nameof(State));
                OnPropertyChanged(nameof(Position));
            };
            Position3In.OnSignalChanged += value =>
            {
                OnPropertyChanged(nameof(State));
                OnPropertyChanged(nameof(Position));
            };
        }

        public async Task<bool> Load()
        {
            try
            {
                if (!await FromHomeToTransportNoPlate())
                    return false;
                if (!await FromTransportToHomeWithPlate())
                    return false;
                if (!await FromHomeToModuleWithPlate())
                    return false;
                if (!await FromModuleToHomeNoPlate())
                    return false;
                return true;
            }
            catch
            {
                EmergencyStop();
                return false;
            } 
        }

        public async Task<bool> UnLoad()
        {
            try
            {
                if(!await FromHomeToModuleNoPlate())
                    return false;
                if (!await FromModuleToHomeWithPlate())
                    return false;
                if (!await FromHomeToTransportWithPlate())
                    return false;
                if (!await FromTransportToHomeNoPlate())
                    return false;
                return true;
            }
            catch
            {
                EmergencyStop();
                return false;
            }
        }

        public void EmergencyStop()
        {
            token.Cancel();
            StopAllMechanisms();
            _logger.Info("Стоп манипулятора");
            token = new CancellationTokenSource();
        }
        private void StopAllMechanisms()
        {
            ManipulatorPrivod1.Value = false;
            PlatePrivod3.Value = false;
            TormosOut.Value = false;
            ReversOut.Value = false;
            Position1Out.Value = false;
            Position2Out.Value = false;
            Position3Out.Value = false;
        }

        private async Task<bool> ExecuteWithCancellation(Func<CancellationToken, Task<bool>> operation)
        {
            try
            {
                return await operation(token.Token);
            }
            catch (OperationCanceledException)
            {
                StopAllMechanisms();
                return false;
            }
        }

        /// <summary>
        /// Манипулятор из 2 в 1 без пластины
        /// </summary>
        public async Task<bool> FromHomeToTransportNoPlate()
        {
            return await ExecuteWithCancellation(async (token) =>
            {
                if (!IsManipulatorInHomePosition())
                    return false;

                if (!IsPlateInManipulator())
                    return false;

                ManipulatorPrivod1.Value = true;
                ReversCommand(true);
                TormosCommand(true);
                Position1Out.Value = true;

                bool positionReached = await WaitForSignal(Position1In, ManipulatorActionTime.Value, token);
                token.ThrowIfCancellationRequested();

                if (!positionReached)
                    _logger.Warn("Манипулятор: превышено время движения");

                ReversCommand(false);
                TormosCommand(false);
                Position1Out.Value = false;

                if (!IsManipulatorInTransport())
                    return false;

                ManipulatorPrivod1.Value = false;
                State = State.On;
                Position = ManipulatorPosition.Transport;
                return true;
            });
        }

        /// <summary>
        /// Манипулятор из 1 в 2 с пластиной
        /// </summary>
        public async Task<bool> FromTransportToHomeWithPlate()
        {
            return await ExecuteWithCancellation(async (token) =>
            {
                if (!IsManipulatorInTransport())
                    return false;

                ManipulatorPrivod1.Value = true;
                TormosCommand(true);
                ReversCommand(false);
                Position2Out.Value = true;

                bool positionReached = await WaitForSignal(Position2In, ManipulatorActionTime.Value, token);
                token.ThrowIfCancellationRequested();

                if (!positionReached)
                    _logger.Warn("Манипулятор: превышено время движения");

                ManipulatorPrivod1.Value = false;
                TormosCommand(false);
                Position2Out.Value = false;

                if (!IsManipulatorHasPlateFromTransport())
                    return false;

                State = State.On;
                Position = ManipulatorPosition.Home;
                return true;
            });
        }

        /// <summary>
        /// Манипулятор из 2 в 3 с пластиной
        /// </summary>
        public async Task<bool> FromHomeToModuleWithPlate()
        {
            return await ExecuteWithCancellation(async (token) =>
            {
                if (!IsManipulatorInHomePosition())
                    return false;

                if (!IsManipulatorHasPlateFromTransport())
                    return false;

                ManipulatorPrivod1.Value = true;
                TormosCommand(true);
                ReversCommand(false);
                Position3Out.Value = true;

                bool positionReached = await WaitForSignal(Position3In, ManipulatorActionTime.Value, token);
                token.ThrowIfCancellationRequested();

                if (!positionReached)
                    _logger.Warn("Манипулятор: превышено время движения");

                ManipulatorPrivod1.Value = false;
                TormosCommand(false);
                ReversCommand(false);
                Position3Out.Value = false;

                if (!IsManipulatorInModule())
                    return false;

                ManipulatorPrivod1.Value = false;
                State = State.On;
                Position = ManipulatorPosition.Module;
                return true;
            });
        }

        /// <summary>
        /// Манипулятор из 3 в 2 без пластины
        /// </summary>
        public async Task<bool> FromModuleToHomeNoPlate()
        {
            return await ExecuteWithCancellation(async (token) =>
            {
                if (!IsManipulatorInModule())
                    return false;

                ManipulatorPrivod1.Value = true;
                ReversCommand(true);
                TormosCommand(true);
                Position2Out.Value = true;

                bool positionReached = await WaitForSignal(Position2In, ManipulatorActionTime.Value, token);
                token.ThrowIfCancellationRequested();

                if (!positionReached)
                    _logger.Warn("Манипулятор: превышено время движения");

                ReversCommand(false);
                TormosCommand(false);
                Position2Out.Value = false;

                if (Position2In.Value)
                {
                    ErrorState = ManipulatorErrors.Error1_5;
                    State = State.Warning;
                    _logger.Warn("Манипулятор не поднялся от ложемента к исходному");
                    return false;
                }

                ManipulatorPrivod1.Value = false;
                if (!IsManipulatorPlacedPlateInModule())
                    return false;

                State = State.On;
                Position = ManipulatorPosition.Home;
                return true;
            });
        }

        /// <summary>
        /// Манипулятор из 2 в 3 без пластины
        /// </summary>
        public async Task<bool> FromHomeToModuleNoPlate()
        {
            return await ExecuteWithCancellation(async (token) =>
            {
                if (!IsManipulatorInHomePosition())
                    return false;

                if (!IsPlateInManipulator())
                    return false;

                ManipulatorPrivod1.Value = true;
                ReversCommand(false);
                TormosCommand(true);
                Position3Out.Value = true;

                bool positionReached = await WaitForSignal(Position3In, ManipulatorActionTime.Value, token);
                token.ThrowIfCancellationRequested();

                if (!positionReached)
                    _logger.Warn("Манипулятор: превышено время движения");

                ReversCommand(false);
                TormosCommand(false);
                Position3Out.Value = false;

                if (!IsManipulatorInModule())
                    return false;

                ManipulatorPrivod1.Value = false;
                State = State.On;
                Position = ManipulatorPosition.Module;
                return true;
            });
        }

        /// <summary>
        /// Манипулятор из 3 в 2 с пластиной
        /// </summary>
        public async Task<bool> FromModuleToHomeWithPlate()
        {
            return await ExecuteWithCancellation(async (token) =>
            {
                if (!IsManipulatorInModule())
                    return false;

                ManipulatorPrivod1.Value = true;
                ReversCommand(true);
                TormosCommand(true);
                Position2Out.Value = true;

                bool positionReached = await WaitForSignal(Position2In, ManipulatorActionTime.Value, token);
                token.ThrowIfCancellationRequested();

                if (!positionReached)
                    _logger.Warn("Манипулятор: превышено время движения");

                ReversCommand(false);
                TormosCommand(false);
                Position2Out.Value = false;

                if (!Position2In.Value)
                {
                    ErrorState = ManipulatorErrors.Error1_5;
                    State = State.Warning;
                    _logger.Warn("Манипулятор не поднялся от ложемента к исходному");
                    return false;
                }

                ManipulatorPrivod1.Value = true;
                if (!IsManipulatorHasPlateFromModule())
                    return false;

                State = State.On;
                Position = ManipulatorPosition.Home;
                return true;
            });
        }

        /// <summary>
        /// Манипулятор из 2 в 1 с пластиной
        /// </summary>
        public async Task<bool> FromHomeToTransportWithPlate()
        {
            return await ExecuteWithCancellation(async (token) =>
            {
                if (!IsManipulatorInHomePosition())
                    return false;

                if (!IsManipulatorHasPlateFromModule())
                    return false;

                ManipulatorPrivod1.Value = true;
                ReversCommand(true);
                TormosCommand(true);
                Position1Out.Value = true;

                bool positionReached = await WaitForSignal(Position1In, ManipulatorActionTime.Value, token);
                token.ThrowIfCancellationRequested();

                if (!positionReached)
                    _logger.Warn("Манипулятор: превышено время движения");

                ReversCommand(false);
                TormosCommand(false);
                Position1Out.Value = false;

                if (!IsManipulatorInTransport())
                    return false;

                ManipulatorPrivod1.Value = false;
                State = State.On;
                Position = ManipulatorPosition.Transport;
                return true;
            });
        }

        /// <summary>
        /// Манипулятор из 1 в 2 без пластины
        /// </summary>
        public async Task<bool> FromTransportToHomeNoPlate()
        {
            return await ExecuteWithCancellation(async (token) =>
            {
                if (!IsManipulatorInTransport())
                    return false;

                ManipulatorPrivod1.Value = true;
                ReversCommand(false);
                TormosCommand(true);
                Position2Out.Value = true;

                bool positionReached = await WaitForSignal(Position2In, ManipulatorActionTime.Value, token);
                token.ThrowIfCancellationRequested();

                if (!positionReached)
                    _logger.Warn("Манипулятор: превышено время движения");

                ReversCommand(false);
                TormosCommand(false);
                Position2Out.Value = false;

                if (!Position2In.Value)
                {
                    ErrorState = ManipulatorErrors.Error1_7;
                    State = State.Warning;
                    _logger.Warn("Манипулятор не поднялся от каретки к исходному");
                    return false;
                }

                ManipulatorPrivod1.Value = false;
                if (!IsManipulatorPlacedPlateInTransport())
                    return false;
                State = State.On;
                Position = ManipulatorPosition.Home;
                return true;
            });
        }


        #region Manipulator Error Check

        /// <summary>
        /// Ожидание приезда в позицию
        /// </summary>
        private async Task<bool> WaitForSignal(InputSignal<bool> signal, int timeout, CancellationToken cancellationToken)
        {
            if (signal.Value)
                return true;

            State = State.Transition;


            var tcs = new TaskCompletionSource<bool>();
            using var registration = cancellationToken.Register(() => tcs.TrySetCanceled());

            Action<bool> handler = null;
            handler = (value) =>
            {
                if (value)
                {
                    signal.OnSignalChanged -= handler;
                    tcs.TrySetResult(true);
                }
            };

            signal.OnSignalChanged += handler;

            try
            {
                var timeoutTask = Task.Delay(timeout * 1000, cancellationToken);
                var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    _logger.Warn("Превышено время ожидания сигнала");
                    return false;
                }

                return await tcs.Task;
            }
            finally
            {
                signal.OnSignalChanged -= handler;
            }
        }

        /// <summary>
        /// Команда реверс
        /// </summary>
        private void TormosCommand(bool value)
        {
            TormosOut.Value = value;
            if (TormosIn.Value != value)
                _logger.Warn("Тормоз: ошибка обратной связи");
        }

        /// <summary>
        /// Команда реверс
        /// </summary>
        /// <param name="value"></param>
        private void ReversCommand(bool value)
        {
            ReversOut.Value = value;
            if (ReversIn.Value != value)
                _logger.Warn("Реверс: ошибка обратной связи");
        }

        /// <summary>
        /// Манипулятор не в исходном положении
        /// </summary>
        private bool IsManipulatorInHomePosition()
        {
            ManipulatorPrivod1.Value = true;
            if (Position2In.Value)
            {
                ManipulatorPrivod1.Value = false;
                return true;
            }
            else
            {
                ErrorState = ManipulatorErrors.Error1_1;
                _logger.Warn("Манипулятор: не в исходном положении");
                State = State.Warning;
                return false;
            }
        }

        /// <summary>
        ///  Манипулятор не взял пластину из каретки
        /// </summary>
        private bool IsManipulatorHasPlateFromTransport()
        {
            PlatePrivod3.Value = true;
            if (Position1In.Value)
            {
                PlatePrivod3.Value = false;
                return true;
            }
            else
            {
                ErrorState = ManipulatorErrors.Error1_10;
                _logger.Warn("Манипулятор не взял пластину из каретки");
                State = State.Warning;
                return false;
            }
        }

        /// <summary>
        /// Манипулятор не взял пластину из ложемента
        /// </summary>
        private bool IsManipulatorHasPlateFromModule()
        {
            PlatePrivod3.Value = true;
            if (!Position1In.Value)
            {
                PlatePrivod3.Value = false;
                return true;
            }
            else
            {
                ErrorState = ManipulatorErrors.Error1_12;
                _logger.Warn("Манипулятор не взял пластину из ложемента");
                State = State.Warning;
                return false;
            }
        }

        /// <summary>
        /// Манипулятор не поставил пластину в ложемент
        /// </summary>
        private bool IsManipulatorPlacedPlateInModule()
        {
            PlatePrivod3.Value = true;
            if (!Position1In.Value)
            {
                PlatePrivod3.Value = false;
                return true;
            }
            else
            {
                ErrorState = ManipulatorErrors.Error1_11;
                _logger.Warn("Манипулятор не поставил пластину в ложемент");
                State = State.Warning;
                return false;
            }
        }

        /// <summary>
        /// Манипулятор не опустился к ложементу
        /// </summary>
        private bool IsManipulatorInModule()
        {
            ManipulatorPrivod1.Value = true;
            if (Position3In.Value)
            {
                ManipulatorPrivod1.Value = false;
                return true;
            }
            else
            {
                ErrorState = ManipulatorErrors.Error1_4;
                _logger.Warn("Манипулятор не опустился к ложементу");
                State = State.Warning;
                return false;
            }
        }

        /// <summary>
        /// Манипулятор не опустился к каретке
        /// </summary>
        private bool IsManipulatorInTransport()
        {
            ManipulatorPrivod1.Value = true;
            if (Position1In.Value)
            {
                ManipulatorPrivod1.Value = false;
                return true;
            }
            else
            {
                ErrorState = ManipulatorErrors.Error1_6;
                _logger.Warn("Манипулятор не опустился к каретке");
                State = State.Warning;
                return false;
            }
        }

        /// <summary>
        /// Наличие пластины в Манипуляторе
        /// </summary>
        private bool IsPlateInManipulator()
        {
            PlatePrivod3.Value = true;
            if (!Position1In.Value)
            {
                PlatePrivod3.Value = false;
                return true;
            }
            else
            {
                ErrorState = ManipulatorErrors.Error1_8;
                _logger.Warn("Наличие пластины в Манипуляторе");
                State = State.Warning;
                return false;
            }
        }

        /// <summary>
        /// Манипулятор не поставил пластину в каретку
        /// </summary>
        private bool IsManipulatorPlacedPlateInTransport()
        {
            PlatePrivod3.Value = true;
            if (Position1In.Value)
            {
                PlatePrivod3.Value = false;
                return true;
            }
            else
            {
                ErrorState = ManipulatorErrors.Error1_9;
                _logger.Warn("Манипулятор не поставил пластину в каретку");
                State = State.Warning;
                return false;
                
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
