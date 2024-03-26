namespace DocumentDbLibrary;
public interface ISimpleDataAccess<T> : ISqlDocumentConfiguration
{
    abstract static T DefaultValue { get; }
}