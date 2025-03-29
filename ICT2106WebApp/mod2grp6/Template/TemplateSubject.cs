using System;
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
            if (observer != null && !observers.Contains(observer))
            {
                observers.Add(observer);
                Console.WriteLine($"Observer attached: {observer.GetType().Name}");
            }
        }

        // Detach an observer
        public void detach(ITemplateObserver observer)
        {
            if (observer != null && observers.Contains(observer))
            {
                observers.Remove(observer);
                Console.WriteLine($"Observer detached: {observer.GetType().Name}");
            }
        }

        // Notify observers when templates are loaded
        public void notifyTemplatesLoaded(List<TemplateDocument> templates)
        {
            if (templates == null || templates.Count == 0) return;
            
            Console.WriteLine($"Notifying {observers.Count} observers about {templates.Count} templates loaded");
            
            foreach (var observer in observers)
            {
                try
                {
                    observer.OnTemplatesLoaded(templates);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error notifying observer {observer.GetType().Name}: {ex.Message}");
                }
            }
        }

        // Notify observers when templates are loaded
        public void notifyTemplateLoaded(TemplateDocument template)
        {
            if (template == null) return;

            Console.WriteLine($"Notifying {observers.Count} observers about {template.TemplateName} templates loaded");
            List<TemplateDocument> templatesList = new List<TemplateDocument> { template };
            foreach (var observer in observers)
            {
                try
                {
                    observer.OnTemplatesLoaded(templatesList);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error notifying observer {observer.GetType().Name}: {ex.Message}");
                }
            }
        }
    }
}