namespace Exploration.Cmdlets
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Reflection;

    //[Cmdlet("Get", "RandomName")]
    [Cmdlet(VerbsCommon.Get, "RandomName")]
    public class GetRandomName : Cmdlet
    {
        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        private string[] names; //= { "Mario", "Toto", "Nathan" };

        protected override void ProcessRecord()
        {
            this.WriteObject(
                this.names
                    .Where(n => n.Length == this.Name.Length)
                    .OrderBy(n => Guid.NewGuid())
                    .FirstOrDefault());//.Select(x=>string.Concat(x, new Random(10025).Next()))

            //this.WriteVerbose(this.Name);

            //var nameCharacters = this.Name.ToCharArray();
            //Array.Reverse(nameCharacters);

            //this.WriteObject(
            //    new
            //        {
            //            ReversedName = new string(nameCharacters), NameLength = this.Name.Length
            //        });
            //Console.WriteLine($"{this.Name} has a length of :{this.Name.Length}");
        }

        protected override void BeginProcessing()
        {
            this.WriteVerbose("Loading names files ...");

            this.names =
                File.ReadAllLines(
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "someNames.txt"));
        }

        protected override void EndProcessing()
        {
        }

        protected override void StopProcessing()
        {
        }
    }
}