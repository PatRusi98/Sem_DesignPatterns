using Sem_DesignPatterns.Logic.Struct.Interfaces;

namespace Sem_DesignPatterns.Logic.Struct
{
    public class TreeManager<T> where T : IStorable
    {
        private readonly ITree<T> _tree;

        public TreeManager(ITreeFactory<IStorable> factory)
        {
            _tree = factory.CreateTree<T>();
        }

        public bool Add(T item) => _tree.Insert(item);
        public List<T>? Find(T item) => _tree.Search(item);
        public List<T>? FindAll() => _tree.SearchAll();
        public bool Remove(T item) => _tree.Delete(item);
    }
}
