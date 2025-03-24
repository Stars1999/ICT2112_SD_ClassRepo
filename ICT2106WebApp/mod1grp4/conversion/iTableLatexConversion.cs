// iTableLatexConversion.cs
using System.Threading.Tasks;

namespace ICT2106WebApp.mod1grp4
{
    public interface iTableLatexConversion
    {
        Task<List<Table>> convertToLatexAsync(List<Table> tableList); // Asynchronous cell conversion
    }
}