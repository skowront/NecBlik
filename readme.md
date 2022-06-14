# NecBlik

NecBlik is an open-source, C#, application for monitoring and controlling networks. It was created for ZigBee network using XBee devices but can be easily extended to support any network. NecBlik is a name taken from silesian language, where Nec can be translated as Network and Blik can be translated as a glimpse, because the application allows us to take a glimpse on our network :).

## Overview

//TODO add photo of the application

## Features

- ## Requirements

- Windows,

- .NET 6.0,

- Python 3.7.9 - optional

## Installation

Simply download a release package from this repository and run NecBlik.exe.

## Compiling

If you want to compile this project on your own please keep in mind that by default it allows usage of PyDigi module that uses python 3.7.9. To turn off PyDigi, simply remove it from NecBlik project attached dependency projects and remove post build events (in NecBlik project) that copies Python scripts to output directory.  

## Architecture

Necblik applicaton is composed of few projects that are together made into an application in NecBlik project.  

### Packages/Modules/Libraries(Submodules)

Here we define some concepts that should explain the modular architecture of NecBlik.

Packages - are projects that are packed as "unpack them into particular folder to attach them to NecBlik" or NecBlik itself. (Release files are composed of packages).

Modules - are projects that implement a network type functionality  (as ZigBee, Wifi, etc.) 

Libraries(Submodules) - are projects  that inherit from a module and implement specific ViewModels and Controls of a module.

### Core projects

- NecBlik - main project that compiled runs NecBlik application,

- NecBlik.CLI - project made for testing, or preparing short scripts that use NecBlik backend to communicate with the network,

- NecBlik.Common - project containing styles, and ResponseProviders that speeds up UI building,

- NecBlik.Core - it contains the core of the NecBlik backend, including abstract classes and bases for any other modules.  

- NecBlik.Core.GUI - contains core of the NecBlik frontend, and ViewModel logic,

- NecBlik.Virtual - this project was made to create a basic implementation of NecBlik.Core classes that simply work. It allows us to add virtual devices that user may interact with.

- NecBlik.Virtual.GUI - it contains GUI for NecBlik.Virtual and ViewModel logic,

### ZigBee - Xbee projects C#

There are two large projects that implement NecBlik.Core features through inheritance from NecBlik.Virtual. They adjust NecBlik so that it works with ZigBee network containing Xbee devices. They are widely extensible and contain a lot of features that may be just removed if not needed for runtime customization.

- NecBlik.Digi - this project contains ported backend for Digi Library for C# [GitHub - digidotcom/xbee-csharp: C# library to interact with Digi International&#39;s XBee radio frequency modules from mobile devices.](https://github.com/digidotcom/xbee-csharp)

- NecBlik.Digi.GUI - it contains GUI and ViewModel logic for NecBlik.Digi

Those projects simply work, so feel free to attach your Xbee coordinator (Xbee 2.0 or 3.0) to your serial port, add network and discover devices. Extensions and customization will be covered in sections somewhere below. 

### ZigBee - Xbee projects Python (C# + Python with Pythonnnet)

This part of NecBlik was created purely for research purposes. It is however perfectly possible to use them to attach an existing application made in python for Xbee, but it will require a complete analysis of how pythonnet package was implemented and how to inject code from C# into python scripts. In case you want to use/edit this part of code feel free to add an issue on github, I will try to answer any questions as it can be tough to go through this alone.

There are 4 main projects using pythonnet: 

- NecBlik.PyDigi - "inherits" from NecBlik.Virtual and implements network functionalities so that they use DigiInternational Library for Python as backend.

- NecBlik.PyDigi.GUI - provides GUI and ViewModel logic for NecBlik.Pydigi.

- NecBlik.PyDigi.Python - a python project containing example scripts that are injected into pythonnet environment and a virtual python Environment.

- NecBlik.PyDigi.Test - project containing tests for NecBlik.Pydigi.Core

### Other (external) projects

NecBlik uses two main external projects that were slightly adjusted for this particular purpose. Those projects are:

- DiagramsExtension - from [WPF Diagram Designer - Part 4 - CodeProject](https://www.codeproject.com/Articles/24681/WPF-Diagram-Designer-Part-4)

- WpfLocalizeExtension - from [GitHub - XAMLMarkupExtensions/WPFLocalizeExtension: LocalizationExtension is a the easy way to localize any type of DependencyProperties or native Properties on DependencyObjects](https://github.com/XAMLMarkupExtensions/WPFLocalizeExtension)

### Research projects (not important)

NecBlik was made as a part of Master's Thesis project and contains some projects that are not really usefull except for R&D purposes. Those projects are:

- NecBlik.Diagnostics.CLI - console program that allows throughput measurement for DigiDevices.

- NecBlik.Performance.CLI - a console application for researchers to compare C# and Python modules.

## Other solutions (not important)

This repo contains two helper solutions. One for monitoring a ZigBeeDevice and one with code for Arduino Romeo with an XBee module. They are located in ZigBee.Arduino and ZigBeeEndDeviceMonitor folders.

## Usage

As noted earlier, NecBlik is an application made mainly for ZigBee network with Xbee deivces. Therefore without extension it works only with this devices. NecBlik itself (without attached dlls) provides:

- possibility to add a map (an image in .svg or .png format),

- change GUI language.

To make any use of the application we must attach specific .dll files with NecBlik name in them. If you want to compile the project yourself, then just add/remove references in "NecBlik" project.
![](https://github.com/skowront/NecBlik/blob/virtual/Manual/Images/2022-06-14-17-05-54-image.png)

You may also want to remove post-build events.

In case you want to use a release package, just unpack the main zip and paste particular package releases into that zip. Keep in mid that you must paste modules into main NecBlik folder, and any submodules into Libraries/[modulename]/ folder. 

As soon as we get this done we can run NecBlik.exe and see something like this:

![](https://github.com/skowront/NecBlik/blob/virtual/Manual/Images/2022-06-14-17-21-07-image.png)

Here you can try some basic functionalities. 

### Scenarios of usage

Now, let's talk about scenarios of usage.

#### Project renaming

First of all we want to rename the project. 

1. Goto project, rename.

2. A window will open, just type a new name and close the window.

After closing you will see that the project name has changed:

![](https://github.com/skowront/NecBlik/blob/virtual/Manual/Images/2022-06-14-17-25-41-image.png)

#### Project background (map)

Now, let's add a background map to our project (it can be any .svg or .png).

1. Goto Map, Load, 

2. Select a file and open it (the map will be stored in cache for now),

3. Optional - you can use Map, Resize to adjust map scale to make it bigger/smaller.

#### Saving a project

Perfect, now that we have a map we can save our basic project. 

1. Goto Project, Save.

2. Select where will be the project saved (a folder of name of the project will be created, but keep in mind that if such a folder already exists the NecBlik will purge it).

3. Done.

#### Loading a project

To load a project simply goto Project, Load and open a .json file in project's save folder.

#### Attaching a network

Let's say we have plugged our network coordinator into our pc's serial port and we want to add our network to NecBlik. Due to NecBlik initial purpose we will use Virtual network as an example.

1. Goto Project, Add network,

2. Select your module (we will select Virtual),

3. Further steps may differ depending on module.

4. ![](https://github.com/skowront/NecBlik/blob/virtual/Manual/Images/2022-06-14-17-40-32-image.png)

5. Enter desided parameters.

6. ![](https://github.com/skowront/NecBlik/blob/virtual/Manual/Images/2022-06-14-17-41-16-image.png)

7. In 6. we select NetworkViewModel and CoordinatorViewModel embedded in module or taken from a submodules (NecBlik will detect them).

8. After clicking confirm, open the expander on the right side of application. Your network will be there. You can open it to see:

9. ![](https://github.com/skowront/NecBlik/blob/virtual/Manual/Images/2022-06-14-17-43-05-image.png)

10. Now you can either open the devices or send them to map and adjust their scale/position.

11. When finished simply save your project (select directory where project directory is located).

Now let's suppose we choose default NetworkViewModel. Then the window would look like this:

![](https://github.com/skowront/NecBlik/blob/virtual/Manual/Images/2022-06-14-18-11-59-image.png)

As you can see we get some additonal (however default) options. Digi module has even more functionalities. 

Poll devices will simply chech if devices are living or not. 

Rules allows us to customize viewmodel/control rules of our Network (we can change behavior of devices that correspond physical/virtual ones). We can also send pings to our devices.

#### Rules

Rules is a mechanism for runtime customization of application viewmodels and classes. Every physical/virtual device consitst of uneditable functionality class (model) and a viewmodel that can differently react to incoming/outgoing messages. Rules can be entered manually in runtime, or applied in constructor of a NetworkViewModel class (it is especially handy when you hardcode a new NetworkViewModel as a submodule for a module).

There are two main types of rules.

1. ViewModel Rules, that define the behavior/class of a physical/virtual device in NecBlik,

2. Control Rules, that define how a physical/virtual device will be shown on the map in NecBlik.

Other rules can be specified on a Module level (that can inherit from an existing module).

## Supported languages

NecBlik supports:

- English (default),

- French,

- Polish.



## Extending application

There are many ways to extend NecBlik. With NecBlik it is possible to do any of the following:

- add your own module factory that allows cooperation with new network type (MQTT, WiFi, RFID, etc...),

- add your own submodule (for existing module or your own module),

- simply hardcode whatever you want into the application.


