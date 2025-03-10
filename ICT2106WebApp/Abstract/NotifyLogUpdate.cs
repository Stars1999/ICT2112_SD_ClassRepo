using ICT2106WebApp.Interfaces;
using System;
using System.Collections.Generic;

namespace ICT2106WebApp.Abstract
{
    public class NotifyLogUpdate
    {
        private readonly List<ILogObserver> observers = new List<ILogObserver>();

        public void RegisterObserver(ILogObserver observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(ILogObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers(string logMessage)
        {
            foreach (var observer in observers)
            {
                observer.Update(logMessage);
            }
        }
    }
}
