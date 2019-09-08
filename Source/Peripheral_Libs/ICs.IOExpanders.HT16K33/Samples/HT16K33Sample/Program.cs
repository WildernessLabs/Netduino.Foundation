using System.Threading;
using ICs.IOExpanders;

namespace HT16K33Sample
{
    public class Program
    {
        static HT16K33 display = new HT16K33();

        public static void Main()
        {
            int index = 0;
            bool on = true;
            // write your code here
            while (true)
            {
                display.ToggleLed((byte)index, on);
                display.UpdateDisplay();
                index++;

                if(index >= 128)
                {
                    index = 0;
                    on = !on;
                }

                Thread.Sleep(100);
            }


        }

    }
}
