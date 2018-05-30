---
layout: Library
title: TextDisplayMenu Library
subtitle: Multiline display menu framework for quick prototyping human interfaces on connected things.
---

# Intro

The `TextDisplayMenu` library is an extensible framework for quickly creating hierarchical, editable menus that can display on an `ITextDisplay` and are driven using either an [`IRotaryEncoder`](/API/Sensors/Rotary/IRotaryEncoder) or [`IButton`](/API/Sensors/Buttons/IButton) interfaces.

![](TextDisplayMenu.gif)

The menu can be completely configured either with JSON or programatically and supports built-in types for time, temperature, and age with the ability to easily create custom types.

# Sample

Creating a menu requires a LCD display, next button, previous button, and select button.  A rotary encoder can replace the next and previous buttons and a rotary encoder with button can replace all three buttons.

```csharp
using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Sensors.Rotary;
using Netduino.Foundation.Displays;
using System.IO;
using TextDisplayMenu_Sample.Properties;
using Netduino.Foundation.Displays.TextDisplayMenu;

namespace TextDisplayMenu_Sample
{
    public class Program
    {
        public static void Main()
        {
            RotaryEncoderWithButton encoder = new RotaryEncoderWithButton(
                N.Pins.GPIO_PIN_D2, N.Pins.GPIO_PIN_D3, N.Pins.GPIO_PIN_D4,
                Netduino.Foundation.CircuitTerminationType.CommonGround);
                
            ITextDisplay display = new SerialLCD(new TextDisplayConfig() { 
                Height = 4, 
                Width = 20 }) as ITextDisplay;

            display.SetBrightness();

            Menu menu = new Menu(_display, _encoder, Resources.GetBytes(Resources.BinaryResources.menu), true);
            menu.Enable();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
```
[View full sample](https://github.com/WildernessLabs/Netduino.Foundation/tree/master/Source/Peripheral_Libs/Displays.TextDisplayMenu)

To create a menu with a rotary encoder or buttons requires the appropriate constructor:

```csharp
// Rotary encoder with button
public Menu(ITextDisplay display, IRotaryEncoderWithButton encoder, byte[] menuResource, bool showBackOnRoot = false)

// Rotary encoder and select button
public Menu(ITextDisplay display, IRotaryEncoder encoder, IButton buttonSelect, byte[] menuResource, bool showBackOnRoot = false)

// Buttons for next, previous, and select
public Menu(ITextDisplay display, IButton buttonNext, IButton buttonPrevious, IButton buttonSelect, byte[] menuResource, bool showBackOnRoot = false)
```

# Loading a Menu From JSON

## Definition

```json
{
  "menu": [
    {
      "text": "My Age: {value}",
      "id": "age",
      "type": "Age",
      "value": 12
    },
    {
      "text": "My Command",
      "command": "DoSomething"
    },
    {
      "text": "Parent",
      "sub": [
        {
          "text": "Sub Item A"
        },
        {
          "text": "Sub Item B"
        },
        {
          "text": "Sub Item C",
          "sub": [
            {
              "text": "Sub Item D"
            },
            {
              "text": "Sub Item E"
            },
            {
              "text": "Sub Item F"
            }
          ]
        }
      ]
    }
  ]
}
```

## Add as a resource

The JSON file with the menu data needs to be added as a resource to the project. Here are the steps:
1. Right-click the project and select Properties
2. Click `Resources` in the left pane
3. Click `Add Resource` and choose the appropriate file.

Now, this resource can be accessed by `Resources.GetBytes(Resources.BinaryResources.[ResourceName])`.

# Handling Events

## Selection Events

To get notified when a menu item with an assigned command is selected, assign a handler to the `Selected` event: `menu.Selected += HandleMenuSelected;`

```csharp
private void HandleMenuSelected(object sender, MenuSelectedEventArgs e)
{
    Debug.Print("menu selected: " + e.Command);
}
```

## Exit Events

To get notified when the menu is exited, assign a handler to the `Exited` event: `menu.Exited += HandleMenuExited`. Also, when instantiating a menu, the `showBackOnRoot` must be set to `True`.

```csharp
private void HandleMenuExited(object sender, EventArgs e)
{
    Debug.Print("menu exited");
}
```

## Edit Events

To get notified when a edit menu item value has changed, assign a handler to the `ValueChanged` event: `menu.ValueChanged += HandleMenuValueChanged`

```csharp
private void HandleMenuValueChanged(object sender, ValueChangedEventArgs e)
{
    Debug.Print(e.ItemID + " changed with value: " + e.Value);
}
```
### Built in Types

The edit menu with collect the input based on the type. For instance, `age` is an integer between 0 and 100. Below is a table with a description of the built in types:

|Type        |Description                                          |
|------------|-----------------------------------------------------|
|Boolean     |A list type including `True` and `False`             |
|Age         |An integer between 0 and 100                         |
|Temperature |A decimal value between -10 and 100 with a scale of 2|
|Time        |24 hour military time with HH:MM                     |
|TimeDetailed|24 hour military time with HH:MM:SS                  |
|TimeShort   |24 hour military time with MM:SS                     |

### Creating Custom Menu Items

Chances are that a custom menu item can be built from the existing base types to get started quickly.  Here are the available base classes to extend from: `NumericBase`, `ListBase`, and `TimeBase`.

#### NumericBase

An example of extending NumericBase is `Age`, a class that inherits from `NumericBase` and specifies the floor, ceiling, and scale of the desired input.

```csharp
using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Displays.TextDisplayMenu.InputTypes
{
    public class Age : NumericBase
    {
        public Age(): base(0, 100, 0)
        {
        }
    }
}
```

#### ListBase
 An example of extending  ListBase is `Boolean`, a class that inherits from `ListBase` and defines the desired list values, in this case `True` and `False`.

 ```csharp
using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Displays.TextDisplayMenu.InputTypes
{
    public class Boolean : ListBase
    {
        public Boolean()
        {
            this._choices = new string[2];
            _choices[0] = "True";
            _choices[1] = "False";
        }
    }
}
```

#### Creating custom types

Creating a custom type is a bit more involved, but achievable.  To start, inherit from `InputBase` and build out the implementation for the following methods:
```csharp
protected abstract void HandlePrevious(object sender, EventArgs e);
protected abstract void HandleNext(object sender, EventArgs e);
protected abstract void HandleRotated(object sender, Sensors.Rotary.RotaryTurnedEventArgs e);
protected abstract void HandleClicked(object sender, EventArgs e);
```

