namespace Exploration.IoT.GrainInterfaces
{
    using Orleans;

    public interface ISystemObserver : IGrainObserver
    {
        void HighTemperature(double value);
    }
}