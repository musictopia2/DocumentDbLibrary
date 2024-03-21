namespace DocumentDbLibrary;
public abstract class ListDataAccess<T>
{
    private DocumentContext? _context;
    public ListDataAccess(string databaseName, string collectionName)
    {
        Init(databaseName, collectionName, "");
    }
    public ListDataAccess(string databaseName, string collectionName, string path)
    {
        Init(databaseName, collectionName, path);
    }
    private void Init(string databaseName, string collectionName, string path)
    {
        _context = new(databaseName, collectionName, path);
    }
    protected async Task<BasicList<T>> GetDocumentsAsync() //for now, just make public.  its only for testing until i figure out how i should make this work.
    {
        string data = await _context!.GetDocumentAsync();
        if (string.IsNullOrWhiteSpace(data))
        {
            return [];
        }
        BasicList<T> output = await jj1.DeserializeObjectAsync<BasicList<T>>(data);
        return output;
    }
    protected async Task UpsertRecordsAsync(BasicList<T> payLoad)
    {
        string content = await jj1.SerializeObjectAsync(payLoad);
        await _context!.UpsertDocumentAsync(content);
    }
}