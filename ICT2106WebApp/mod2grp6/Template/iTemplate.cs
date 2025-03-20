namespace ICT2106WebApp.mod2grp6.Template
{
    public interface ITemplate
    {
        Template ConvertToTemplate(string id);  // Method to convert a document to a specific template
        Template GetTemplate(string id);        // Method to retrieve a specific template
    }
}
