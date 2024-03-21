namespace DocumentDbLibrary;
internal class DocumentTable : ISimpleDapperEntity
{
    public int ID { get; set; }
    public string DatabaseName { get; set; } = "";
    public string CollectionName { get; set; } = "";
    public string Data { get; set; } = "";
}