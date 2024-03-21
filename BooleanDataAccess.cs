namespace DocumentDbLibrary;
public abstract class BooleanDataAccess : BaseSimpleTypesDataAccess<bool>
{
    public BooleanDataAccess(string databaseName, string collectionName) : base(databaseName, collectionName)
    {

    }
    public BooleanDataAccess(string databaseName, string collectionName, string path) : base(databaseName, collectionName, path)
    {

    }
    protected override bool GetResults(string text)
    {
        return bool.Parse(text);
    }
}