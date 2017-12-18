using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.ICs.EEPROM;

namespace AT24C32Test
{
    public class Program
    {
        public static void Main()
        {
            var eeprom = new AT24Cxx(0x57);
            eeprom.Write(0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            var memory = eeprom.Read(0, 16);
            for (ushort index = 0; index < 16; index++)
            {
                Debug.Print("Byte: " + index + ", Value: " + memory[index]);
            }
            eeprom.Write(3, new byte[] { 10 });
            eeprom.Write(7, new byte[] { 1, 2, 3, 4 });
            memory = eeprom.Read(0, 16);
            for (ushort index = 0; index < 16; index++)
            {
                Debug.Print("Byte: " + index + ", Value: " + memory[index]);
            }
            Thread.Sleep(Timeout.Infinite);
        }
    }
}