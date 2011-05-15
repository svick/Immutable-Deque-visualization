using System.ComponentModel;

namespace Immutable_Deque_visualization
{
    public class DequeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { }; 

        public IDeque<string> Deque { get; protected set; }

        public string NewNodeValue { get; set; }

        public RelayCommand EnqueueLeftCommand { get; protected set; }

        public RelayCommand EnqueueRightCommand { get; protected set; }

        public RelayCommand DequeueLeftCommand { get; protected set; }

        public RelayCommand DequeueRightCommand { get; protected set; }

        public DequeViewModel()
        {
            Deque = Deque<string>.Empty;

            EnqueueLeftCommand = new RelayCommand(EnqueueLeft);
            EnqueueRightCommand = new RelayCommand(EnqueueRight);
            DequeueLeftCommand = new RelayCommand(DequeueLeft);
            DequeueRightCommand = new RelayCommand(DequeueRight);
        }

        public void EnqueueLeft()
        {
            Deque = Deque.EnqueueLeft(NewNodeValue);
            DequeChanged();
        }

        public void EnqueueRight()
        {
            Deque = Deque.EnqueueRight(NewNodeValue);
            DequeChanged();
        }

        void DequeueLeft()
        {
            if (!Deque.IsEmpty)
            {
                Deque = Deque.DequeueLeft();
                DequeChanged();
            }
        }

        void DequeueRight()
        {
            if (!Deque.IsEmpty)
            {
                Deque = Deque.DequeueRight();
                DequeChanged();
            }
        }


        void DequeChanged()
        {
            PropertyChanged(this, new PropertyChangedEventArgs("Deque"));
        }
    }
}