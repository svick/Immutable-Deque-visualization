using System;

// code from http://blogs.msdn.com/b/ericlippert/archive/2008/02/12/deque-cs.aspx

namespace Immutable_Deque_visualization
{
    public interface IDeque<T>
    {
        T PeekLeft();
        T PeekRight();
        IDeque<T> EnqueueLeft(T value);
        IDeque<T> EnqueueRight(T value);
        IDeque<T> DequeueLeft();
        IDeque<T> DequeueRight();
        bool IsEmpty { get; }
    }

    public sealed class Deque<T> : IDeque<T>
    {
        private sealed class EmptyDeque : IDeque<T>
        {
            public bool IsEmpty { get { return true; } }
            public IDeque<T> EnqueueLeft(T value) { return new SingleDeque(value); }
            public IDeque<T> EnqueueRight(T value) { return new SingleDeque(value); }
            public IDeque<T> DequeueLeft() { throw new Exception("empty deque"); }
            public IDeque<T> DequeueRight() { throw new Exception("empty deque"); }
            public T PeekLeft() { throw new Exception("empty deque"); }
            public T PeekRight() { throw new Exception("empty deque"); }
        }
        public sealed class SingleDeque : IDeque<T>
        {
            public SingleDeque(T t)
            {
                Item = t;
            }
            public readonly T Item;
            public bool IsEmpty { get { return false; } }
            public IDeque<T> EnqueueLeft(T value)
            {
                return new Deque<T>(new One(value), Deque<Dequelette>.Empty, new One(Item));
            }
            public IDeque<T> EnqueueRight(T value)
            {
                return new Deque<T>(new One(Item), Deque<Dequelette>.Empty, new One(value));
            }
            public IDeque<T> DequeueLeft() { return Empty; }
            public IDeque<T> DequeueRight() { return Empty; }
            public T PeekLeft() { return Item; }
            public T PeekRight() { return Item; }
        }

        public abstract class Dequelette
        {
            public abstract int Size { get; }
            public virtual bool Full { get { return false; } }
            public abstract T PeekLeft();
            public abstract T PeekRight();
            public abstract Dequelette EnqueueLeft(T t);
            public abstract Dequelette EnqueueRight(T t);
            public abstract Dequelette DequeueLeft();
            public abstract Dequelette DequeueRight();
        }
        public class One : Dequelette
        {
            public One(T t1)
            {
                V1 = t1;
            }
            public override int Size { get { return 1; } }
            public override T PeekLeft() { return V1; }
            public override T PeekRight() { return V1; }
            public override Dequelette EnqueueLeft(T t) { return new Two(t, V1); }
            public override Dequelette EnqueueRight(T t) { return new Two(V1, t); }
            public override Dequelette DequeueLeft() { throw new Exception("Impossible"); }
            public override Dequelette DequeueRight() { throw new Exception("Impossible"); }
            public readonly T V1;
        }
        public class Two : Dequelette
        {
            public Two(T t1, T t2)
            {
                V1 = t1;
                V2 = t2;
            }
            public override int Size { get { return 2; } }
            public override T PeekLeft() { return V1; }
            public override T PeekRight() { return V2; }
            public override Dequelette EnqueueLeft(T t) { return new Three(t, V1, V2); }
            public override Dequelette EnqueueRight(T t) { return new Three(V1, V2, t); }
            public override Dequelette DequeueLeft() { return new One(V2); }
            public override Dequelette DequeueRight() { return new One(V1); }
            public readonly T V1;
            public readonly T V2;
        }
        public class Three : Dequelette
        {
            public Three(T t1, T t2, T t3)
            {
                V1 = t1;
                V2 = t2;
                V3 = t3;
            }
            public override int Size { get { return 3; } }
            public override T PeekLeft() { return V1; }
            public override T PeekRight() { return V3; }
            public override Dequelette EnqueueLeft(T t) { return new Four(t, V1, V2, V3); }
            public override Dequelette EnqueueRight(T t) { return new Four(V1, V2, V3, t); }
            public override Dequelette DequeueLeft() { return new Two(V2, V3); }
            public override Dequelette DequeueRight() { return new Two(V1, V2); }
            public readonly T V1;
            public readonly T V2;
            public readonly T V3;
        }
        public class Four : Dequelette
        {
            public Four(T t1, T t2, T t3, T t4)
            {
                V1 = t1;
                V2 = t2;
                V3 = t3;
                V4 = t4;
            }
            public override int Size { get { return 4; } }
            public override bool Full { get { return true; } }
            public override T PeekLeft() { return V1; }
            public override T PeekRight() { return V4; }
            public override Dequelette EnqueueLeft(T t) { throw new Exception("Impossible"); }
            public override Dequelette EnqueueRight(T t) { throw new Exception("Impossible"); }
            public override Dequelette DequeueLeft() { return new Three(V2, V3, V4); }
            public override Dequelette DequeueRight() { return new Three(V1, V2, V3); }
            public readonly T V1;
            public readonly T V2;
            public readonly T V3;
            public readonly T V4;
        }

        private static readonly IDeque<T> empty = new EmptyDeque();
        public static IDeque<T> Empty { get { return empty; } }

        public bool IsEmpty { get { return false; } }

        private Deque(Dequelette left, IDeque<Dequelette> middle, Dequelette right)
        {
            Left = left;
            Middle = middle;
            Right = right;
        }

        public readonly Dequelette Left;
        public readonly IDeque<Dequelette> Middle;
        public readonly Dequelette Right;

        public IDeque<T> EnqueueLeft(T value)
        {
            if (!Left.Full)
                return new Deque<T>(Left.EnqueueLeft(value), Middle, Right);
            return new Deque<T>(
                new Two(value, Left.PeekLeft()),
                Middle.EnqueueLeft(Left.DequeueLeft()),
                Right);
        }
        public IDeque<T> EnqueueRight(T value)
        {
            if (!Right.Full)
                return new Deque<T>(Left, Middle, Right.EnqueueRight(value));
            return new Deque<T>(
                Left,
                Middle.EnqueueRight(Right.DequeueRight()),
                new Two(Right.PeekRight(), value));
        }
        public IDeque<T> DequeueLeft()
        {
            if (Left.Size > 1)
                return new Deque<T>(Left.DequeueLeft(), Middle, Right);
            if (!Middle.IsEmpty)
                return new Deque<T>(Middle.PeekLeft(), Middle.DequeueLeft(), Right);
            if (Right.Size > 1)
                return new Deque<T>(new One(Right.PeekLeft()), Middle, Right.DequeueLeft());
            return new SingleDeque(Right.PeekLeft());
        }
        public IDeque<T> DequeueRight()
        {
            if (Right.Size > 1)
                return new Deque<T>(Left, Middle, Right.DequeueRight());
            if (!Middle.IsEmpty)
                return new Deque<T>(Left, Middle.DequeueRight(), Middle.PeekRight());
            if (Left.Size > 1)
                return new Deque<T>(Left.DequeueRight(), Middle, new One(Left.PeekRight()));
            return new SingleDeque(Left.PeekRight());
        }
        public T PeekLeft() { return Left.PeekLeft(); }
        public T PeekRight() { return Right.PeekRight(); }
    }
}