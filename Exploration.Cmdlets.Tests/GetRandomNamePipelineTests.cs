namespace Exploration.Cmdlets.Tests
{
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    using NUnit.Framework;

    [TestFixture]
    public class GetRandomNamePipelineTests
    {
        [Test]
        public void TestThreeNames()
        {
            var initialSessionState = InitialSessionState.CreateDefault();

            initialSessionState.Commands.Add(
                new SessionStateCmdletEntry(
                    "Get-RandomName", typeof(InvokeSqlQuery), null));

            using (var runspace = RunspaceFactory.CreateRunspace(initialSessionState))
            {
                runspace.Open();

                using (var powershell = PowerShell.Create())
                {
                    powershell.Runspace = runspace;

                    powershell.Commands.AddCommand("Get-RandomName");

                    var results = powershell.Invoke<string>(new[] { "Nathan", "John", "Ted" });

                    Assert.AreEqual(results.Count, 3);
                    Assert.AreEqual(results[0].Length, 6);
                    Assert.AreEqual(results[0].Length, 4);
                    Assert.AreEqual(results[0].Length, 3);
                }
            }
        }
    }
}