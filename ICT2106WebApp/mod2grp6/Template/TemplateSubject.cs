using System.Collections.Generic;

namespace ICT2106WebApp.mod2grp6.Template
{
    public abstract class TemplateSubject
    {
        private List<ITemplateObserver> observers = new List<ITemplateObserver>();

        // Attach an observer
        public void Attach(ITemplateObserver observer)
        {
            observers.Add(observer);
        }

        // Detach an observer
        public void Detach(ITemplateObserver observer)
        {
            observers.Remove(observer);
        }

        // Notify all observers
        public void NotifyObservers(string templateId)
        {
            foreach (var observer in observers)
            {
                observer.UpdateTemplate(templateId);
            }
        }
    }
}
