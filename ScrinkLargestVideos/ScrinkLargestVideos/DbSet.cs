using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ScrinkLargestVideos
{
    public class DbSet<T> : IEnumerable<T>, IDisposable
    {
        public DbSet(string fullName)
        {
            DataStream = File.Open($"{fullName}.data", FileMode.OpenOrCreate);
            DataReader = new BinaryReader(DataStream);
            Queue = new Queue<T>();

            while (DataStream.Position < DataStream.Length)
            {
                var json = DataReader.ReadString();
                var item = JsonConvert.DeserializeObject<T>(json);
                Queue.Enqueue(item);
                Count++;
            }

            DataWriter = new BinaryWriter(DataStream);
        }

        FileStream DataStream;
        BinaryReader DataReader;
        BinaryWriter DataWriter;
        Queue<T> Queue;

        public int Count { get; private set; }

        public void Add(T item)
        {
            Queue.Enqueue(item);
            Count++;
            var json = JsonConvert.SerializeObject(item);
            DataWriter.Write(json);
        }

        internal void Remove(FileRapport file)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            var json = JsonConvert.SerializeObject(item);
            foreach (var queuedItem in Queue)
            {
                var queuedItemJson = JsonConvert.SerializeObject(queuedItem);
                if (queuedItemJson == json)
                {
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            DataWriter.Dispose();
            DataReader.Dispose();
            DataStream.Dispose();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}