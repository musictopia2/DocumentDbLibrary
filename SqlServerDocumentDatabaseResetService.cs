namespace DocumentDbLibrary;
public class SqlServerDocumentDatabaseResetService
{
    //this is the name of the database you are deleting.


    BasicConnector _connect;
    private IDbConnector Connector => _connect.GetConnector;

    public SqlServerDocumentDatabaseResetService()
    {
        //only works with sql server this time.
        _connect = new("DocumentDatabase");
    }


    private void SetDatabaseParameters(IDbCommand command, string databaseName)
    {
        DbParameter parameter;
        parameter = Connector.GetParameter();
        parameter.DbType = DbType.String;
        //i don't think there is size this time.
        parameter.ParameterName = "@DatabaseName";
        parameter.Value = databaseName;
        command.Parameters.Add(parameter);

    }
    public async Task ResetDatabaseAsync(string databaseName)
    {
        if (string.IsNullOrWhiteSpace(databaseName))
        {
            throw new CustomBasicException("Database name cannot be blank.");
        }
        await _connect.DoWorkAsync(async capture =>
        {
            //BasicList<DynamicParameter> parameters =
            //[
            //    new("@DatabaseName", databaseName)
            //];
            var cons = capture.CurrentConnection;
            await Task.Run(() =>
            {

                using IDbCommand command = Connector.GetCommand();
                command.Connection = cons;
                SetDatabaseParameters(command, databaseName);


                command.CommandText = """
                    delete from DocumentTable where DatabaseName = @DatabaseName
                    """;



                command.ExecuteNonQuery();
            });


        });
    }




}
