using Laba10;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laba12_3;
using Laba10;

namespace Laba13
{
    public class MyObservableCollection<T> : MyCollection<T> where T : IInit, IComparable, ICloneable, new()
    {
        public delegate void CollectionHandler(object source, CollectionHandlerEventArgs args);

        public event CollectionHandler CollectionCountChanged;
        public event CollectionHandler CollectionReferenceChanged;

        public MyObservableCollection() : base() { }

        public MyObservableCollection(int length) : base(length) { }

        public override void Add(T data)
        {
            base.Add(data);
            CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs("Добавление", data));
        }

        public override bool Remove(T data)
        {
            bool result = base.Remove(data);
            if (result)
            {
                CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs("Удаление", data));
            }
            return result;
        }

        public override T this[int index]
        {
            get => base[index];
            set
            {
                base[index] = value;
                CollectionReferenceChanged?.Invoke(this, new CollectionHandlerEventArgs("Изменение", value));
            }
        }
    }
}
