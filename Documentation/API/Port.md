---
title: Netduino.Foundation.Port Class
---

Represents a connectable input or output on a block.

## Remarks

Ports allow blocks to be bound together such that the output from one port can be directly piped to the input of another port. This allows a reactive programming style paradigm in which the app code more closely resembles the composability of the underlying circuits.

Ports can come in two different flavors, Input and Output, which indicate the direction of data flow. An InputPort on a Block receives signal data, and an OutputPort sends signal data. 

Additionally, there is one more specialized Port called a ConfigPort. Certain Blocks can be configured to modify their output based on parameters specified by a ConfigPort. The beauty of ConfigPorts is that as opposed to having set-and-forget parameters, a ConfigPort is itself a port, meaning it can modify configuration in real-time, over time. For example, you could use the SineWave Generator Block to generate a Sine Wave signal, and then feed that into the ConfigPort, so that the configuration values would be constantly changing, in synch with the sine wave.

The following example illustrates creating Digital Pin Blocks that are bound to physical pins on the Netduino hardware (in this case pins that correspond to the onboard button and onboard LED), and then connecting the OutputPort of the Button to the InputPort on the LED, such that when the button is pressed (signal goes high), its output is sent directly to the LED and it also goes high (on):

```csharp
public class Program
{
  static H.Cpu.Pin buttonHardware = Pins.ONBOARD_BTN;
  static H.Cpu.Pin ledHardware = Pins.ONBOARD_LED;

  public static void Main()
  {
    // Create the blocks
    var button = new DigitalInputPin (buttonHardware);
    var led = new DigitalOutputPin (ledHardware);

    button.Output.ConnectTo (led.Input);

    // keep the program alive
    while (true) {
      System.Threading.Thread.Sleep (1000);
    }
  }
}
```

## Members

### Constructors

| Signature                                              | Description                                     |
|--------------------------------------------------------|-------------------------------------------------|
| `public Port (BlockBase block, string name, Units units, double initialValue = 0);` | Instantiates a new Port [what] |

### Methods

| Signature                                              | Description                                     |
|--------------------------------------------------------|-------------------------------------------------|
| `ConnectTo(server, writeable = false, name = null)`    |
| `public void ConnectTo (Messaging.ControlServer server, bool writeable = false, string name = null);` |  |
| `public void DisconnectFrom (Port other);`             |

### Properties


| Signature                                              | Description                                     |
|--------------------------------------------------------|-------------------------------------------------|
| `string FullName { get; }` |
| `string FullNameWithUnits { get; }` |
| `string Name { get; }` |
| `double Value { get; set; }` |
| `Units ValueUnits { get; }` |

### Events

| Signature                                              | Description                                     |
|--------------------------------------------------------|-------------------------------------------------|
| `Microsoft.SPOT.EventHandler ValueChanged` |
