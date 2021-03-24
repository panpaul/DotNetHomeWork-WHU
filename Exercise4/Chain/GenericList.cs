using System;

namespace Chain
{
    public class GenericList<T>
    {
        public GenericList()
        {
            Head = Tail = null;
        }

        public Node<T> Head { get; private set; }

        public Node<T> Tail { get; private set; }

        public void Add(T t)
        {
            var n = new Node<T>(t);
            if (Tail == null)
            {
                Head = Tail = n;
            }
            else
            {
                Tail.Next = n;
                Tail = n;
            }
        }

        public void ForEach(Action<T> action)
        {
            if (action == null) throw new ArgumentNullException();

            var head = Head;
            while (head != null)
            {
                action(head.Data);
                head = head.Next;
            }
        }
    }

    public class Node<T>
    {
        public Node(T t)
        {
            Next = null;
            Data = t;
        }

        public Node<T> Next { get; set; }
        public T Data { get; set; }
    }
}