using ICT2106WebApp.Utilities;
using System.Collections.Generic;

namespace ICT2106WebApp.mod2grp6.Template
{
    public class TemplateManager
    {
        private Dictionary<string, List<AbstractNode>> templates = new Dictionary<string, List<AbstractNode>>();
        private List<IObserver> observers = new List<IObserver>();

        public TemplateManager applyTemplateConversion(string id)
        {
            if (templates.ContainsKey(id))
            {
                // Perform template conversion logic here
                // For now, just return the TemplateManager itself
                return this;
            }
            return null;
        }

        public TemplateManager getTemplate(string id)
        {
            if (templates.ContainsKey(id))
            {
                // Return the TemplateManager
                return this;
            }
            return null;
        }

        public void SetTemplate(string id, List<AbstractNode> content)
        {
            if (templates.ContainsKey(id))
            {
                templates[id] = content;
            }
            else
            {
                templates.Add(id, content);
            }
            NotifyObservers(id);
        }

        public void NotifyObservers(string id)
        {
            foreach (var observer in observers)
            {
                observer.Update(id);
            }
        }

        public void RegisterObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            observers.Remove(observer);
        }
    }

    public interface IObserver
    {
        void Update(string id);
    }
}