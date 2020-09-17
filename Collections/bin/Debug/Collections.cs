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
            return _items.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
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
            return _items.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
    public class Set<T> : IEnumerable<T>
    {
        private List<T> _items = new List<T>();
        public int Count => _items.Count;
        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (!_items.Contains(item))
            {
                _items.Add(item);
            }
        }
        public void Remove(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (!_items.Contains(item))
            {
                throw new KeyNotFoundException($"Элемент {item} не найден в множестве.");
            }

            _items.Remove(item);
        }
        public static Set<T> Union(Set<T> set1, Set<T> set2)
        {
            if (set1 == null)
            {
                throw new ArgumentNullException(nameof(set1));
            }

            if (set2 == null)
            {
                throw new ArgumentNullException(nameof(set2));
            }

            var resultSet = new Set<T>();
            var items = new List<T>();

            if (set1._items != null && set1._items.Count > 0)
            {
                items.AddRange(new List<T>(set1._items));
            }

            if (set2._items != null && set2._items.Count > 0)
            {
                items.AddRange(new List<T>(set2._items));
            }

            resultSet._items = items.Distinct().ToList();

            return resultSet;
        }
        public static Set<T> Intersection(Set<T> set1, Set<T> set2)
        {
            if (set1 == null)
            {
                throw new ArgumentNullException(nameof(set1));
            }

            if (set2 == null)
            {
                throw new ArgumentNullException(nameof(set2));
            }

            var resultSet = new Set<T>();

            if (set1.Count < set2.Count)
            {
                foreach (var item in set1._items)
                {
                    if (set2._items.Contains(item))
                    {
                        resultSet.Add(item);
                    }
                }
            }
            else
            {
                foreach (var item in set2._items)
                {
                    if (set1._items.Contains(item))
                    {
                        resultSet.Add(item);
                    }
                }
            }

            return resultSet;
        }
        public static Set<T> Difference(Set<T> set1, Set<T> set2)
        {
            if (set1 == null)
            {
                throw new ArgumentNullException(nameof(set1));
            }

            if (set2 == null)
            {
                throw new ArgumentNullException(nameof(set2));
            }

            var resultSet = new Set<T>();
            
            foreach (var item in set1._items)
            {
                if (!set2._items.Contains(item))
                {
                    resultSet.Add(item);
                }
            }

            foreach (var item in set2._items)
            {
                if (!set1._items.Contains(item))
                {
                    resultSet.Add(item);
                }
            }

            resultSet._items = resultSet._items.Distinct().ToList();

            return resultSet;
        }
        public static bool Subset(Set<T> set1, Set<T> set2)
        {
            if (set1 == null)
            {
                throw new ArgumentNullException(nameof(set1));
            }

            if (set2 == null)
            {
                throw new ArgumentNullException(nameof(set2));
            }

            var result = set1._items.All(s => set2._items.Contains(s));
            return result;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
    public class BinaryTree
        : IComparable,
          IComparable<BinaryTree>
    {
        private readonly int _value;

        public int Value => _value;

        public BinaryTree Left { get; private set; }
        public BinaryTree Right { get; private set; }

        public BinaryTree(int value)
        {
            _value = value;
        }

        public void Add(int value)
        {
            var newTreeNode = new BinaryTree(value);
            Add(newTreeNode);
        }

        public int CompareTo(object obj)
        {
            var binaryTree = obj as BinaryTree;
            if (binaryTree == null)
            {
                throw new ArgumentException($"Uncomparable argument {nameof(obj)} in CompareTo(object) method.", nameof(obj));
            }

            return CompareTo(binaryTree);
        }

        public int CompareTo(BinaryTree other)
        {
            if (other == null)
            {
                throw new ArgumentNullException($"Null argument {nameof(other)} in CompareTo(BinaryTree) method.", nameof(other));
            }

            return Value.CompareTo(other.Value);
        }

        public override bool Equals(object obj)
        {
            var binaryTree = obj as BinaryTree;
            if (binaryTree == null)
            {
                return false;
            }

            return Equals(binaryTree);
        }

        public bool Equals(BinaryTree other)
        {
            if (other == null)
            {
                return false;
            }

            if (Value != other.Value)
            {
                return false;
            }

            if ((Left ?? other.Left) != null)
            {
                var leftCompare = Left?.Equals(other.Left) ?? false;
                if (!leftCompare)
                    return false;
            }

            if ((Right ?? other.Right) != null)
            {
                return Right?.Equals(other.Right) ?? false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        protected void Add(BinaryTree node)
        {
            if (node.Equals(this))
            {
                return;
            }

            if (node.CompareTo(this) < 0)
            {
                if (Left == null)
                {
                    Left = node;
                }
                else
                {
                    Left.Add(node);
                }
            }
            else if (node.CompareTo(this) > 0)
            {
                if (Right == null)
                {
                    Right = node;
                }
                else
                {
                    Right.Add(node);
                }
            }
        }
    }
    namespace Map
    {
        public class Item<TKye, TValue>
        {
            public TKye Key { get; set; }
            public TValue Value { get; set; }
            public Item() { }
            public Item(TKye key, TValue value)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                Key = key;
                Value = value;
            }
            public override string ToString()
            {
                return Value.ToString();
            }
        }
        public class Map<TKey, TValue>
        {
            private List<Item<TKey, TValue>> _items = new List<Item<TKey, TValue>>();
            public int Count => _items.Count;
            public IReadOnlyList<TKey> Keys => (IReadOnlyList<TKey>)_items.Select(i => i.Key).ToList();
            public void Add(Item<TKey, TValue> item)
            {
                if (item == null)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                if (_items.Any(i => i.Key.Equals(item.Key)))
                {
                    throw new ArgumentException($"Словарь уже содержит значение с ключом {item.Key}.", nameof(item));
                }

                _items.Add(item);
            }
            public void Add(TKey key, TValue value)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (_items.Any(i => i.Key.Equals(key)))
                {
                    throw new ArgumentException($"Словарь уже содержит значение с ключом {key}.", nameof(key));
                }

                var item = new Item<TKey, TValue>()
                {
                    Key = key,
                    Value = value
                };

                _items.Add(item);
            }
            public void Remove(TKey key)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                var item = _items.SingleOrDefault(i => i.Key.Equals(key));

                if (item != null)
                {
                    _items.Remove(item);
                }
            }
            public void Update(TKey key, TValue newValue)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (newValue == null)
                {
                    throw new ArgumentNullException(nameof(newValue));
                }

                if (!_items.Any(i => i.Key.Equals(key)))
                {
                    throw new ArgumentException($"Словарь не содержит значение с ключом {key}.", nameof(key));
                }

                var item = _items.SingleOrDefault(i => i.Key.Equals(key));

                if (item != null)
                {
                    item.Value = newValue;
                }
            }
            public TValue Get(TKey key)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                var item = _items.SingleOrDefault(i => i.Key.Equals(key)) ??
                    throw new ArgumentException($"Словарь не содержит значение с ключом {key}.", nameof(key));

                return item.Value;
            }
        }
    }
    namespace LinkedList
    {
        public class Item<T>
        {
            public T Data { get; set; }
            public Item<T> Next { get; set; }
            public Item(T data)
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                Data = data;
            }
            public override string ToString()
            {
                return Data.ToString();
            }
        }
        public class LinkedList<T> : IEnumerable<T>
        {
            private Item<T> _head = null;
            private Item<T> _tail = null;
            private int _count = 0;
            public int Count
            {
                get => _count;
            }
            public void Add(T data)
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                var item = new Item<T>(data);

                if (_head == null)
                {
                    _head = item;
                }
                else
                {
                    _tail.Next = item;
                }

                _tail = item;

                _count++;
            }
            public void Delete(T data)
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                var current = _head;

                Item<T> previous = null;

                while (current != null)
                {
                    if (current.Data.Equals(data))
                    {
                        if (previous != null)
                        {
                            previous.Next = current.Next;

                            if (current.Next == null)
                            {
                                _tail = previous;
                            }
                        }
                        else
                        {
                            _head = _head.Next;

                            if (_head == null)
                            {
                                _tail = null;
                            }
                        }

                        _count--;
                        break;
                    }

                    previous = current;
                    current = current.Next;
                }
            }
            public void Clear()
            {
                _head = null;
                _tail = null;
                _count = 0;
            }
            public IEnumerator<T> GetEnumerator()
            {
                var current = _head;
                while (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this).GetEnumerator();
            }
        }
    }
}
