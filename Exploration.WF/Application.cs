namespace Exploration.WF
{
    using System;
    using System.Activities;

    internal class Application
    {
        private readonly InstanceStoreSetupper instanceStoreSetupper;

        public Application(InstanceStoreSetupper instanceStoreSetupper)
        {
            this.instanceStoreSetupper = instanceStoreSetupper;
        }

        public void Run()
        {
            try
            {
                var workflow1 = new Workflow1();

                var workflowApplication = new WorkflowApplication(workflow1);
                workflowApplication.InstanceStore = this.instanceStoreSetupper.InstanceStore;

                // configure here the workflowapplication
                workflowApplication.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}