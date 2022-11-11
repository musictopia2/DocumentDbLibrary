namespace DocumentDbLibrary;
public abstract class ListDataAccess<T>
{
    public abstract string DatabaseName { get; }
    public abstract string CollectionName { get; }
    readonly DocumentContext _context;
    public ListDataAccess()
    {
        _context = new();
    }
    protected async Task<BasicList<T>> GetDocumentsAsync()
    {
        string data = await _context.GetDocumentAsync(DatabaseName, CollectionName);
        if (string.IsNullOrWhiteSpace(data))
        {
            return new();
        }
        BasicList<T> output = await jj1.DeserializeObjectAsync<BasicList<T>>(data);
        return output;
    }
    protected async Task UpsertRecordsAsync(BasicList<T> payLoad)
    {
        string content = await jj1.SerializeObjectAsync(payLoad);
        await _context.UpsertDocumentAsync(DatabaseName, CollectionName, content);
    }
}