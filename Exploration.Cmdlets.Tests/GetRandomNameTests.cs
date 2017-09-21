namespace Exploration.Cmdlets.Tests
{
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class GetRandomNameTests
    {
        [Test]
        public void TestReplacingNathan()
        {
            var cmdlet = new GetRandomName() { Name = "Nathan" };
            var results = cmdlet.Invoke<string>().ToList();

            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Length, 6);
        }

        [Test]
        public void TestReplacingMario()
        {
            var cmdlet = new GetRandomName() { Name = "Mario" };
            var results = cmdlet.Invoke<string>().ToList();

            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Length, 5);
        }
    }
}
