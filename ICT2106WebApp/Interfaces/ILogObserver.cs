namespace ICT2106WebApp.Interfaces
{
    public interface ILogObserver
    {
        public void Update(string logMessage)
        {
            Console.WriteLine($"Observer Notified: {logMessage}");
        }
    }
}
