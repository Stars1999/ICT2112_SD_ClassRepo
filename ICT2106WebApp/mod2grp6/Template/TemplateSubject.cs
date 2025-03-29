using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICT2106WebApp.mod2grp6.Template
{
    public abstract class TemplateSubject
    {
        private List<ITemplateObserver> observers = new List<ITemplateObserver>();

        // Attach an observer
        public void attach(ITemplateObserver observer)
        {
            observers.Add(observer);
        }

        // Detach an observer
        public void detach(ITemplateObserver observer)
        {
            observers.Remove(observer);
        }

        // Notify all observers
        public void notifyObservers(string id)
        {
            foreach (var observer in observers)
            {
                // Need to get the template and pass it to UpdateTemplate
                var template = GetTemplateByIdAsync(id).Result;
                if (template != null)
                {
                    observer.UpdateTemplate(template).Wait();
                }
            }
        }

        // Changed to match implementation in TemplateManager
        protected abstract Task<TemplateDocument> GetTemplateByIdAsync(string id);
    }
}