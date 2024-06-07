using System.Threading.Tasks;

namespace Downloder
{
    public interface IFileService
    {
        Task<bool> SaveFileToLocalStorage();
    }
}
