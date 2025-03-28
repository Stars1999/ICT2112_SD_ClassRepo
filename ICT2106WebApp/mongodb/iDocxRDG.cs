using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDocxRDG
{
// {
//     Task CreateAsync(Docx docx);
//     Task<Docx> GetByIdAsync(string id);
//     Task<IEnumerable<Docx>> GetAllAsync();
    // Other methods as needed
    Task<Docx> getDocument(string id);
    // Task<Docx> GetByIdAsync(string id, IDocxControl notifier = null);
    Task<List<Docx>> GetAllAsync();
    Task saveDocument(Docx docx);
    Task UpdateAsync(Docx docx);
    Task DeleteAsync(string id);
}

