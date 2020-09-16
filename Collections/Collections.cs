using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Collections
{
    public class Stack<T> : IEnumerable<T>
    {
        private List<T> _items = new List<T>();
        public int Count => _items.Count;
        public Stack() { }
        public Stack(IEnumerable<T> Collection)
        {
            foreach (var item in Collection)
            {
                _items.Add(item);
            }
        }
        public void Push(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
        }
        public T Pop()
        {
            var item = _items.LastOrDefault();

            if (item == null)
            {
                throw new NullReferenceException("Stack is empty!\n");
            }

            _items.RemoveAt(_items.Count - 1);
            return item;
        }
        public T Peek()
        {
            var item = _items.LastOrDefault();

            if (item == null)
            {
                throw new NullReferenceException("Stack is empty!\n");
            }

            return item;
        }
        public void Clear()
        {
            _items.Clear();
        }
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in _items)
            {
                yield return item;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class Queue<T> : IEnumerable<T>
    {
        private List<T> _items = new List<T>();
        public int Count => _items.Count;
        public Queue() { }
        public Queue(IEnumerable<T> Collection)
        {
            foreach (var item in Collection)
            {
                _items.Insert(0, item);
            }
        }
        public void Enqueue(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Insert(0, item);
        }
        public T Dequeue()
        {
            var item = _items.LastOrDefault();

            if (item == null)
            {
                throw new NullReferenceException("Stack is empty!\n");
            }

            _items.RemoveAt(_items.Count - 1);
            return item;
        }
        public T Peek()
        {
            var item = _items.LastOrDefault();

            if (item == null)
            {
                throw new NullReferenceException("Stack is empty!\n");
            }

            return item;
        }
        public void Clear()
        {
            _items.Clear();
        }
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in _items)
            {
                yield return item;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
