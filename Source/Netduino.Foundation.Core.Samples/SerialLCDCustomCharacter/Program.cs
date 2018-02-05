using System.Threading;
using System.Text;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Displays;


namespace Netduino.Foundation.Samples
{
    /// <summary>
    /// Custom Character Generator: http://maxpromer.github.io/LCD-Character-Creator/
    /// Custom Characters: https://www.quinapalus.com/hd44780udg.html
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            SerialLCD _display = new SerialLCD();

            byte[] happyFace = { 0x0, 0x0, 0xa, 0x0, 0x11, 0xe, 0x0, 0x0 };
            byte[] sadFace = { 0x0, 0x0, 0xa, 0x0, 0xe, 0x11, 0x0, 0x0 };

            // save the custom characters
            _display.SaveCustomCharacter(happyFace, 0);
            _display.SaveCustomCharacter(sadFace, 1);

            _display.Clear();

            // why does this not work?
            //StringBuilder s = new StringBuilder("Chars:");
            //s.Append(System.Convert.ToChar(0));
            //s.Append(System.Convert.ToChar(1));
            //_display.WriteLine(s.ToString(), 0);

            // yet this does?
            _display.TestCustomChar(0);

            while (true)
            {
                Thread.Sleep(Timeout.Infinite);
            }

        }
    }
}
