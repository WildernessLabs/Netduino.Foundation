using System;
using Microsoft.SPOT;
using Netduino.Foundation.Displays;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Threading;

namespace PCD8544Sample
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print(Resources.GetString(Resources.StringResources.String1));

            var display = new PCD8544(chipSelectPin: Pins.GPIO_PIN_D9, dcPin: Pins.GPIO_PIN_D8, 
                                      resetPin: Pins.GPIO_PIN_D10, spiModule: SPI.SPI_module.SPI1);

            var gl = new GraphicsLibrary(display);
            gl.CurrentFont = new Font8x8();
            gl.DrawText(0, 0, "PCD8544");
            gl.CurrentFont = new Font4x8();
            gl.DrawText(0, 10, "Nokia 3110 & 5110");

            gl.DrawRectangle(20, 20, 10, 10);

            gl.DrawCircle(60, 30, 12, true, false);

            gl.Show();

            Thread.Sleep(-1);
        }
    }
}