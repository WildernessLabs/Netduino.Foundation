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
            byte[] sadFace   = { 0x0, 0x0, 0xa, 0x0, 0xe, 0x11, 0x0, 0x0 };
            byte[] rocket = { 0x4, 0xa, 0xa, 0xa, 0x11, 0x15, 0xa, 0x0 };
            byte[] heart     = { 0x0, 0xa, 0x1f, 0x1f, 0xe, 0x4, 0x0, 0x0 };

            // save the custom characters
            _display.SaveCustomCharacter(happyFace, 1);
            _display.SaveCustomCharacter(sadFace, 2);
            _display.SaveCustomCharacter(rocket, 3);
            _display.SaveCustomCharacter(heart, 4);

            _display.Clear();
            _display.SetBrightness();

            // 
            StringBuilder s = new StringBuilder("Chars:");
            s.Append((char)1);
            s.Append((char)2);
            s.Append((char)3);
            s.Append((char)4);
            _display.WriteLine(s.ToString(), 0);

            // for 0
            //_display.TestCustomChar(0);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
