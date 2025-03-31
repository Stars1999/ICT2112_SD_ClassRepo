namespace ICT2106WebApp.mod1grp4 {
    // iBackupTabularSubject (Joel - COMPLETED)
    public abstract class iBackupTabularSubject {
        private ICollection<iBackupGatewayObserver> observers;

        public iBackupTabularSubject() {
            observers = new List<iBackupGatewayObserver>();
        }

        public void attach(iBackupGatewayObserver observer)
        {
            observers.Add(observer);
        }

        public void detach(iBackupGatewayObserver observer)
        {
            observers.Remove(observer);
        }

        public async Task<T> notify<T>(OperationType type, string message, object data) {
            foreach (iBackupGatewayObserver observer in observers)
            {
                return await observer.updateSubject<T>(type, message, data);
            }
            return default(T);
        }
    }
}