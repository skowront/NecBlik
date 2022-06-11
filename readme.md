# NecBlik

NecBlik is an open-source, C#, application for monitoring and controlling networks. It was created for ZigBee network using XBee devices but can be easily extended to support any network.

## Overview

//TODO add photo of the application

## Features

- 

## Requirements

- Windows,

- .NET 6.0,

- Python 3.7.9 - optional

## Installation

Simply download a release package from this repository and run NecBlik.exe.

## Compiling

If you want to compile this project on your own please keep in mind that by default it allows usage of PyDigi module that uses python 3.7.9. To turn off PyDigi, simply remove it from NecBlik project attached dependency projects and remove post build events (in NecBlik project) that copies Python scripts to output directory.  

## Architecture

Necblik applicaton is composed of few projects that are together made into an application in NecBlik project.  

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

## Extending application

There are many ways to extend NecBlik. With NecBlik it is possible to do any of the following:

- add your own module factory that allows cooperation with new network type (MQTT, WiFi, RFID, etc...),

- add your own submodule (for existing module)


