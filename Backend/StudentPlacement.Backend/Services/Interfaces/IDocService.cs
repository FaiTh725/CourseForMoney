using Xceed.Words.NET;

namespace StudentPlacement.Backend.Services.Interfaces
{
    public interface IDocService
    {
        public Task<DocX> CreateReport(int idGroup);
    }
}
