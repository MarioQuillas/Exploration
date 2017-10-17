namespace Exploration.WF
{
    using System.Activities;

    using RDotNet;

    public sealed class RScriptActivity : NativeActivity
    {
        private const string RScriptDirectoryRFormatted = @"C:/MyGithub/Exploration/Exploration.RScripts/";

        public OutArgument<string> Result { get; set; }

        public InArgument<string> ScriptName { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            REngine.SetEnvironmentVariables(); // <-- May be omitted; the next line would call it.

            using (var engine = REngine.GetInstance())
            {
                var script = $@"source('{RScriptDirectoryRFormatted}{this.ScriptName.Get(context)}')";

                engine.Evaluate(script);
            }
        }
    }
}