namespace Exploration.Cmdlets
{
    using System;
    using System.Data.SqlClient;
    using System.Management.Automation;

    [Cmdlet(VerbsLifecycle.Invoke, "SqlQuery")]
    public class InvokeSqlQuery : PSCmdlet
    {
        [Parameter(Position = 1)]
        public string Server { get; set; }

        [Parameter(Position = 2)]
        public string Database { get; set; }

        [Parameter(Position = 3, Mandatory = true)]
        public string Query { get; set; }

        private SqlConnection connection;

        protected override void BeginProcessing()
        {
            this.ValidateParameters();

            var connectionString = "";
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
            if (string.IsNullOrEmpty(this.Server)) this.ThrowParameterError("Server");
            if (string.IsNullOrEmpty(this.Database)) this.ThrowParameterError("Database");
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