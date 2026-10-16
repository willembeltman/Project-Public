using gAPI.Core.Attributes;
using gAPI.Core.AttributesSerializers;
using Pgvector;
using System.Runtime.InteropServices;

namespace Rekenkamer.Shared.Serializers;

public static class VectorSerializer
{
    [IsSpanSerializerWrite]
    public static void WriteVector(ref Span<byte> span, ref int offset, Vector value)
    {
        float[] floatArray = value.ToArray();
        int dimensions = floatArray.Length;

        // 1. Schrijf het aantal dimensies (4 bytes)
        System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset, 4), dimensions);
        offset += 4;

        // 2. Schrijf de vector floats naar bytes via MemoryMarshal.Cast
        Span<byte> targetSpan = span.Slice(offset, dimensions * 4);
        Span<float> sourceSpan = floatArray.AsSpan();
        MemoryMarshal.Cast<float, byte>(sourceSpan).CopyTo(targetSpan);

        offset += dimensions * 4;
    }

    [IsSpanSerializerRead]
    public static Vector ReadVector(ReadOnlySpan<byte> span, ref int offset)
    {
        // 1. Lees het aantal dimensies
        int dimensions = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span.Slice(offset, 4));
        offset += 4;

        // 2. Cast de byte-span direct naar een float-span en zet om naar een array
        ReadOnlySpan<byte> byteSpan = span.Slice(offset, dimensions * 4);
        ReadOnlySpan<float> floatSpan = MemoryMarshal.Cast<byte, float>(byteSpan);
        float[] floatArray = floatSpan.ToArray();

        offset += dimensions * 4;
        return new Vector(floatArray);
    }

    [IsSerializerRead]
    public static Vector ReadVector(this BinaryReader br)
    {
        int dimensions = br.ReadInt32();
        byte[] bytes = br.ReadBytes(dimensions * 4);

        // Veilig en snel casten van byte array naar float array
        float[] floatArray = new float[dimensions];
        Buffer.BlockCopy(bytes, 0, floatArray, 0, bytes.Length);

        return new Vector(floatArray);
    }

    [IsSerializerWrite]
    public static void Write(this BinaryWriter bw, Vector value)
    {
        float[] floatArray = value.ToArray();
        bw.Write(floatArray.Length); // Altijd eerst de dimensies schrijven

        byte[] byteArray = new byte[floatArray.Length * 4];
        Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);
        bw.Write(byteArray);
    }

    [IsComparer]
    public static bool IsDifferent(this Vector value, Vector otherValue)
    {
        if (value == null || otherValue == null) return !ReferenceEquals(value, otherValue);
        return !value.Equals(otherValue);
    }

    [IsSpanSerializerLength]
    public static int GetMessageLength(ref int offset, Vector value)
    {
        // 4 bytes voor de dimensie-indicator + (dimensies * 4 bytes per float)
        int length = 4 + (value.ToArray().Length * 4);
        offset += length;
        return offset;
    }

    [IsMultipartFormDataContentSerializer]
    public static void Write(this MultipartFormDataContent content, string propertyName, Vector value)
    {
        content.Add(new StringContent(value.ToString()), propertyName);
    }
}
