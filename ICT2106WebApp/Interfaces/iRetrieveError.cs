using System.Collections.Generic;
using System.Threading.Tasks;

public interface iRetrieveError
{
    List<ErrorStyle> FetchAllError(string latexContent);
}
