namespace Immutable_Deque_visualization
{
    public abstract class DequeNodeBase
    {}

    public class ValueNode<T> : DequeNodeBase
    {
        public T Value { get; protected set; }

        public ValueNode(T value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class DequeNode<T> : DequeNodeBase
    {
        public IDeque<T> Deque { get; protected set; }

        public DequeNode(IDeque<T> deque)
        {
            Deque = deque;
        }

        public override string ToString()
        {
            return Deque.GetType().GetShortName();
        }
    }

    public class DequeletteNode<T> : DequeNodeBase
    {
        public Deque<T>.Dequelette Dequelette { get; protected set; }

        public DequeletteNode(Deque<T>.Dequelette dequelette)
        {
            Dequelette = dequelette;
        }

        public override string ToString()
        {
            return Dequelette.GetType().GetShortName();
        }
    }

}