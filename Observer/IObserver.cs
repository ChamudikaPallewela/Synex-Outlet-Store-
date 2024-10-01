namespace Client.Observer
{
    public interface IObserver
    {
        void Notify(string message);
    }
}