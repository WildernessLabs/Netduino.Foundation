namespace Netduino.Foundation.Sensors.Motion
{
    public interface IGyroscope
    {
        float XRotation { get; }
        float YRotation { get; }
        float ZRotation { get; }
    }
}