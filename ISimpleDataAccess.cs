namespace DocumentDbLibrary;
public interface ISimpleDataAccess<T> : ISourceGeneratedDataAccess<T>
{
    abstract static T DefaultValue { get; }
}