namespace DocumentDbLibrary;
public abstract class DateTimeDataAccess : BaseSimpleTypesDataAccess<DateTime>
{
    public DateTimeDataAccess(string databaseName, string collectionName) : base(databaseName, collectionName)
    {

    }
    public DateTimeDataAccess(string databaseName, string collectionName, string path) : base(databaseName, collectionName, path)
    {

    }
    protected override DateTime GetResults(string text)
    {
        return DateTime.Parse(text);
    }
}