namespace DocumentDbLibrary;
internal class DocumentContext(string databaseName, string collectionName, string proposedPath)
{
    public readonly DocumentConnector Helps = new(databaseName, collectionName, proposedPath);
    public IDbConnector Connector => Helps.GetConnector;
    public async Task<string> GetDocumentAsync()
    {
        DocumentTable? document = null;
        await Helps.DoWorkAsync(async cons =>
        {
            BasicList<ICondition> conditions = ss1.StartWithOneCondition(nameof(DocumentTable.DatabaseName), databaseName)
            .AppendCondition(nameof(DocumentTable.CollectionName), collectionName);
            var list = await cons.GetAsync<DocumentTable>(conditions, Helps.GetConnector);
            if (list.Count > 1)
            {
                throw new CustomBasicException($"There cannot be more than one document with database {databaseName} and collection {collectionName}");
            }
            if (list.Count == 1)
            {
                document = list.Single();
            }
        });
        if (document is null)
        {
            return "";
        }
        return document.JsonData;
    }
    public async Task UpsertDocumentAsync(string content)
    {
        await Helps.DoWorkAsync(async cons =>
        {
            BasicList<ICondition> conditions = ss1.StartWithOneCondition(nameof(DocumentTable.DatabaseName), databaseName)
            .AppendCondition(nameof(DocumentTable.CollectionName), collectionName);
            var list = await cons.GetAsync<DocumentTable>(conditions, Helps.GetConnector);
            if (list.Count > 1)
            {
                throw new CustomBasicException($"There was already more than one document with database {databaseName} and collection {collectionName}");
            }
            DocumentTable document;
            if (list.Count == 1)
            {
                document = list.Single();
                document.JsonData = content;
                await cons.UpdateEntityAsync(document, EnumUpdateCategory.All);
            }
            else
            {
                document = new();
                document.DatabaseName = databaseName;
                document.CollectionName = collectionName;
                document.JsonData = content;
                await cons.InsertSingleAsync(document, Helps.GetConnector);
            }
        });
    }
}