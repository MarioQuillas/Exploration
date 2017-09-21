namespace Exploration.Cmdlets
{
    using System;
    using System.Data.SqlClient;
    using System.Management.Automation;

    [Cmdlet(VerbsLifecycle.Invoke, "SqlQuery", DefaultParameterSetName = IntegratedAuth)]
    public class InvokeSqlQuery : PSCmdlet
    {
        private const string IntegratedAuth = "IntegratedAuth";

        private const string SqlAuth = "SqlAuth";

        [Parameter(Position = 1, ParameterSetName = IntegratedAuth)]
        [Parameter(Position = 1, ParameterSetName = SqlAuth)]
        public string Server { get; set; }

        [Parameter(Position = 2, ParameterSetName = IntegratedAuth)]
        [Parameter(Position = 2, ParameterSetName = SqlAuth)]
        public string Database { get; set; }

        [Parameter(Position = 3, Mandatory = true, ParameterSetName = IntegratedAuth)]
        [Parameter(Position = 3, Mandatory = true, ParameterSetName = SqlAuth)]
        public string Query { get; set; }

        [Parameter(Position = 4, ParameterSetName = SqlAuth, Mandatory = true)]
        public string Username { get; set; }

        [Parameter(Position = 5, ParameterSetName = SqlAuth, Mandatory = true)]
        public string Password { get; set; }

        private SqlConnection connection;

        protected override void BeginProcessing()
        {
            this.ValidateParameters();

            this.WriteVerbose(this.ParameterSetName);

            string connectionString;

            if (this.ParameterSetName == IntegratedAuth)
            {
                connectionString =
                    $"Data Source={this.Server};Initial Catalog={this.Database};Integrated Security=SSPI;Persist Security Info=true";
            }
            else
            {
                connectionString =
                    $"Data Source={this.Server};Initial Catalog={this.Database};User ID={this.Username};Password={this.Password}";
            }
            this.connection = new SqlConnection(connectionString);
            this.connection.Open();
        }

        protected override void EndProcessing()
        {
            this.connection?.Dispose();
        }

        protected override void StopProcessing()
        {
            this.connection?.Dispose();
        }

        protected override void ProcessRecord()
        {
            using (var command = this.connection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = this.Query;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var record = new PSObject();

                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            record.Properties.Add(
                                new PSVariableProperty(
                                    new PSVariable(reader.GetName(i), reader[i])));
                        }

                        this.WriteObject(record);
                    }
                }
            }
        }

        private void ValidateParameters()
        {
            const string ServerVariable = "InvokeSqlQueryServer";
            const string DatabaseVariable = "InvokeSqlQueryDatabase";

            if (!string.IsNullOrEmpty(this.Server))
            {
                this.SessionState.PSVariable.Set(ServerVariable, this.Server);
            }
            else
            {
                this.Server = 
                    this.SessionState.PSVariable.GetValue(
                        ServerVariable, defaultValue: string.Empty)
                        .ToString();
                if (string.IsNullOrEmpty(this.Server)) this.ThrowParameterError("Server");               
            }

            if (!string.IsNullOrEmpty(this.Database))
            {
                this.SessionState.PSVariable.Set(DatabaseVariable, this.Database);
            }
            else
            {
                this.Database =
                    this.SessionState.PSVariable.GetValue(
                            DatabaseVariable, defaultValue: string.Empty)
                        .ToString();
                if (string.IsNullOrEmpty(this.Database)) this.ThrowParameterError("Database");
            }
        }

        private void ThrowParameterError(string parameterName)
        {
            this.ThrowTerminatingError(
                new ErrorRecord(
                    new ArgumentException(
                        $"Must specify {parameterName}"),
                    Guid.NewGuid().ToString(),
                    ErrorCategory.InvalidArgument,
                    null));
        }
    }
}