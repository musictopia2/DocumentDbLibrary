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
    //was doing to be public but decided to make it protected like all other methods for this class.
    protected async Task<bool> CanExportAsync()
    {
        string data = await _context!.GetDocumentAsync();
        return string.IsNullOrWhiteSpace(data) == false;
    }
    protected async Task ExportDocumentAsync(string path)
    {
        string data = await _context!.GetDocumentAsync();
        if (string.IsNullOrWhiteSpace(data))
        {
            throw new CustomBasicException("No data was found.  Cannot export");
        }
        await ff1.WriteAllTextAsync(path, data);
    }

    //protected async Task<bool> ExistsAsync(Func<T, bool> match)
    //{
    //    var list = await GetDocumentsAsync();
    //    return list.Any(match);
    //}
    protected static void UpsertItem(BasicList<T> list, T item, Func<T, bool> match)
    {
        var existing = list.SingleOrDefault(x => match(x));
        if (existing is null)
        {
            list.Add(item);
            return;
        }

        int index = list.IndexOf(existing);
        list[index] = item;
    }
    protected async Task UpsertRecordsAsync(BasicList<T> payLoad)
    {
        string content = await jj1.SerializeObjectAsync(payLoad);
        await _context!.UpsertDocumentAsync(content);
    }
}