namespace DocumentDbLibrary;
public abstract class StringDataAccess : BaseSimpleTypesDataAccess<string>
{
    public StringDataAccess(string databaseName, string collectionName) : base(databaseName, collectionName)
    {

    }
    public StringDataAccess(string databaseName, string collectionName, string path) : base(databaseName, collectionName, path)
    {

    }
    protected override string GetResults(string text)
    {
        return text;
    }
}
