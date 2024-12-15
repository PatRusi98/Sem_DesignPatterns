using Sem_DesignPatterns.UI.Models;

namespace Sem_DesignPatterns.UI.Services.Interfaces
{
    public interface IFileHandler
    {
        List<BaseModel> LoadData(string filePath);
        void SaveData(string filePath, List<BaseModel> data);
    }
}
