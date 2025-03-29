using System.Threading.Tasks;

namespace ICT2106WebApp.mod2grp6.Template
{
    public interface ITemplate
    {
        Template convertToTemplate(TemplateDocument document);
        Task<Template> getTemplate(string id);
    }
}