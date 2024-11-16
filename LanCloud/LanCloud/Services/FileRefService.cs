using LanCloud.Models;
using System;
using System.IO;

namespace LanCloud.Services
{
    public static class FileRefService
    {
        public static FileRef Save(FileInfo fileInfo, FileRef fileRef)
        {
            if (!fileInfo.Exists) fileInfo.Delete();
            using (var stream = fileInfo.OpenWrite())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(fileRef.Length ?? -1);
                writer.Write(fileRef.Hash ?? "");
                writer.Write(Convert.ToByte(fileRef.Bits?.Length ?? 0));
                if (fileRef.Bits != null)
                {
                    foreach (var bit in fileRef.Bits)
                    {
                        writer.Write(Convert.ToByte(bit.Indexes.Length));
                        foreach (var index in bit.Indexes)
                        {
                            writer.Write(index);
                        }
                    }
                }
            }
            return fileRef;
        }

        public static FileRef Load(FileInfo fileInfo)
        {
            if (!fileInfo.Exists) return null;
            using (var stream = fileInfo.OpenRead())
            using (var reader = new BinaryReader(stream))
            {
                long? Length = reader.ReadInt64();
                if (Length == -1) Length = null;
                var Hash = reader.ReadString();
                var Bits = new FileRefStripe[reader.ReadByte()];
                for (int i = 0; i < Bits.Length; i++)
                {
                    var Indexes = new byte[reader.ReadByte()];
                    for (int j = 0; j < Indexes.Length; j++)
                    {
                        Indexes[j] = reader.ReadByte();
                    }
                    Bits[i] = new FileRefStripe(Indexes);
                }
                return new FileRef(Length, Hash, Bits);
            }
        }

    }
}
