using Oratoria.Application.Module2;
using Oratoria.Domain.Devices;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Devices.Valve;
using Oratoria.UI.Controls.Mnemo;
using Oratoria.UI.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace Oratoria.UI.Views.Pages
{
    public partial class Module2MnemoPage : Page
    {
        public Module2MnemoPage(Module2MnemoPageVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }

    public class Module2MnemoPageVM : INotifyPropertyChanged
    {
        private readonly Module2Context _context;

        public string FK_KNName => _context.FK_KN_DU_63.DeviceName;

        public StateColor FK_KNState => MapStateToColor(_context.FK_KN_DU_63);

        public ErrorIcon FK_KNError => MapErrorsToColor(_context.FK_KN_DU_63);

        public ICommand FK_KNCommand => new RelayCommand(async (_) => await _context.FK_KN_DU_63.OpenValve());

        public Module2MnemoPageVM(Module2Context context)
        {
            _context = context;

            _context.FK_KN_DU_63.StateChanged += () => OnPropertyChanged(nameof(FK_KNState));
            _context.FK_KN_DU_63.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_KNError));
        }

        private static StateColor MapStateToColor(Valve valve)
        {
            if (valve.State == OpenableStatus.Open)
                return StateColor.On;
            if (valve.State == OpenableStatus.Close)
                return StateColor.Off;
            return StateColor.Transition;
        }

        private static ErrorIcon MapErrorsToColor(Valve valve)
        {
            if (valve.DeviceErrors.GetHighestCategory() == DeviceErrorCategory.Error)
                return ErrorIcon.Error;
            else if (valve.DeviceErrors.GetHighestCategory() == DeviceErrorCategory.Warn)
                return ErrorIcon.Warning;
            return ErrorIcon.None;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
