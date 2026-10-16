namespace Rekenkamer.Shared.Dtos;

public record UploadFile(
    string Name, 
    string RelativePath, 
    byte[] Bytes);