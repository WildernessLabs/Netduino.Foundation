# Netduino.Foundation

Netduino.Foundation greatly simplifies the task of building complex .NET Microframework (MF) powered connected things with Netduino, by providing drivers and abstractions for a variety of common peripherals such as sensors, displays, motors, and more. Additionally, it includes utility functions and helpers for common tasks when building connected things, such as a Netduino compatible web server, to functions for creating RGB LED colors from hues.

## Using

To use Netduino.Foundation, simply add a nuget reference to the core library (for core helpers and drivers), or to the specific driver you'd like to use, and the core will come with it.

[image]

### Sample App

[todo: simple sample app illustrating the manual API of a sensor]

### [Supported Peripherals](Documentation/Supported_Peripherals)

For a list of supported peripherals and their usage info, see the [supported peripherals](Documentation/Supported_Peripherals) document.

## Wait, there's more.

Many of the drivers and core helpers adhere to a modular architecture based on the concept of _Blocks_ and _Scopes_ that represent devices and listeners, accordingly. This is to simplify prototyping and quickly connect peripherals and application components in a reactive-like way that more realistically models the underlying circuit. For many complex peripherals, this architecture only exposes the most basic of functionality, and is conceptually similar to .NET primitive calls like .ToString(), in which an object is reduced to a basic representation of its IO meant to provide a fast way to prototype.

This model is completely optional; each driver has been designed to be used in a more traditional, manual API style. A character LCD screen, for instance, might have an API that exposes multiple rows of characters and various styles of text and animation, but using it as a Scope might simply write out a scrolling string to the first line of the screen.


## Contributing

Netduino.Foundation, is open source and community powered, much like Netduino itself. We love pull requests, so if you've got a driver to add, send it on over! For each driver, please include:

 * Documentation, including a Fritzing breadboard schematic on wiring it up and API docs. Please see other drivers for examples.
 * A nuget project file (use the Nuget script to make a new one).
 * Tests, if possible and/or applicable.
 * The datasheet for the peripheral and a link to purchase it, if applicable.

### List of Peripherals TODO

We maintain a list of peripherals that we'd like to support. Feel free to add to this list, or assign yourself if you're actively working on a driver for it.

[todo: link to the list after we clean it up]

# Documentation and Guides

* [Blocks and Scopes Architectural Guide](documentation/architecture.md)
* [API Documentation] TODO - move old xml [API Documentation](Documentation/API_Docs/) into markdown


# License
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

Authors: Bryan Costanich, Frank Krueger, Craig Dunn, Mark Stevens

