namespace Oratoria.Infrastructure
{
    public static class EventWaiter
    {
        public static async Task<bool> WaitEvent(string eventName, object source, int timeout, CancellationToken token)
        {
            var eventInfo = source.GetType().GetEvent(eventName);
            if (eventInfo == null)
                throw new ArgumentNullException("Нет найдено событие");

            AutoResetEvent wh = new AutoResetEvent(false);
            var t = eventInfo.EventHandlerType;
            Waiter waiter = new Waiter(wh);
            var method = typeof(Waiter).GetMethod("Wait");
            Delegate del = Delegate.CreateDelegate(t, waiter, method);
            eventInfo.AddEventHandler(source, del);
            try
            {
                return await Task.Run(() => WaitHandle.WaitAny([wh, token.WaitHandle], timeout) == 0);
            }
            finally
            {
                eventInfo.RemoveEventHandler(source, del);
            }
        }

        public static async Task<bool> WaitEvent<T>(string eventName, object source, Predicate<T> predict, int timeout, CancellationToken token)
        {
            var eventInfo = source.GetType().GetEvent(eventName);
            if (eventInfo == null)
                throw new ArgumentNullException("Нет найдено событие");

            AutoResetEvent wh = new AutoResetEvent(false);
            var t = eventInfo.EventHandlerType;
            WaiterT<T> waiter = new WaiterT<T>(wh, predict);
            var method = typeof(WaiterT<T>).GetMethod("Wait");
            Delegate del = Delegate.CreateDelegate(t, waiter, method);
            eventInfo.AddEventHandler(source, del);
            try
            {
                return await Task.Run(() => WaitHandle.WaitAny([wh, token.WaitHandle], timeout) == 0);
            }
            finally
            {
                eventInfo.RemoveEventHandler(source, del);
            }
        }
    }

    file class Waiter
    {
        AutoResetEvent _wh;

        public Waiter(AutoResetEvent wh)
        {
            _wh = wh;
        }

        public void Wait()
        {
            _wh.Set();
        }
    }

    file class WaiterT<T>
    {
        AutoResetEvent _wh;
        Predicate<T> _predicate;

        public WaiterT(AutoResetEvent wh, Predicate<T> predicate)
        {
            _wh = wh;
            _predicate = predicate;
        }

        public void Wait(T obj)
        {
            Task.Run(() =>
            {
                if (_predicate(obj))
                    _wh.Set();
            });
        }
    }
}
