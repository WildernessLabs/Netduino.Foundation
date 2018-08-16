using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Sensors.Camera
{
    public interface ICamera
    {
        void TakePicture(string path);
    }
}