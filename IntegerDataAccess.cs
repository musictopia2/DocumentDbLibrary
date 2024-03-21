namespace DocumentDbLibrary;
public abstract class IntegerDataAccess : BaseSimpleTypesDataAccess<int>
{
    public IntegerDataAccess(string databaseName, string collectionName) : base(databaseName, collectionName)
    {

    }
    public IntegerDataAccess(string databaseName, string collectionName, string path) : base(databaseName, collectionName, path)
    {

    }
    protected override int GetResults(string text)
    {
        return int.Parse(text);
    }
}