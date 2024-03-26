namespace DocumentDbLibrary;
public abstract class EnumStandardDataAccess<E> : BaseSimpleTypesDataAccess<E>
    where E : struct, Enum
{
    public EnumStandardDataAccess(string databaseName, string collectionName) : base(databaseName, collectionName)
    {

    }
    public EnumStandardDataAccess(string databaseName, string collectionName, string path) : base(databaseName, collectionName, path)
    {

    }
    protected override E GetResults(string text)
    {
        var output = Enum.Parse<E>(text);
        return output;
    }
}