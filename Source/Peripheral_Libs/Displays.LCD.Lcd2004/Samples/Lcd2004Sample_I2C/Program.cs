using System;
using Microsoft.SPOT;
using Netduino.Foundation.Displays;
using System.Threading;
using Netduino.Foundation.Displays.LCD;
using Netduino.Foundation.ICs.IOExpanders.MCP23008;
using System.Text;

namespace Lcd2004Sample_I2C
{
    public class Program
    {
        public static void Main()
        {
            MCP23008 mcp = new MCP23008();

            ITextDisplay lcd = new Lcd2004(mcp);
            lcd.WriteLine("Wilderness Rabs", 0);
            lcd.WriteLine("Powering", 1);
            lcd.WriteLine("Connected", 2);
            lcd.WriteLine("Things", 3);

            //Thread.Sleep(3000);

            //lcd.Clear();

            //byte[] happyFace = { 0x0, 0x0, 0xa, 0x0, 0x11, 0xe, 0x0, 0x0 };
            //byte[] sadFace = { 0x0, 0x0, 0xa, 0x0, 0xe, 0x11, 0x0, 0x0 };
            //byte[] rocket = { 0x4, 0xa, 0xa, 0xa, 0x11, 0x15, 0xa, 0x0 };
            //byte[] heart = { 0x0, 0xa, 0x1f, 0x1f, 0xe, 0x4, 0x0, 0x0 };

            //// save the custom characters
            //lcd.SaveCustomCharacter(happyFace, 1);
            //lcd.SaveCustomCharacter(sadFace, 2);
            //lcd.SaveCustomCharacter(rocket, 3);
            //lcd.SaveCustomCharacter(heart, 4);

            //lcd.Clear();

            //// create our string, using the addresses of the characters
            //// casted to char.
            //StringBuilder s = new StringBuilder();
            //s.Append("1:" + (char)1 + " ");
            //s.Append("2:" + (char)2 + " ");
            //s.Append("3:" + (char)3 + " ");
            //s.Append("4:" + (char)4 + " ");
            //lcd.WriteLine(s.ToString(), 0);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
