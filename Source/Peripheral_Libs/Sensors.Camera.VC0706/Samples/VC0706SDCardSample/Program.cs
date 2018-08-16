using Microsoft.SPOT;
using Microsoft.SPOT.IO;
using System.IO;

namespace Camera_VC0706
{
    public class Program
    {
        public static CameraVC0706 Camera = new CameraVC0706();

        public static void Main()
        {
            Debug.Print("hello VC0706");

            var volume = new VolumeInfo("SD");

            RecurseFolders(new DirectoryInfo("SD"));

            Camera.Initialize("COM1");

            Camera.SetTVOutput(false);

            ShowCameraConfigTest();

            Camera.Initialize("COM1", CameraVC0706.ComPortSpeed.Baud115200, CameraVC0706.ImageSize.Res160x120);
            TakePictures(@"SD\StressTestSmall", 100);

            Camera.SetTVOutput(true);
        }

        static void RecurseFolders(DirectoryInfo directory)
        {
            if (directory == null)
                return;

            foreach (var file in directory.GetFiles())
            {
                Debug.Print(file.FullName);
            }

            foreach(var subDir in directory.GetDirectories())
            {
                RecurseFolders(subDir);
            }
        }

        public static void ShowCameraConfigTest()
        {
            Debug.Print("Camera version: " + Camera.GetVersion());
            Debug.Print("Compression: " + Camera.GetCompression());
            Debug.Print("Image size: " + Camera.GetImageSize());
            Debug.Print("Motion detection activation: " + Camera.GetMotionDetectionCommStatus());
        }

        public static void TakePictures(string path, int maxCount = 100)
        {
            Directory.CreateDirectory(path);
            for (var count = 0; count < maxCount; count++)
            {
                var imgPath = Path.Combine(path, "Pic_" + count + ".jpg");
                Debug.Print("Working on " + imgPath);

                Camera.TakePicture(imgPath);
            }
        }
    }
}

namespace System.Diagnostics
{
    public enum DebuggerBrowsableState
    {
        Never = 0,
        Collapsed = 2,
        RootHidden = 3
    }
}