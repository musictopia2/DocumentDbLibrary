namespace DocumentDbLibrary;
#if NET6_0_OR_GREATER
public abstract class DateOnlyDataAccess : BaseSimpleTypesDataAccess<DateOnly>
{
    public DateOnlyDataAccess(string databaseName, string collectionName) : base(databaseName, collectionName)
    {

    }
    public DateOnlyDataAccess(string databaseName, string collectionName, string path) : base(databaseName, collectionName, path)
    {

    }
    protected override DateOnly GetResults(string text)
    {
        return DateOnly.Parse(text);
    }
}
#endif