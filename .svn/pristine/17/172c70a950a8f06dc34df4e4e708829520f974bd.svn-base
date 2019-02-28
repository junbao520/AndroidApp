using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Foundation
{
    public interface IPriorityQueue<T, in TK> 
        where TK : IComparable<TK>
    {
        int Count { get; }
        bool Empty { get; }
        void Enqueue(T x, TK key);
        T Dequeue();
        T Top { get; }
        void Clear();
    }

    public interface IPriorityQueue<T>
    {
        int Count { get; }
        bool Empty { get; }
        void Enqueue(T x);
        T Dequeue();
        T Top { get; }
        void Clear();
    }
}
