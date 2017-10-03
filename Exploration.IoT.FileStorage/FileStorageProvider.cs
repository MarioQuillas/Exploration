namespace Exploration.IoT.FileStorage
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Orleans;
    using Orleans.Providers;
    using Orleans.Runtime;
    using Orleans.Storage;

    public class FileStorageProvider : IStorageProvider
    {
        public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            this.Name = name;
            this.directory = config.Properties["directory"];
            return Task.CompletedTask;
        }

        public Task Close()
        {
            return Task.CompletedTask;
        }

        private string directory;
        public string Name { get; set; }

        public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var fileInfo = this.GetFileInfo(grainType, grainReference);

            if (!fileInfo.Exists) return;

            using (var stream = fileInfo.OpenText())
            {
                var json = await stream.ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                //grainState.SetAll(data);
            }
        }

        private FileInfo GetFileInfo(string grainType, GrainReference grainReference)
        {
            return new FileInfo(
                Path.Combine(this.directory, $"{grainType}-{grainReference.ToKeyString()}"));
        }

        public Task WriteStateAsync(
            string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var json = JsonConvert.SerializeObject(grainState);

            var fileInfo = this.GetFileInfo(grainType, grainReference);

            using (var stream = fileInfo.OpenWrite())
            using (var writer = new StreamWriter(stream))
            {
                return writer.WriteAsync(json);
            }
        }

        public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var fileInfo = this.GetFileInfo(grainType, grainReference);
            fileInfo.Delete();
            return Task.CompletedTask;
        }

        public Logger Log { get; set; }
    }
}
