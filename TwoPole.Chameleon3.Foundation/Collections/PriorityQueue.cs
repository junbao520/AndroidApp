using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Foundation
{
    public class PriorityQueue<T> : IPriorityQueue<T>
    {
        private readonly List<T> _items;
        private readonly IComparer<T> _comparer;

        public PriorityQueue()
            :this(Comparer<T>.Default)
        {
            _items = new List<T>();
        }

        public PriorityQueue(IComparer<T> comparer)
        {
            _items = new List<T>();
            _comparer = comparer;
        }

        public bool Empty { get { return _items.Count == 0; } }

        public void Enqueue(T x)
        {
            _items.Add(x);
            _items.Sort(_comparer);
        }

        public T Dequeue()
        {
            var max = Top;
            _items.RemoveAt(0);
            return max;
        }
        public T Top { get { return _items.First(); } }
        public void Clear() { _items.Clear(); }
        public int Count { get { return _items.Count; } }
    }
}
