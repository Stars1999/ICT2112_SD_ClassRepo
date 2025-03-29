namespace ICT2106WebApp.mod2grp6.Template
{
    public interface ITemplate
    {
        Template ConvertToTemplate(TemplateDocument document);
        Task<Template> GetTemplate(string id);        // Method to retrieve a specific template
    }
}
