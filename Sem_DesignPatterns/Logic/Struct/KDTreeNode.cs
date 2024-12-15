namespace Sem_DesignPatterns.Logic.Struct
{
    public class KDTreeNode<T>(T? value, int depth, KDTreeNode<T>? parent = null)
    {
        public T? Value { get; set; } = value;
        public KDTreeNode<T>? LeftSon { get; set; } = null;
        public KDTreeNode<T>? RightSon { get; set; } = null;
        public KDTreeNode<T>? Parent { get; set; } = parent;
        public int Depth { get; set; } = depth;
        public List<KDTreeNode<T>> Duplicates { get; set; } = new();
    }
}
