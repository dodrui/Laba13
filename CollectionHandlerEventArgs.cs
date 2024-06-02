using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba13
{
    public class CollectionHandlerEventArgs : EventArgs
    {
        public string ChangeInfo { get; set; }
        public object ChangedItem { get; set; }

        public CollectionHandlerEventArgs(string info, object item)
        {
            ChangeInfo = info;
            ChangedItem = item;
        }
    }
}
