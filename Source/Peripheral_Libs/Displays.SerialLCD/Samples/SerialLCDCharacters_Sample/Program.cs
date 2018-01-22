using System.Threading;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Displays;
using F = Netduino.Foundation;

namespace SerialLCDCharacters_Sample
{
    public class Program
    {
        public static void Main()
        {
            // initialize our network
            App app = new App();
            app.Run();

            while (app.IsRunning)
            {
                Thread.Sleep(10000);
            }
        }

        public class App
        {
            F.Displays.SerialLCD _lcd = null;

            public bool IsRunning { get; set; }

            public App()
            {
                TextDisplayConfig displayConfig = new TextDisplayConfig() { Width = 16, Height = 2 };
                _lcd = new F.Displays.SerialLCD(displayConfig);
            }

            public void Run()
            {
                this.IsRunning = true;

                _lcd.Clear();

                this.StartTestCharDisplay();

            }

            public void DisplayTemperature(float value)
            {
                char degree = System.Convert.ToChar(223);
                string temp = value.ToString("N2");
                string text = ("Temp: " + temp + degree + "C");
                _lcd.WriteLine(text, 0);
            }

            public void DisplayHumidity(float value)
            { }

            protected void StartTestCharDisplay()
            {
                System.Threading.Thread t = new Thread(() =>
                {
                    ushort start = 0;
                    ushort end = 255;
                    ushort current = start;
                    ushort count = _lcd.DisplayConfig.Width;
                    ushort calculatedEnd = 0;

                    while (true)
                    {
                        Debug.Print("Loop");
                        calculatedEnd = (ushort)(current + count);

                        // if we're passed the end, start at the beginning
                        if (current >= end)
                        {
                            current = start;
                            calculatedEnd = calculatedEnd = (ushort)(current + count);
                        }
                        // if we're near the end, only display up to 255
                        else if (current + count > end)
                        {
                            ushort surplus = (ushort)(current + count - end);
                            calculatedEnd = (ushort)(end - surplus);
                        }

                        _lcd.Clear();
                        _lcd.WriteLine("chars " + current.ToString() + "-" + calculatedEnd.ToString(), 0);
                        DisplayChars(current, calculatedEnd);

                        current += count;

                        Thread.Sleep(500);
                    }
                });
                t.Start();

            }

            protected void DisplayChars(ushort start, ushort count)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (ushort i = start; i < count; i++)
                {
                    sb.Append(System.Convert.ToChar(i));
                }

                _lcd.WriteLine(sb.ToString(), 1);
            }
        }
    }
}
