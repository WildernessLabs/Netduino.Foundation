# Netduino.Foundation

Netduino.Foundation greatly simplifies the task of building complex .NET Microframework (MF) powered connected things with Netduino, and includes:


 * **Low-level Hardware Abstraction** - This is a modular/composable based on the concept of _Blocks_ and _Scopes_ that represent devices and listeners, accordingly.
 * **Sensor and Peripheral Library** - Strongly typed libraries that do the heavy lifting of integration with hundreds of popular sensors spanning the gamut from Alcohol Sensors to 3-axis Accelerometers.

 
[also a plugable library to organize and house peripheral code/suppoort]
 
 
Netduino.Foundation  uses reactive-like design patterns, in that it consists of composable **Blocks** that can be connected together to automatically bind output from one item into another. It also includes the notion of **Scopes**, which take the output from a block and do interesting things with it, such as transform it.

For example, the following program blinks the Netduino's onboard LED when the onboard button is pressed:
 
```CSharp
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

## Understanding the Architecture and Programing Model

Netduino.Foundation loosely follows a [Reactive Programming](http://en.wikipedia.org/wiki/Reactive_programming) pattern. This means that instead of an event-driven model (which works well in tradition UI apps), components within any paritcular hardware configuration are actually bound to each other and represented by *Blocks*; such that an output signal from one block is automatically passed to another block.

This reactive pattern works incredibly well for circuits because signals tend to be a constant, rather than periodic events such as a person interacting with a UI. Building with composible blocks allow Micro apps to more closely mimic the composibility of the hardware configurations themselves.

### Types of Blocks

In Netduino.Foundation, nearly everything is a block. But they generally fall into various categories (which roughly translate to namespaces):

 * **Sensors** - Sensors are peripherals that typically have outputs and are sometimes configurable. Sensors include buttons, light sensors, infrared distancing, accelerometers, compasses, temperature, etc. We have bound many commonly available sensors sold in Maker stores.
 * **Motors** - These include not only the electric motors themselves, but also the drivers such as driver boards, and H-Bridges.
 * **Generators** - These are specialized blocks that generate signals. For example, there is a sine-wave generator, Pulse-width modulation (PWM) generator, constant generator, etc.
 * **Scopes** - Scopes are blocks that are meant to listen in on signals and do interesting things with the signal. For example, we have a DebugScope which can be configured to sample a signal at specific time intervals and write the signal value out to the debug console.
 * **Specialized Blocks** - Specialized Blocks are blocks that don't fit into the previous categories. Some of them are used for signal transformation; for instance, converting a signal that carries Celsius temperature data into a signal that carries Fahrenheit temperature data.


### Connecting Blocks

In practice, the way this works is that nearly every thing derives from the Block class. A block can have Input and Output ports which are bound to each other via the `ConnectTo()` method. For instance, the following micro framework app binds the *Output* from the Netduino's onboard button to the *Input* of the onboard LED. This results in the LED lighting up when the button is pressed:


```CSharp
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

		// Connect the outpt of the button to the input of the LED
		button.Output.ConnectTo (led.Input);

		// keep the program alive
		while (true) {
			System.Threading.Thread.Sleep (1000);
		}
	}
}

```

### Advanced Block Usage

Because Blocks are composable, they can be strung together to create complex integrations. When you connect two blocks up, a `Connector` class is created internally that does the plumbing for you, so you don't have to. By connecting these blocks, interesting work can be accomplished easily. For example, let's say we have a temperature sensor that outputs the temperate in Celsius, but we actually want to get the temperature in Fahrenheit. Additionally, we want to add debug scope along the way to see that things are working properly, we would wire things up as follows:

![Block Architecture](Support_Files/Images/Block_Flow.png)

By using this architecture, it also helps us to think of our apps using the same conceptual framework as the underlying circuits themselves.

# Creating Peripheral Libraries

[inherit from block or scope]

[put them in namespace of Netduino.Foundation.x]

 * **Buttons**
 * 

[publish them as nugets]


# Documentation and Guides


## [API Documentation](Documentation/API_Docs/)

Check out the [API Documentation](Documentation/API_Docs/) for reference and API browsing.

## License
Copyright 2017, Wilderness Labs Inc.
    
    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at
    
      http://www.apache.org/licenses/LICENSE-2.0
    
    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
 
## Author Credits

Authors: Bryan Costanich, Frank Krueger, Craig Dunn

