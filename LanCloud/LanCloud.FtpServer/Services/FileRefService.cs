using LanCloud.Domain.IO;
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
                        //writer.Write(bit.Length);
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
                var res = new FileRef();
                res.Length = reader.ReadInt64();
                if (res.Length == -1) res.Length = null;
                res.Hash = reader.ReadString();
                res.Bits = new FileRefBit[reader.ReadByte()];
                for (int i = 0; i < res.Bits.Length; i++)
                {
                    res.Bits[i] = new FileRefBit();
                    //res.Bits[i].Length = reader.ReadInt64();
                    res.Bits[i].Indexes = new byte[reader.ReadByte()];
                    for (int j = 0; j < res.Bits[i].Indexes.Length; j++)
                    {
                        res.Bits[i].Indexes[j] = reader.ReadByte();
                    }
                }
                return res;
            }
        }

    }
}
