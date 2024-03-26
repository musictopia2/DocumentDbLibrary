namespace DocumentDbLibrary;
public interface ISourceGeneratedSerializeDataAccess<T> : ISqlDocumentConfiguration
{
    static abstract EnumSourceGeneratedSerializeOptions SerializationOptions { get; }
}