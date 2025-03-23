using Newtonsoft.Json;
using System.Collections;

namespace CPUCalculator2.Data;

public class Table<T> : IEnumerable<T>
    where T : class
{
    public Table()
    {
        Items = [];
        Reload();
    }

    private T[] Items { get; set; }

    public string Filename => typeof(T).Name + "s.json";
    public void Reload()
    {
        lock (Items)
        {
            if (!File.Exists(Filename))
            {
                Items = [];
                return;
            }
            var itemsjson = File.ReadAllText(Filename);
            var items = JsonConvert.DeserializeObject<T[]>(itemsjson);
            if (items == null)
                Items = [];
            else
                Items = items!;
        }
    }
    public void SaveChanges()
    {
        lock (Items)
        {
            var json = JsonConvert.SerializeObject(Items);
            File.WriteAllText(Filename, json);
        }
    }
    public void Add(T cpu)
    {
        lock (Items)
        {
            var newArray = new T[Items.Length + 1];
            Items.CopyTo(newArray, 0);
            newArray[Items.Length] = cpu;
            Items = newArray;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in Items)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Items.GetEnumerator();
    }
}
