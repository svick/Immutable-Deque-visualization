namespace Immutable_Deque_visualization
{
    public class DequeViewModel
    {
        public IDeque<string> Deque { get; protected set; }

        public DequeViewModel()
        {
            Deque = Deque<string>.Empty;

            int i = 1;
            for (; i <= 21; i++)
                Deque = Deque.EnqueueLeft(i.ToString());
        }
    }
}