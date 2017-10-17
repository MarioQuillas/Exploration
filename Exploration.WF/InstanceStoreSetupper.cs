namespace Exploration.WF
{
    using System;
    using System.Activities.DurableInstancing;
    using System.Configuration;

    public class InstanceStoreSetupper
    {
        public InstanceStoreSetupper()
        {
            this.InstanceStore = this.SetupInstanceStore();
        }

        public SqlWorkflowInstanceStore InstanceStore { get; }

        private SqlWorkflowInstanceStore SetupInstanceStore()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["InstanceStore"].ConnectionString;

            var result = new SqlWorkflowInstanceStore(connectionString);

            result.InstanceCompletionAction = InstanceCompletionAction.DeleteAll;
            result.InstanceEncodingOption = InstanceEncodingOption.None;
            result.InstanceLockedExceptionAction = InstanceLockedExceptionAction.AggressiveRetry;

            var handle = result.CreateInstanceHandle();
            var view = result.Execute(handle, new CreateWorkflowOwnerCommand(), TimeSpan.FromSeconds(30));

            result.DefaultInstanceOwner = view.InstanceOwner;
            handle.Free();

            return result;
        }
    }
}