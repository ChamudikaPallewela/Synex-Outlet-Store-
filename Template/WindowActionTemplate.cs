using Client.Observer;

namespace Client.Template
{
    public abstract class WindowActionTemplate
    {
        protected readonly IObserver Observer;

        protected WindowActionTemplate(IObserver observer)
        {
            Observer = observer;
        }

        public void Execute()
        {
            PerformPreAction();
            PerformAction();
            PerformPostAction();
        }

        protected abstract void PerformPreAction();

        protected abstract void PerformAction();

        protected abstract void PerformPostAction();
    }
}