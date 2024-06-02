using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba13
{
    public class Point<T> where T : IComparable
    {
        public T? Data { get; set; }
        public Point<T>? Left { get; set; }
        public Point<T>? Right { get; set; }
        public int Balance { get; set; }

        public Point()
        {
            this.Data = default(T);
            this.Left = null;
            this.Right = null;
            this.Balance = 0;
        }

        public Point(T data)
        {
            this.Data = data;
            this.Left = null;
            this.Right = null;
            this.Balance = 0;
        }

        public override string ToString()
        {
            return Data == null ? "" : Data.ToString();
        }

        public int CompareTo(Point<T> other)
        {
            return Data.CompareTo(other.Data);
        }
    }
}
