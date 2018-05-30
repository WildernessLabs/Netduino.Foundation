---
layout: Library
title: TextDisplayMenu Library
subtitle: Multiline display menu framework for quick prototyping human interfaces on connected things.
---

# Intro

The `TextDisplayMenu` library is an extensible framework for quickly creating hierarchical, editable menus that can display on an `ITextDisplay` and are driven using either an [`IRotaryEncoder`](/API/Sensors/Rotary/IRotaryEncoder) or [`IButton`](/API/Sensors/Buttons/IButton) interfaces.

***
 
I would explain what ITextDisplay is/or similar. basically; folks need to know that they can just plug in an LCD screen and we have drivers. 

***


![](TextDisplayMenu.gif)

The menu can be completely configured either with JSON or programmatically and supports built-in types for time, temperature, and age with the ability to easily create custom types.

***

Maybe:

The menu can be created programmatically or loaded from JSON, and has a number of built-in menu item types for display and editing input including time, temperature, and others. Additionally; you can easily create custom menu item types that allow users to edit their value via the inputs.

***

# Using

Creating a menu requires a LCD display, next button, previous button, and select button.  A rotary encoder can replace the next and previous buttons and a rotary encoder with button can replace all three buttons.

***

maybe:

To use the menu, you'll need; an [`ITextDisplay`](/API/Displays/ITextDisplay/) compatible LCD or other display, as well as some combination of buttons and rotary encoder that allows for **next**, **previous**, and **select** functionality. For instance, you can use; three discrete [`IButton`](link) inputs for next/previous/selection, a rotary encoder for next/previous and an `IButton` for selection, or a [`RotaryEncoderWithPushButton`](link) to handle all three inputs.

TODO: add compatible displays to the ITextDisplay page?

***

## Circuit

***
[intro:]

The following schematic illustrates a typical holistic configuration for driving the menu and includes a common 4 line LCD display that's driven directly from Netduino's digital GPIO pins, as well as a rotary encoder with push button:

[need this - can be created in Fritzing]

[photo of it in the appliance control box would be good, too. can link to the 3D stuff]

***

## Sample Code

***

[intro this, something like the following:]

The following code illustrates how to create a new `TextDisplayMenu`, driven by a `RotaryEncoderWithButton` that loads its contents from JSON:

***

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

The full sample can be found [here](https://github.com/WildernessLabs/Netduino.Foundation/tree/master/Source/Peripheral_Libs/Displays.TextDisplayMenu).

### Using Other Inputs

*** I added that heading^ ***

To create a menu with a rotary encoder or buttons requires the appropriate constructor:

*** Maybe:

To create a menu with other inputs, such as buttons or an optional rotary encoder, you can use the other constructors:

[delete the first constructor below, since you show it above.]
***


```csharp
// Rotary encoder with button
public Menu(ITextDisplay display, IRotaryEncoderWithButton encoder, byte[] menuResource, bool showBackOnRoot = false)

// Rotary encoder and select button
public Menu(ITextDisplay display, IRotaryEncoder encoder, IButton buttonSelect, byte[] menuResource, bool showBackOnRoot = false)

// Buttons for next, previous, and select
public Menu(ITextDisplay display, IButton buttonNext, IButton buttonPrevious, IButton buttonSelect, byte[] menuResource, bool showBackOnRoot = false)
```

# Loading a Menu From JSON

*** needs an intro:

To create the menu from JSON, first define the menu contents in a .json file, and then add it as a resource:

***

## Sample Definition

***

[need to explain a little here]

[root node is `menu`, which contains an array of items? should explain the schema here just a little.]

For example, the following json code defines a hierarchical menu arranged in menu pages and items. 

***

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

*** maybe the heading should be "Adding the Menu JSON as a Resource" ***

The JSON file with the menu data needs to be added as a resource to the project. Here are the steps:

***
since we stated this above now in the JSON intro, i would simplify to:

To add the JSON file to the project as a resource:
***

1. Right-click the project and select Properties
2. Click `Resources` in the left pane
3. Click `Add Resource` and choose the appropriate file.

Now, this resource can be accessed by `Resources.GetBytes(Resources.BinaryResources.[ResourceName])`.

# Handling Events

***
explain that the menu raises events on various interactions.
***

## Selection Events

To get notified when a menu item with an assigned command is selected, assign a handler to the `Selected` event: `menu.Selected += HandleMenuSelected;`

```csharp
private void HandleMenuSelected(object sender, MenuSelectedEventArgs e)
{
    Debug.Print("menu selected: " + e.Command);
}
```

## Exit Event

***
need to explain that this is built-in optional functionality first. 
***

To get notified when the menu is exited, assign a handler to the `Exited` event: `menu.Exited += HandleMenuExited`. Also, when instantiating a menu, the `showBackOnRoot` must be set to `True`.

```csharp
private void HandleMenuExited(object sender, EventArgs e)
{
    Debug.Print("menu exited");
}

*** 
maybe show as a lambda? it feels a little disjointed right now.
***
```

## Edit Events

To get notified when a edit menu item value has changed, assign a handler to the `ValueChanged` event: `menu.ValueChanged += HandleMenuValueChanged`

```csharp
private void HandleMenuValueChanged(object sender, ValueChangedEventArgs e)
{
    Debug.Print(e.ItemID + " changed with value: " + e.Value);
}
```

***
lambda?
***

# Built in Types

***
i wrapped values in backticks, fyi
***

The edit menu with collect the input based on the type. For instance, `age` is an integer between `0` and `100`. Below is a table with a description of the built in types:

***
a little unclear, maybe simplify since we introduce the concept earlier?

The following table enumerates the built-in menu item types, and their associated usage and values:
***


|Type        |Description                                          |
|------------|-----------------------------------------------------|
|Boolean     |A list type including `True` and `False`             |
|Age         |An integer between 0 and 100                         |
|Temperature |A decimal value between -10 and 100 with a scale of 2|
|Time        |24 hour military time with HH:MM                     |
|TimeDetailed|24 hour military time with HH:MM:SS                  |
|TimeShort   |24 hour military time with MM:SS                     |

***
You'll run into menu formatting/parsing issues in some places without spaces, FYI (plus, it looks better/easier to read). Also think you should backtick types and values (also, true and false are lower-case):

| Type           | Description                                                |
|----------------|------------------------------------------------------------|
| `Boolean`      | A list type including `true` and `false`.                  |
| `Age`          | An integer between `0` and `100`.                          |
| `Temperature`  | A decimal value between `-10` and `100` with a scale of 2. |
| `Time`         | 24 hour military time with `HH:MM`                         |
| `TimeDetailed` | 24 hour military time with `HH:MM:SS`                      |
| `TimeShort`    | 24 hour military time with `MM:SS`                         |

***

# Creating Custom Menu Item Types

Chances are that a custom menu item can be built from the existing base types to get started quickly.  Here are the available base classes to extend from: `NumericBase`, `ListBase`, and `TimeBase`.

*** 
I would give a general overview here first, and rearrange the headings based on that:

There are two ways to create custom menu items. The easiest and most common is to inherit from, and modify, the built-in base types. However, you can also create completely custom menu item types.


## Customizing Built-In Base Types

`TextDisplayMenu` includes a number of built-in base types that handle common types input and can be customized to suit:

| Base Type     | Description                                                   |
|---------------|---------------------------------------------------------------|
| `NumericBase` | Provides a generic numeric display and input. The min/max, and number of decimal places can be modified. |
| `TimeBase`    | [describe] |
| `ListBase`    | Provides a selectable list of items. |

***

### Custom NumericBase Example [note i modified the heading title to make descriptive]

An example of extending NumericBase is `Age`, a class that inherits from `NumericBase` and specifies the floor, ceiling, and scale of the desired input.

***

Maybe:

The following code is pulled from the `Age` menu type, and illustrates how to....

***

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

### Custom ListBase Example

An example of extending  ListBase is `Boolean`, a class that inherits from `ListBase` and defines the desired list values, in this case `True` and `False`.

*** see previous section edit ***

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

## Creating custom types

Creating a custom type is a bit more involved, but achievable.  To start, inherit from `InputBase` and build out the implementation for the following methods:

```csharp
protected abstract void HandlePrevious(object sender, EventArgs e);
protected abstract void HandleNext(object sender, EventArgs e);
protected abstract void HandleRotated(object sender, Sensors.Rotary.RotaryTurnedEventArgs e);
protected abstract void HandleClicked(object sender, EventArgs e);
```

*** 

do we have an example?

***

# Troubleshooting

***

we need a pic of the garbage and mention that it's likely a faulty connection, often associated with breadboards, and to make sure to check all the GNDs

***