namespace ICT2106WebApp.mod2grp6.Template
{
    public class TemplateObserver : ITemplateObserver
    {
        public void UpdateTemplate(string templateId)
        {
            // Handle the template update notification
            Console.WriteLine($"Template {templateId} has been updated.");
        }
    }
}
