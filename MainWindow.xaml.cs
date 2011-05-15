using System.Windows;

namespace Immutable_Deque_visualization
{
    public partial class MainWindow : Window
    {
        private readonly DequeViewModel m_viewModel;

        public MainWindow()
        {
            InitializeComponent();

            m_viewModel = new DequeViewModel();
            DataContext = m_viewModel;
        }
    }
}