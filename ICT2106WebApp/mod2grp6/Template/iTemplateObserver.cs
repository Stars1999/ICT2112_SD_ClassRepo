using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICT2106WebApp.mod2grp6.Template
{
    public interface ITemplateObserver
    {
        // Methods for the observer pattern
        void OnTemplateUpdated(TemplateDocument template);
        void OnTemplatesLoaded(List<TemplateDocument> templates);
    }
}