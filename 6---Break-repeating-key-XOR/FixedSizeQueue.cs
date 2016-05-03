using System.Collections.Generic;
using System.Linq;

namespace sandbox2
{
    public class FixedSizeQueue<T>
    {
        readonly Queue<T> _queue = new Queue<T>();
        private readonly int _size;

        public FixedSizeQueue(int size)
        {
            _size = size;
        }

        public void Enqueue(T item)
        {
            if (_queue.Count >= _size)
            {
                _queue.Dequeue();
            }
            _queue.Enqueue(item);
        }

        public T Dequeue()
        {
            return _queue.Dequeue();
        }

        public IEnumerable<T> BestEnumeration
        {
            get { return _queue.Reverse(); }
        }

        public List<T> Results
        {
            get { return _queue.ToList(); }
        }

        public T Best
        {
            get { return _queue.LastOrDefault(); }

        }
    }
}
