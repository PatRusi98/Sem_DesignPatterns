using Sem_DesignPatterns.Logic.Struct.Interfaces;

namespace Sem_DesignPatterns.Logic.Objects
{
    public class NodeData<T>(double[] key) : IStorable
    {
        public T? Value { get; set; } = default;
        public double[] KeyArr { get; } = key;

        public object[] GetKeys() => KeyArr.Cast<object>().ToArray();

        public static bool operator ==(NodeData<T> left, NodeData<T> right)
        {
            return Equals(left!.Value, right!.Value);
        }

        public static bool operator !=(NodeData<T>? left, NodeData<T>? right)
        {
            return !Equals(left!.Value, right!.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is NodeData<T> other)
            {
                return Equals(Value, other.Value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}
