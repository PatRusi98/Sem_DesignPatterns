namespace Sem_DesignPatterns.Logic.Struct.Interfaces
{
    public interface ITreeFactory<TBase>
    {
        ITree<T> CreateTree<T>() where T : TBase;
    }
}
