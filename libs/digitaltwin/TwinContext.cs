namespace DigitalTwin
{
    public class TwinContext
    {
        public TwinModel TModel { get; }

        public TwinContext()
        {
            TModel = new TwinModel();
        }
    }
}
