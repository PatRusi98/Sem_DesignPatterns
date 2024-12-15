namespace Sem_DesignPatterns.Logic.Struct.Interfaces
{
    public interface ITree<T>
    {
        bool Insert(T record);
        List<T>? Search(T record);
        List<T>? SearchAll();
        bool Delete(T record);
    }
}
