namespace ICT2106WebApp.mod1grp4 {
    public abstract class iBackupTabularSubject {
        protected List<iBackupGatewayObserver> observers = new List<iBackupGatewayObserver>();

        public void attach(iBackupGatewayObserver observer)
        {
            observers.Add(observer);
        }

        public void detach(iBackupGatewayObserver observer)
        {
            observers.Remove(observer);
        }

        public void notify(string message) {
            foreach (var observer in observers)
            {
                // Notify each observer
                Console.WriteLine(message);
            }
        }
    }
}