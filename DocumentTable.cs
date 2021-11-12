namespace DocumentDbLibrary;
public class DocumentTable : ISimpleDapperEntity
{
    public int ID { get; set; }
    public string DatabaseName { get; set; } = "";
    public string CollectionName { get; set; } = "";
    public string JsonData { get; set; } = "";
}