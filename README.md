# [Netduino.Foundation](http://Netduino.Foundation)

Netduino.Foundation greatly simplifies the task of building complex .NET Microframework (MF) powered connected things with Netduino, by providing drivers and abstractions for a variety of common peripherals such as sensors, displays, motors, and more. Additionally, it includes utility functions and helpers for common tasks when building connected things, such as a functions for creating RGB LED colors from hues.

## Website

If you're interested in contributing to Netduino.Foundation, or just want to peek at the source code, you're in the right place. If you want to use Netduino.Foundation, check out the website at [Netduino.Foundation](http://Netduino.Foundation).

## Using

To use Netduino.Foundation, simply add a nuget reference to the core library (for core helpers and drivers), or to the specific driver you'd like to use, and the core will come with it.

For more information see the [getting started](http://Netduino.Foundation/Getting_Started) guide on the official [Netduino.Foundation](http://Netduino.Foundation) site.


## [Supported Peripherals](http://Netduino.Foundation/Library)

For a list of supported peripherals and their usage info, see the [peripheral library](http://Netduino.Foundation/Library) page.

## Contributing

Netduino.Foundation, is open source and community powered, much like Netduino itself. We love pull requests, so if you've got a driver to add, send it on over! For each peripheral driver, please include:

 * **Documentation** - Including a Fritzing breadboard schematic on wiring it up, sourcing info, and API docs. Please see other drivers for examples. Documentation is hosted on the [Netduino.Foundation](http://Netduino.Foundation) GitHub pages site and should be placed in the appropriate place in the `/docs/` folder.
 * **Datasheet** - For the peripheral, if applicable.
 * **Sample** - Application illustrating usage of the peripheral.
 * **Nuspec file** - For building the peripheral's Nuget package (if it's an external peripheral). See other peripherals for an example.

### Publishing Updated Nuget Packages

When any of the libraries have their assembly version bumped, and that change is checked in, our CI system will automatically build a new package and upload it to Nuget.org (yay!). 

Netduino apps are constrained to VS2015 (VS2017 support coming soon), which doesn't support the latest Nuget Package Manager. As such, it has a bug related to package dependencies. Specifically; if two Netduino.Foundation packages require different minimum versions of the Netduino.Foundation Core, say 0.12, and 0.13, then they cannot be used in the same project, because the version of Nuget in VS2015 will not use 0.12 as the minimum and utilize the 0.13 package. 

As such, whenever a change to the Netduino.Foundation.Core is made, _every_ peripheral package needs to have its Netduino.Foundation.Core dependency to be updated to point to the latest core.

This is a lot of work to do, manually, so we have a script in the `./source` folder called `UpdateNFDependencyVersion.ps1`. It will update the `<dependency.../>` node in each of the project's .nuspec file AND increment the version number of the library in the `Assembly.cs` folder, so that the nuget package will automatically be built and updated.

To run the script, open up a terminal window, navigate to the `./source` folder and execute:

```
.\UpdateNFDependencyVersion.ps1 0.13
```

Replace `0.13` with whatever latest version of the Netduino.Foundation.Core package is at.

Note: this script has a small bug that often throws an error. If it does, don't panic, just do a git checkout of master again and re-run the script. If a checkout doesn't clean up the changes that the script did, use `git reset --hard HEAD`.

### List of Peripherals TODO

We maintain a list of peripherals that we'd like to support. Feel free to add to this list, or assign yourself if you're actively working on a driver for it.

[todo: link to the list after we clean it up]

# Documentation and Guides

Please see the documentation and guides at [Netduino.Foundation](http://netduino.foundation).

# Source Info

The Netduino.Foundation project is broken into the following source folders:

* **`/docs`** - This is the source for the [Netduino.Foundation](http://Netduino.Foundation) GitHub pages site.
* **`/Source`** - The home of the source code for the libraries.
  * **`/Source/Netduino.Foundation`** - Netduino.Foundation core library project.
  * **`/Source/Netduino.Foundation.Core.Samples`** - Samples for the core library peripherals.
  * **`/Peripheral_Libs`** - Home of all the external peripheral drivers, their samples, and datasheets.
* **`/Design`** - Design files for the website, logo, etc.

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

