namespace DocumentDbLibrary;
//decided to make this public.  this way source generators can be used to hook into this as well.
public class DocumentContext(string databaseName, string collectionName, string proposedPath)
{
    public readonly DocumentConnector Helps = new(databaseName, collectionName, proposedPath);
    public IDbConnector Connector => Helps.GetConnector;
    public async Task<string> GetDocumentAsync()
    {
        BasicList<string> results = [];
        await Helps.DoWorkAsync(async cons =>
        {
            await Task.Run(() =>
            {
                using IDbCommand command = Connector.GetCommand();
                command.Connection = cons;
                command.CommandType = CommandType.Text;
                command.CommandText = """
                select a.Data from DocumentTable a where a.DatabaseName = @DatabaseName and a.CollectionName = @CollectionName
                """;
                SetDatabaseParameters(command);
                using DbDataReader? reader = command.ExecuteReader() as DbDataReader ?? throw new CustomBasicException("No reader found");
                while (reader.Read())
                {
                    results.Add(reader.GetString("Data"));
                }
            });
        });
        if (results.Count > 1)
        {
            throw new CustomBasicException($"There cannot be more than one document with database {databaseName} and collection {collectionName}");
        }
        if (results.Count == 0)
        {
            return ""; //because nothing.
        }
        return results.Single();
    }
    private void SetDatabaseParameters(IDbCommand command)
    {
        DbParameter parameter;
        parameter = Connector.GetParameter();
        parameter.DbType = DbType.String;
        //i don't think there is size this time.
        parameter.ParameterName = "@DatabaseName";
        parameter.Value = databaseName;
        command.Parameters.Add(parameter);
        parameter = Connector.GetParameter();
        parameter.DbType = DbType.String;
        parameter.ParameterName = "@CollectionName";
        parameter.Value = collectionName;
        command.Parameters.Add(parameter);
    }
    private BasicList<int> GetResults(IDbConnection cons)
    {
        using IDbCommand command = Connector.GetCommand();
        BasicList<int> results = [];
        command.Connection = cons;
        command.CommandType = CommandType.Text;
        command.CommandText = """
                select a.ID from DocumentTable a where a.DatabaseName = @DatabaseName and a.CollectionName = @CollectionName
                """;
        SetDatabaseParameters(command);
        using DbDataReader? reader = command.ExecuteReader() as DbDataReader ?? throw new CustomBasicException("No reader found");
        while (reader.Read())
        {
            results.Add(reader.GetInt32("ID"));
        }
        reader.Close();
        return results;
    }
    public async Task UpsertDocumentAsync(string content)
    {
        BasicList<int> results = [];
        await Helps.DoWorkAsync(async cons =>
        {
            await Task.Run(() =>
            {
                results = GetResults(cons);
                if (results.Count > 1)
                {
                    throw new CustomBasicException($"There was already more than one document with database {databaseName} and collection {collectionName}");
                }
                using IDbCommand command = Connector.GetCommand();
                command.Connection = cons;
                SetDatabaseParameters(command);
                DbParameter parameter;
                parameter = Connector.GetParameter();
                parameter.DbType = DbType.String;
                parameter.ParameterName = "Data";
                parameter.Value = content;
                command.Parameters.Add(parameter);
                if (results.Count == 0)
                {
                    //this means to add a new record.
                    command.CommandText = """
                    insert into DocumentTable (DatabaseName, CollectionName, Data) values (@DatabaseName, @CollectionName, @Data)
                    """;
                }
                else
                {
                    command.CommandText = """
                    update DocumentTable set Data = @Data where DatabaseName = @DatabaseName and CollectionName = @CollectionName
                    """;
                }
                command.ExecuteScalar(); //well see.
            });
        });
    }
}