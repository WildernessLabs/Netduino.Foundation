using System;
using Microsoft.SPOT;
using Netduino.Foundation;

namespace NetworkSample
{
    public class Program
    {
        public static void Main()
        {
            // initialize the network
            bool isGood = Netduino.Foundation.Network.Initializer.InitializeNetwork("http://www.google.com");


        }
    }
}
