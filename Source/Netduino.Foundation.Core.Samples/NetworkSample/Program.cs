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
            Netduino.Foundation.Network.Initializer.InitializeNetwork("http://google.com");


        }
    }
}
