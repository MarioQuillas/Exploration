namespace Exploration.Cmdlets
{
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Get, "SquaredValue")]
    public class GetSquaredValue : PSCmdlet
    {
        [Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true)]
        public int Number { get; set; }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Number * this.Number);
        }
    }
}