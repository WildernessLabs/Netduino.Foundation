# Netduino.Foundation

Netduino.Foundation greatly simplifies the task of building complex .NET Microframework (MF) powered connected things with Netduino, and include:


 * **Low-level Hardware Abstraction** - This is a modular/compositable based on the concept of _Blocks_ and _Scopes_ that represent devices and listeners, accordingly.
 * **Sensor and Peripheral Library** - Strongly typed libraries that do the heavy lifting of integration with hundreds of popular sensors spanning the gamut from Alcohol Sensors to 3-axis Accelerometers.
 
Netduino.Foundation  uses reactive-like design patterns, in that it consists of composable **Blocks** that can be connected together to automatically bind output from one item into another. It also includes the notion of **Scopes**, which take the output from a block and do interesting things with it, such as transform it.

For example, the following program blinks the Netduino's onboard LED when the onboard button is pressed:
 
```csharp
public class Program
{
	// create our pin references.
	// note, when you create a reference to the onboard button, netduino is 
	// smart enough to not use it as a reset button. :)
	static H.Cpu.Pin buttonHardware = Pins.ONBOARD_BTN;
	static H.Cpu.Pin ledHardware = Pins.ONBOARD_LED;

	public static void Main()
	{
		// Create the blocks
		var button = new DigitalInputPin (buttonHardware);
		var led = new DigitalOutputPin (ledHardware);

		// Connect them together. with the block/scope architecture, you can think
		// of everything as being connectable - output from one thing can be piped
		// into another. in this case, we're setting the button output to the LED
		// input. so when the user presses on the button, the signal goes straight
		// to the LED.
		button.Output.ConnectTo (led.Input);

		// keep the program alive
		while (true) {
			System.Threading.Thread.Sleep (1000);
		}
	}
}
``` 

# Documentation and Guides

## [Getting Started](Documentation/Getting_Started/)

Start [here](Documentation/Getting_Started/) to get up and running with Monkey.Robotics quickly.

## [API Documentation](Documentation/API_Docs/)

Check out the [API Documentation](Documentation/API_Docs/) for reference and API browsing.

 
## Author Credits

Authors: Bryan Costanich, Frank Krueger, Craig Dunn

