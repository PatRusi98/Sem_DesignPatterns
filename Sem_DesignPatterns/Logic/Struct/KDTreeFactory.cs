using Sem_DesignPatterns.Logic.Struct.Interfaces;

namespace Sem_DesignPatterns.Logic.Struct
{
    public class KDTreeFactory : ITreeFactory<IStorable>
    {
        private static KDTreeFactory instance;
        private KDTreeFactory() { }
        public static KDTreeFactory Instance()
        {
            if (instance == null)
            {
                instance = new KDTreeFactory();
            }
            return instance;
        }

        public ITree<T> CreateTree<T>() where T : IStorable
        {
            return new KDTree<T>();
        }
    }
}
