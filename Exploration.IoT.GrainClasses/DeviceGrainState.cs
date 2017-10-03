namespace Exploration.IoT.GrainClasses
{
    using Orleans;

    public class DeviceGrainState //: IGrainState
    {
        public double LastValue { get; set; }
        //double LastValue { get; set; }
    }
}