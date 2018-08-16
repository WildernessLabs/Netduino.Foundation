using System;
using Microsoft.SPOT;

namespace Camera_VC0706
{
    public interface ICamera
    {
        void TakePicture(string path);
    }
}
