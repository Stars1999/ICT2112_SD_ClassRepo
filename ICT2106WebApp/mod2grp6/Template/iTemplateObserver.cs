using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICT2106WebApp.mod2grp6.Template
{
    public interface ITemplateObserver
    {
        Task<List<TemplateDocument>> GetAllTemplatesAsync();
        Task<bool> UpdateTemplate(TemplateDocument template);
    }
}