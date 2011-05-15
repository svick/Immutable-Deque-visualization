using System;
using System.Reflection;
using System.Windows;
using TreeContainer;

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

            Visualize(m_viewModel.Deque, null);
        }

        void Visualize<T>(IDeque<T> iDeque, TreeNode parent)
        {
            var dequeNode = AddDequeNode(iDeque, parent);

            if (iDeque is Deque<T>.SingleDeque)
            {
                var singleDeque = (Deque<T>.SingleDeque)iDeque;
                Visualize(singleDeque.Item, dequeNode);
            }
            else if (iDeque is Deque<T>)
            {
                var deque = (Deque<T>)iDeque;
                Visualize(deque.Left, dequeNode);
                Visualize(deque.Middle, dequeNode);
                Visualize(deque.Right, dequeNode);
            }
        }

        void Visualize<T>(Deque<T>.Dequelette dequelette, TreeNode parent)
        {
            var dequeletteNode = AddDequeletteNode(dequelette, parent);

            if (dequelette is Deque<T>.One)
            {
                var one = (Deque<T>.One)dequelette;
                Visualize(one.V1, dequeletteNode);
            }
            else if (dequelette is Deque<T>.Two)
            {
                var two = (Deque<T>.Two)dequelette;
                Visualize(two.V1, dequeletteNode);
                Visualize(two.V2, dequeletteNode);
            }
            else if (dequelette is Deque<T>.Three)
            {
                var three = (Deque<T>.Three)dequelette;
                Visualize(three.V1, dequeletteNode);
                Visualize(three.V2, dequeletteNode);
                Visualize(three.V3, dequeletteNode);
            }
            else if (dequelette is Deque<T>.Four)
            {
                var four = (Deque<T>.Four)dequelette;
                Visualize(four.V1, dequeletteNode);
                Visualize(four.V2, dequeletteNode);
                Visualize(four.V3, dequeletteNode);
                Visualize(four.V4, dequeletteNode);
            }
        }

        void Visualize<T>(T value, TreeNode parent)
        {
            MethodInfo bestOverload = null;
            Type t = typeof(T);
            while (bestOverload == null)
            {
                bestOverload = typeof(MainWindow).GetGenericMethod(
                    "Visualize", BindingFlags.NonPublic | BindingFlags.Instance, t, typeof(TreeNode));
                if (bestOverload == null)
                    t = t.BaseType;
            }
            var thisMethod = MethodBase.GetCurrentMethod();

            if (bestOverload == thisMethod)
                AddValueNode(value, parent);
            else
                bestOverload.MakeGenericMethod(t.GetGenericArguments()).Invoke(this, new object[] { value, parent });
        }

        private TreeNode AddDequeletteNode<T>(Deque<T>.Dequelette dequelette, TreeNode parent)
        {
            var valueNode = new DequeletteNode<T>(dequelette);
            return treeContainer.AddNode(valueNode, parent);
        }

        TreeNode AddDequeNode<T>(IDeque<T> deque, TreeNode parent)
        {
            var dequeNode = new DequeNode<T>(deque);
            return parent == null ? treeContainer.AddRoot(dequeNode) : treeContainer.AddNode(dequeNode, parent);
        }

        void AddValueNode<T>(T value, TreeNode parent)
        {
            var valueNode = new ValueNode<T>(value);
            treeContainer.AddNode(valueNode, parent);
        }
    }
}