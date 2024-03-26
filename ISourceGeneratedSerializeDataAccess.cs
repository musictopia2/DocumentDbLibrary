namespace DocumentDbLibrary;
public interface ISourceGeneratedSerializeDataAccess<T> : ISourceGeneratedDataAccess<T>
{
    static abstract EnumSourceGeneratedSerializeOptions SerializationOptions { get; }
}