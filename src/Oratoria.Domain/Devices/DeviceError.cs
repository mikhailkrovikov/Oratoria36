using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Oratoria.Domain.Devices
{
    public class DeviceError<TError> : IEnumerable<TError>, INotifyPropertyChanged where TError : Enum
    {
        private readonly HashSet<TError> _errors;
        public delegate void ModuleErrorHandler(TError error);
        public event ModuleErrorHandler? ErrorChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public DeviceError()
        {
            _errors = new HashSet<TError>();
        }

        public DeviceErrorCategory HighestCategory => GetHighestCategory();

        public static DeviceErrorCategory GetCategory(TError error)
        {
            var f = typeof(TError).GetField(error.ToString());
            if (f == null) return DeviceErrorCategory.None;
            var attribute = f.GetCustomAttribute<DeviceErrorCategoryAttribute>(false);
            return attribute?.Category ?? DeviceErrorCategory.None;
        }

        public DeviceErrorCategory GetHighestCategory()
        {
            var highest = DeviceErrorCategory.None;
            foreach (var error in _errors)
            {
                var category = GetCategory(error);
                if (category > highest)
                    highest = category;
            }
            return highest;
        }

        public void AddError(TError error)
        {
            _errors.Add(error);
            ErrorChanged?.Invoke(error);
            OnPropertyChanged(nameof(HighestCategory));
        }

        public void ResetRangeErrors(params TError[] errors)
        {
            foreach (var e in errors)
                _errors.Remove(e);
            OnPropertyChanged(nameof(HighestCategory));
        }

        public bool HasError(TError error)
        {
            return _errors.Contains(error);
        }

        public bool HasErrors()
        {
            return _errors.Count != 0;
        }

        public void ResetError(TError error)
        {
            _errors.Remove(error);
            ErrorChanged?.Invoke(error);
            OnPropertyChanged(nameof(HighestCategory));
        }

        public void ResetAllErrors()
        {
            _errors.Clear();
            OnPropertyChanged(nameof(HighestCategory));
        }

        public IEnumerator<TError> GetEnumerator()
        {
            return _errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
