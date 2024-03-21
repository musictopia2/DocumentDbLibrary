namespace DocumentDbLibrary;
public abstract class BaseSimpleTypesDataAccess<T>
{
    protected abstract T DefaultValue { get; } //will be just a get.
    private DocumentContext? _context;
    public BaseSimpleTypesDataAccess(string databaseName, string collectionName)
    {
        Init(databaseName, collectionName, "");
    }
    public BaseSimpleTypesDataAccess(string databaseName, string collectionName, string path)
    {
        Init(databaseName, collectionName, path);
    }
    private void Init(string databaseName, string collectionName, string path)
    {
        _context = new(databaseName, collectionName, path);
    }
    protected abstract T GetResults(string text);
    protected async Task<T> GetDocumentsAsync() //for now, just make public.  its only for testing until i figure out how i should make this work.
    {
        string data = await _context!.GetDocumentAsync();
        if (string.IsNullOrWhiteSpace(data))
        {
            await UpsertRecordsAsync(DefaultValue); //make sure to add it if it does not exist.
            return DefaultValue;
        }
        //this should not need to serialize this.
        return GetResults(data);
        //BasicList<T> output = await jj1.DeserializeObjectAsync<BasicList<T>>(data);
        //return output;
    }
    protected async Task UpsertRecordsAsync(T payLoad)
    {
        string content = payLoad!.ToString()!; 
        //string content = await jj1.SerializeObjectAsync(payLoad);
        await _context!.UpsertDocumentAsync(content);
    }
}