using Microsoft.SPOT;
using Netduino.Foundation.Displays;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Threading;
using Netduino.Foundation.Sensors.GPS;
using System.IO.Ports;

namespace PCD8544_GPS
{
    public class Program
    {
        static GraphicsLibrary gl;
        public static void Main()
        {
            Debug.Print(Resources.GetString(Resources.StringResources.String1));

            var display = new PCD8544(chipSelectPin: Pins.GPIO_PIN_D9, dcPin: Pins.GPIO_PIN_D8,
                                      resetPin: Pins.GPIO_PIN_D10, spiModule: SPI.SPI_module.SPI1);

            gl = new GraphicsLibrary(display);
            gl.CurrentFont = new Font8x8();

            InitGPS();
            
            Thread.Sleep(-1);
        }

        static void InitGPS()
        {
            var gps = new NMEA("COM1", 9600, Parity.None, 8, StopBits.One);
            //
            var ggaDecoder = new GGADecoder();
            ggaDecoder.OnPositionReceived += ggaDecoder_OnPositionReceived;
            gps.AddDecoder(ggaDecoder);
            //
            var gllDecoder = new GLLDecoder();
            gllDecoder.OnGeographicLatitudeLongitudeReceived += gllDecoder_OnGeographicLatitudeLongitudeReceived;
            gps.AddDecoder(gllDecoder);
            //
            var gsaDecoder = new GSADecoder();
            gsaDecoder.OnActiveSatellitesReceived += gsaDecoder_OnActiveSatelitesReceived;
            gps.AddDecoder(gsaDecoder);
            //
            var rmcDecoder = new RMCDecoder();
            rmcDecoder.OnPositionCourseAndTimeReceived += rmcDecoder_OnPositionCourseAndTimeReceived;
            gps.AddDecoder(rmcDecoder);
            //
            var vtgDecoder = new VTGDecoder();
            vtgDecoder.OnCourseAndVelocityReceived += vtgDecoder_OnCourseAndVelocityReceived;
            gps.AddDecoder(vtgDecoder);
            //
            gps.Open();
        }

        private static void vtgDecoder_OnCourseAndVelocityReceived(object sender, CourseOverGround courseAndVelocity)
        {
            gl.DrawText(0, 0, courseAndVelocity.KPH.ToString("f3") + "km/h");
            gl.Show();

            Debug.Print("Satellite information received.");
            Debug.Print("True heading: " + courseAndVelocity.TrueHeading.ToString("f2"));
            Debug.Print("Magnetic heading: " + courseAndVelocity.MagneticHeading.ToString("f2"));
            Debug.Print("Knots: " + courseAndVelocity.Knots.ToString("f2"));
            Debug.Print("KPH: " + courseAndVelocity.KPH.ToString("f2"));
            Debug.Print("*********************************************\n");
        }

        private static void rmcDecoder_OnPositionCourseAndTimeReceived(object sender,
            PositionCourseAndTime positionCourseAndTime)
        {
            gl.DrawText(0, 10, DecodeDMPostion(positionCourseAndTime.Latitude));
            gl.DrawText(0, 20, DecodeDMPostion(positionCourseAndTime.Longitude));
            gl.DrawText(0, 30, positionCourseAndTime.TimeOfReading.TimeOfDay.ToString());
            gl.Show();

            Debug.Print("Satellite information received.");
            Debug.Print("Time of reading: " + positionCourseAndTime.TimeOfReading);
            Debug.Print("Latitude: " + DecodeDMPostion(positionCourseAndTime.Latitude));
            Debug.Print("Longitude: " + DecodeDMPostion(positionCourseAndTime.Longitude));
            Debug.Print("Speed: " + positionCourseAndTime.Speed.ToString("f2"));
            Debug.Print("Course: " + positionCourseAndTime.Course.ToString("f2"));
            Debug.Print("*********************************************\n");
        }

        private static void gsaDecoder_OnActiveSatelitesReceived(object sender, ActiveSatellites activeSatellites)
        {
            gl.DrawText(0, 40, "#Sats:" + activeSatellites.SatellitesUsedForFix.Length);
            gl.Show();

            Debug.Print("Satellite information received.");
            Debug.Print("Number of satellites involved in fix: " + activeSatellites.SatellitesUsedForFix.Length);
            Debug.Print("Dilution of precision: " + activeSatellites.DilutionOfPrecision.ToString("f2"));
            Debug.Print("HDOP: " + activeSatellites.HorizontalDilutionOfPrecision.ToString("f2"));
            Debug.Print("VDOP: " + activeSatellites.VerticalDilutionOfPrecision.ToString("f2"));
            Debug.Print("*********************************************\n");
        }

        private static void gllDecoder_OnGeographicLatitudeLongitudeReceived(object sender, GPSLocation location)
        {
          //  gl.DrawText(0, 40, location.ReadingTime.ToString());
          //  gl.Show();

            Debug.Print("Location information received.");
            Debug.Print("Time of reading: " + location.ReadingTime);
            Debug.Print("Latitude: " + DecodeDMPostion(location.Latitude));
            Debug.Print("Longitude: " + DecodeDMPostion(location.Longitude));
            Debug.Print("*********************************************\n");
        }

        private static void ggaDecoder_OnPositionReceived(object sender, GPSLocation location)
        {
            Debug.Print("Location information received.");
            Debug.Print("Time of reading: " + location.ReadingTime);
            Debug.Print("Latitude: " + DecodeDMPostion(location.Latitude));
            Debug.Print("Longitude: " + DecodeDMPostion(location.Longitude));
            Debug.Print("Altitude: " + location.Altitude.ToString("f2"));
            Debug.Print("Number of satellites: " + location.NumberOfSatellites);
            Debug.Print("Fix quality: " + location.FixQuality);
            Debug.Print("HDOP: " + location.HorizontalDilutionOfPrecision.ToString("f2"));
            Debug.Print("*********************************************\n");
        }

        private static string DecodeDMPostion(DegreeMinutePosition dmp)
        {
            var position = dmp.Degrees.ToString("f0") + "." + dmp.Minutes.ToString("f2");
            switch (dmp.Direction)
            {
                case DirectionIndicator.East:
                    position += "E";
                    break;
                case DirectionIndicator.West:
                    position += "W";
                    break;
                case DirectionIndicator.North:
                    position += "N";
                    break;
                case DirectionIndicator.South:
                    position += "S";
                    break;
                case DirectionIndicator.Unknown:
                    position += "?";
                    break;
            }
            return position;
        }
    }
}