namespace DocumentDbLibrary;
public abstract class ObjectDataAccess<T>
{
    private DocumentContext? _context;
    private string? _text;
    public ObjectDataAccess(string databaseName, string collectionName)
    {
        Init(databaseName, collectionName, "");
    }
    public ObjectDataAccess(string databaseName, string collectionName, string path)
    {
        Init(databaseName, collectionName, path);
    }
    private void Init(string databaseName, string collectionName, string path)
    {
        _context = new(databaseName, collectionName, path);
    }
    protected async Task<bool> ObjectExists()
    {
        string data = await _context!.GetDocumentAsync();
        if (string.IsNullOrWhiteSpace(data))
        {
            return false;
        }
        _text = data;
        return true;
    }
    protected async Task<T> GetDocumentAsync() //for now, just make public.  its only for testing until i figure out how i should make this work.
    {
        if (_text is null)
        {
            throw new CustomBasicException("No text was found.  Try to call ObjectExists first and do an action when the object does not exist");
        }
        T output = await jj1.DeserializeObjectAsync<T>(_text);
        return output;
    }
    protected async Task UpsertRecordAsync(T payLoad)
    {
        string content = await jj1.SerializeObjectAsync(payLoad);
        await _context!.UpsertDocumentAsync(content);
    }
}