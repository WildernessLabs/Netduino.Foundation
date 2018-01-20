using System.Threading;
using Netduino.Foundation.Displays;

namespace SerialLCDTest
{
    public class Program
    {
        public static void Main()
        {
            var display = new SerialLCD();
            //
            //  Clear the display ready for the test.
            //
            display.Clear();
            display.SetCursorStyle(SerialLCD.CursorStyle.BlinkingBoxOff);
            display.SetCursorStyle(SerialLCD.CursorStyle.UnderlineOff);
            //
            //  Display some text on the bottom row of a 16x2 LCD.
            //
            display.SetCursorPosition(1, 1);
            display.Write("Hello, world");
            Thread.Sleep(1000);
        }
    }
}