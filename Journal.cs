using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab12_3;

namespace Laba13
{
    public class JournalEntry
    {
        public string CollectionName { get; set; }
        public string ChangeType { get; set; }
        public string ChangedItemInfo { get; set; }

        public JournalEntry(string collectionName, string changeType, string itemInfo)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            ChangedItemInfo = itemInfo;
        }

        public override string ToString()
        {
            return $"Коллекция: {CollectionName}, Тип изменения: {ChangeType}, Информация об объекте: {ChangedItemInfo}";
        }
    }

    public class Journal
    {
        public bool IsEmpty()
        {
            return !entries.Any();
        }

        public List<JournalEntry> entries = new List<JournalEntry>();

        public void AddEntry(object source, CollectionHandlerEventArgs args)
        {
            string collectionName = source.GetType().Name;
            entries.Add(new JournalEntry(collectionName, args.ChangeInfo, args.ChangedItem.ToString()));
        }

        public override string ToString()
        {
            string result = "";
            foreach (var entry in entries)
            {
                result += entry.ToString() + "\n";
            }
            return result;
        }
    }
}
