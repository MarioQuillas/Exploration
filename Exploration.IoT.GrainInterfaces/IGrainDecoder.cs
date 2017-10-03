namespace Exploration.IoT.GrainInterfaces
{
    using System.Threading.Tasks;

    using Orleans;

    public interface IGrainDecoder : IGrainWithIntegerKey
    {
        Task Decode(string message);
    }
}