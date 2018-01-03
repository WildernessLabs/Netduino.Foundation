using System.Threading;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.Displays.MicroLiquidCrystal;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace MicroLiquidCrystal595Test
{
    public class Program
    {
        public static void Main()
        {
            var setup = new BaseShifterLcdTransferProvider.ShifterSetup
            {
                BL = ShifterPin.GP7,
                RS = ShifterPin.GP1,
                RW = ShifterPin.None,
                Enable = ShifterPin.GP2,
                D4 = ShifterPin.GP6,
                D5 = ShifterPin.GP5,
                D6 = ShifterPin.GP4,
                D7 = ShifterPin.GP3
            };
            var lcdBus = new Shifter74Hc595LcdTransferProvider(SPI.SPI_module.SPI1, Pins.GPIO_PIN_D3,
                                                               Shifter74Hc595LcdTransferProvider.BitOrder.MSBFirst,
                                                               setup);
            var lcd = new Lcd(lcdBus);
            lcd.Begin(16, 2);
            lcd.Write("Hello, world!");
            while (true)
            {
                lcd.SetCursorPosition(0, 1);
                lcd.Write((Utility.GetMachineTime().Ticks / 10000).ToString());
                Microsoft.SPOT.Debug.Print("here");
                Thread.Sleep(100);
            }
        }
    }
}