using System.Threading;
using Netduino.Foundation.Displays.LCD;
using N = SecretLabs.NETMF.Hardware.Netduino;
using System.Text;
using Netduino.Foundation.Displays;

namespace SunStar2004aSample
{
    public class Program
    {
        public static void Main()
        {
            ITextDisplay lcd = new Lcd2004
            (
                V0: N.PWMChannels.PWM_PIN_D5,
                RS: N.Pins.GPIO_PIN_D8,
                E: N.Pins.GPIO_PIN_D9,
                D4: N.Pins.GPIO_PIN_D10,
                D5: N.Pins.GPIO_PIN_D11,
                D6: N.Pins.GPIO_PIN_D12,
                D7: N.Pins.GPIO_PIN_D13
            );
            lcd.WriteLine("Wilderness Rabs", 0);
            lcd.WriteLine("Powering", 1);
            lcd.WriteLine("Connected", 2);
            lcd.WriteLine("Things", 3);

            Thread.Sleep(3000);

            lcd.Clear();

            byte[] happyFace = { 0x0, 0x0, 0xa, 0x0, 0x11, 0xe, 0x0, 0x0 };
            byte[] sadFace = { 0x0, 0x0, 0xa, 0x0, 0xe, 0x11, 0x0, 0x0 };
            byte[] rocket = { 0x4, 0xa, 0xa, 0xa, 0x11, 0x15, 0xa, 0x0 };
            byte[] heart = { 0x0, 0xa, 0x1f, 0x1f, 0xe, 0x4, 0x0, 0x0 };

            // save the custom characters
            lcd.SaveCustomCharacter(happyFace, 1);
            lcd.SaveCustomCharacter(sadFace, 2);
            lcd.SaveCustomCharacter(rocket, 3);
            lcd.SaveCustomCharacter(heart, 4);

            lcd.Clear();

            // create our string, using the addresses of the characters
            // casted to char.
            StringBuilder s = new StringBuilder();
            s.Append("1:" + (char)1 + " ");
            s.Append("2:" + (char)2 + " ");
            s.Append("3:" + (char)3 + " ");
            s.Append("4:" + (char)4 + " ");
            lcd.WriteLine(s.ToString(), 0);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}