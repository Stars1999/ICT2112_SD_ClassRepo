using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6.Template
{
    public interface ITemplateObserver
    {
        Task<bool> UpdateTemplate(TemplateDocument template);
        Task<List<TemplateDocument>> GetAllTemplatesAsync();
    }
}
