using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laba10;

namespace Laba13
{
    public class MyCollection<T> : Point<T>, IEnumerable<T>, ICollection<T> where T : IInit, IComparable, ICloneable, new()
    {
        public Point<T>? root = null;

        int count = 0;

        public int Count => count;

        public bool IsReadOnly => throw new NotImplementedException();

        public MyCollection() { }

        public MyCollection(int length)
        {
            count = length;
            T data = new T();
            for (int i = 0; i < length; i++)
            {
                data.RandomInit();
                Add(data);
                data = new T();
            }
        }

        public MyCollection(MyCollection<T> collection)
        {
            foreach (T item in collection)
            {
                this.Insert((T)item.Clone());
            }
        }

        int Height(Point<T> root)
        {
            if (root == null)
                return 0;
            return Math.Max(Height(root.Left), Height(root.Right)) + 1;
        }

        public int GetTreeHeight()
        {
            return Height(root);
        }

        void SetBalance(Point<T> root)
        {
            if (root != null)
                root.Balance = Height(root.Right) - Height(root.Left);
        }

        Point<T> TurnLeft(Point<T> root)
        {
            Point<T> rightSubtree, rightSubtreeLeftSubtree;
            rightSubtree = root.Right;
            rightSubtreeLeftSubtree = rightSubtree.Left;

            rightSubtree.Left = root;
            root.Right = rightSubtreeLeftSubtree;
            root = rightSubtree;
            SetBalance(root.Left);
            SetBalance(root);
            return root;
        }

        Point<T> TurnRight(Point<T> root)
        {
            Point<T> leftSubtree, leftSubtreeRightSubtree;
            leftSubtree = root.Left;
            leftSubtreeRightSubtree = leftSubtree.Right;

            leftSubtree.Right = root;
            root.Left = leftSubtreeRightSubtree;
            root = leftSubtree;
            SetBalance(root.Right);
            SetBalance(root);
            return root;
        }

        public void Insert(T data)
        {
            root = Insert(root, data);
        }

        public Point<T> Insert(Point<T> root, T d)
        {
            if (root == null)
            {
                root = new Point<T>(d);
                root.Balance = 0;
                root.Right = null;
                root.Left = null;
                return root;
            }
            else
            {
                if (d.CompareTo(root.Data) > 0)
                    root.Right = Insert(root.Right, d);
                if (Height(root.Right) - Height(root.Left) > 1)
                {
                    if (Height(root.Right.Right) < Height(root.Right.Left))
                        root.Right = TurnRight(root.Right);
                    root = TurnLeft(root);
                }
                else
                {
                    if (d.CompareTo(root.Data) < 0)
                    {
                        root.Left = Insert(root.Left, d);
                        if (Height(root.Left) - Height(root.Right) > 1)
                        {
                            if (Height(root.Left.Left) < Height(root.Left.Right))
                                root.Left = TurnLeft(root.Left);
                            root = TurnRight(root);
                        }
                    }
                }
            }
            return root;
        }
        public void Delete(T data)
        {
            root = Delete(root, data);
        }
        public Point<T>? Delete(Point<T>? root, T data)
        {
            if (root == null)
                return root;

            if (data.CompareTo(root.Data) < 0)
            {
                root.Left = Delete(root.Left, data);
            }
            else if (data.CompareTo(root.Data) > 0)
            {
                root.Right = Delete(root.Right, data);
            }
            else
            {
                if ((root.Left == null) || (root.Right == null))
                {
                    Point<T>? temp = root.Left ?? root.Right;
                    if (temp == null)
                    {
                        temp = root;
                        root = null;
                    }
                    else
                        root = temp;
                }
                else
                {
                    Point<T> temp = GetMinValueNode(root.Right);
                    root.Data = temp.Data;
                    root.Right = Delete(root.Right, temp.Data);
                }
            }

            if (root == null)
                return root;

            SetBalance(root);

            if (root.Balance > 1)
            {
                if (Height(root.Right.Right) < Height(root.Right.Left))
                    root.Right = TurnRight(root.Right);
                root = TurnLeft(root);
            }
            else if (root.Balance < -1)
            {
                if (Height(root.Left.Left) < Height(root.Left.Right))
                    root.Left = TurnLeft(root.Left);
                root = TurnRight(root);
            }

            return root;
        }
        public virtual void Add(T data)
        {
            root = Insert(root, data);
        }

        public void Clear()
        {
            root = null;
            count = 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (T item in this)
            {
                if (array.Length <= arrayIndex)
                {
                    T[] temp = new T[array.Length * 2];
                    for (int i = 0; i < array.Length; i++)
                    {
                        temp[i] = array[i];
                    }
                    array = temp;
                }
                array[arrayIndex] = (T)item.Clone();
                arrayIndex++;
            };
        }

        public virtual bool Remove(T data)
        {
            root = Delete(root, data);
            return true;
        }

        public bool Contains(T data)
        {
            return Contains(root, data);
        }

        private bool Contains(Point<T>? node, T data)
        {
            if (node == null)
                return false;
            if (data.CompareTo(node.Data) == 0)
                return true;
            if (data.CompareTo(node.Data) < 0)
                return Contains(node.Left, data);
            else
                return Contains(node.Right, data);
        }

        public void ShowTree()
        {
            Show(root);
        }

        private void Show(Point<T> root, int spaces = 0)
        {
            if (root != null)
            {
                Show(root.Right, spaces + 5);

                for (int i = 0; i < spaces; i++)
                {
                    Console.Write(" ");
                }

                Console.WriteLine(root.Data);

                Show(root.Left, spaces + 5);
            }
        }
        public Point<T> ShowMinElem(MyCollection<T> tree)
        {
            return GetMinValueNode(root);
        }
        Point<T> GetMinValueNode(Point<T> root)
        {
            Point<T> current = root;
            while (current.Left != null)
                current = current.Left;
            return current;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InOrder(this.root).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> InOrder(Point<T>? node)
        {
            if (node != null)
            {
                foreach (T item in InOrder(node.Left))
                {
                    yield return item;
                }

                yield return node.Data;

                foreach (T item in InOrder(node.Right))
                {
                    yield return item;
                }
            }
        }
        public void DeleteTree()
        {
            root = null;
            count = 0;
            Console.WriteLine("Дерево удалено!");
        }
        public virtual T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
                }
                return GetElementAtIndex(root, index);
            }
            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
                }
                SetElementAtIndex(root, index, value);
            }
        }

        private T GetElementAtIndex(Point<T>? node, int index)
        {
            int leftCount = CountNodes(node.Left);
            if (index < leftCount)
            {
                return GetElementAtIndex(node.Left, index);
            }
            else if (index == leftCount)
            {
                return node.Data;
            }
            else
            {
                return GetElementAtIndex(node.Right, index - leftCount - 1);
            }
        }

        private void SetElementAtIndex(Point<T>? node, int index, T value)
        {
            int leftCount = CountNodes(node.Left);
            if (index < leftCount)
            {
                SetElementAtIndex(node.Left, index, value);
            }
            else if (index == leftCount)
            {
                node.Data = value;
            }
            else
            {
                SetElementAtIndex(node.Right, index - leftCount - 1, value);
            }
        }

        private int CountNodes(Point<T>? node)
        {
            if (node == null)
            {
                return 0;
            }
            return 1 + CountNodes(node.Left) + CountNodes(node.Right);
        }

        internal class MyEnumerator : IEnumerator<T>
        {
            private List<T> items;
            private int position = -1;

            public MyEnumerator(MyCollection<T> collection)
            {
                items = new List<T>();
                foreach (var item in collection)
                {
                    items.Add(item);
                }
            }

            public T Current => items[position];

            object IEnumerator.Current => Current;

            public IEnumerable<T> InOrder(Point<T>? node)
            {
                if (node != null)
                {
                    foreach (T item in InOrder(node.Left))
                    {
                        yield return item;
                    }

                    yield return node.Data;

                    foreach (T item in InOrder(node.Right))
                    {
                        yield return item;
                    }
                }
            }

            public bool MoveNext()
            {
                position++;
                return (position < items.Count);
            }

            public void Reset()
            {
                position = -1;
            }

            public void Dispose()
            {

            }
        }
    }
}
