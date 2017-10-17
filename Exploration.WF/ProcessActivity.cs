namespace Exploration.WF
{
    using System.Activities;
    using System.Diagnostics;

    public sealed class ProcessActivity : CodeActivity
    {
        public InArgument<string> ArgumentPath { get; set; }

        // Define an activity input argument of type string
        public InArgument<string> ExecutablePath { get; set; }

        public OutArgument<string> MyStandardOutput { get; set; }

        // protected override void CacheMetadata(NativeActivityMetadata metadata)
        // {
        // metadata.AddArgument(this.ExecutablePath);
        // }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // var pass = new SecureString();
            var processStartInfo = new ProcessStartInfo()
                                       {
                                           CreateNoWindow = true,
                                           RedirectStandardOutput = true,
                                           RedirectStandardInput = true,
                                           UseShellExecute = false,

                                           // UserName = "quillass",
                                           // Domain = "UFG_DOM",
                                           // Password = pass,
                                           FileName = this.ExecutablePath.Get(context),
                                           Arguments = this.ArgumentPath.Get(context)
                                       };

            var process = new Process() { StartInfo = processStartInfo };

            process.Start();

            // process.BeginOutputReadLine();
            var readToEnd = process.StandardOutput.ReadToEnd();

            this.MyStandardOutput.Set(context, readToEnd);

            // Obtain the runtime value of the FilePath input argument
            // string text = context.GetValue(this.FilePath);
        }
    }
}