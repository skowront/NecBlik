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

REMARK! Remember to use Sync() or a Synchronize button after changing factory rules.

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

Extendind the app may define either behaviour (of network, coordinator or device), presentation (of network, coordinator, device) or both.

We will cover extending the application from the least to the most complicated.

### General exntension remarks

Any extension of the application will inherit classes from NecBlik.Core and NecBlik.Core.GUI Interfaces or Models either directly or indirectly. 

Keep in mind that the application uses MahApps metro windows and therefore created windows should not inherit from window, and should be changed to MahApps.MetroWindow control in xaml.

Note that EVERY object has a unique CacheId for a Network and each network must have a unique CacheId in whole application or you may expect the app to be unstable or crash.

#### Translation and localization

NecBlik uses localization extension. To add a localization extension just add this code to your xaml.

```xml
xmlns:lexp="clr-namespace:WPFLocalizeExtension.Providers;assembly=WPFLocalizeextension"
xmlns:lex="clr-namespace:WPFLocalizeExtension.Extensions;assembly=WPFLocalizeextension"
lexp:ResxLocalizationProvider.DefaultAssembly="NecBlik.xxx.GUI"
lexp:ResxLocalizationProvider.DefaultDictionary="SR"
```

Create a SR.resx in Strings directory and you can use the translations for example.

```xml
Text="{lex:Loc NecBlik.Digi.Gui:SR:GPTemperature}"
```

### Hardcoded extension

If you intend to never update NecBlik, you can simply take the code and adjust it in any way you want. That is however not the case we will cover in this section. We will cover here adding classes so they become embedded with the output binaries, but does not interfere with existing code (and therefore can be still updated with new repository versions).

We will use a simple scenario.

Let's say we have a remote device that every X seconds sends us current value of temperature sensor. We want our ViewModel to store that value, and we want to see this device as a "termometer" on our map. We also want to see a bar with color of the temperature when we click on the device on map. 

For hardcoded extension we will need our NecBlik.Digi.GUI project that has a folder Examples where we can put our code.

REMARK!!! The same can be done for NecBlik.Virtual.GUI

#### Presentation

Let's start with adding presentation (Windows) for our particular purpose. 

##### Device

###### Window

Full example code is available in the code on this repository. 

In Examples/Views/Sources we create a new window, called "TemperatureDeviceWindow". We change the window type to "mah:MetroWindow" and add "xmlns:mah="http://metro.mahapps.com/winfx/xaml/controls"" to our code.

Remark: remember to remove inheritance in TemperatureDeviceWindow.xaml.cs.

Now let's add a temperature value indicator.

```xml
<StackPanel Grid.Row="1" HorizontalAlignment="Center" Orientation="Horizontal" Margin="0 10 0 0">
           <TextBlock Text="{lex:Loc NecBlik.Digi.Gui:SR:GPTemperature}" Foreground="{StaticResource MaterialDesignDarkForeground}"></TextBlock>
           <TextBlock Text="{Binding Temperature, FallbackValue='?'}"  Margin="10 0 0 0"  Foreground="{StaticResource MaterialDesignDarkForeground}"></TextBlock>
</StackPanel>
```

Then we add a status indicator.

```xml
<StackPanel Grid.Row="2" Orientation="Horizontal" HorizontalAlignment="Center">
           <TextBlock Text="{lex:Loc NecBlik.Core.Gui:SR:GPStatus}" Foreground="{StaticResource MaterialDesignDarkForeground}"></TextBlock>
           <TextBlock Text="{Binding Status, FallbackValue='?'}" Margin="10 0 0 0" Foreground="{StaticResource MaterialDesignDarkForeground}"></TextBlock>
</StackPanel>
```

And let's add a bar.

```xml
<Rectangle Grid.Row="1" Height="20">
                <Rectangle.Fill>
                    <SolidColorBrush Color="{Binding TemperatureColor}" />
                </Rectangle.Fill>
</Rectangle>
```

Note how we can just bind to properties of our ViewModels (that we will create later).

Note how we can use translation with lex:Loc.

REMARK!!! Window will be attached in behavior. 

```csharp
public TemperatureDeviceViewModel(DeviceModel model, NetworkViewModel networkModel) : base(model, networkModel)
        {
            this.EditCommand = new RelayCommand((o) =>
            {
                var window = new TemperatureDeviceWindow(this);
                window.Show();
            });
        }
```

###### Control

In Examples/Views/Controls we create a new UserControl, called "TemperatureControl.xaml". A full example is available in code, so for simplicity we can just show that it is possible to bind to properties of viewmodel (and everything that it inherited):

```xml
<Grid.ToolTip>
                    <Grid>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="auto"></ColumnDefinition>
                            <ColumnDefinition Width="*"></ColumnDefinition>
                        </Grid.ColumnDefinitions>
                        <Grid.RowDefinitions>
                            <RowDefinition></RowDefinition>
                            <RowDefinition></RowDefinition>
                            <RowDefinition></RowDefinition>
                            <RowDefinition></RowDefinition>
                            <RowDefinition></RowDefinition>
                            <RowDefinition></RowDefinition>
                        </Grid.RowDefinitions>
                        <TextBlock Grid.Row="0" Grid.Column="0" VerticalAlignment="Center" HorizontalAlignment="Left" Text="{lex:Loc NecBlik.Core.GUI:SR:GPGuid}"></TextBlock>
                        <TextBlock Grid.Row="0" Grid.Column="1" VerticalAlignment="Center" HorizontalAlignment="Left" Margin="0 0 5 0" Text="{Binding Name, FallbackValue='???'}"></TextBlock>
                        <TextBlock Grid.Row="1" Grid.Column="0" VerticalAlignment="Center" HorizontalAlignment="Left" Text="{lex:Loc NecBlik.Core.GUI:SR:GPGuid}"></TextBlock>
                        <TextBlock Grid.Row="1" Grid.Column="1" VerticalAlignment="Center" HorizontalAlignment="Left" Margin="0 0 5 0" Text="{Binding Guid, FallbackValue='???'}"></TextBlock>
                        <TextBlock Grid.Row="2" Grid.Column="0" VerticalAlignment="Center" HorizontalAlignment="Left" Text="{lex:Loc NecBlik.Core.GUI:SR:GPAddress}"></TextBlock>
                        <TextBlock Grid.Row="2" Grid.Column="1" VerticalAlignment="Center" HorizontalAlignment="Left" Margin="0 0 5 0" Text="{Binding Address, FallbackValue='???'}"></TextBlock>
                        <TextBlock Grid.Row="3" Grid.Column="0" VerticalAlignment="Center" HorizontalAlignment="Left" Text="{lex:Loc NecBlik.Core.GUI:SR:GPFactory}"></TextBlock>
                        <TextBlock Grid.Row="3" Grid.Column="1" VerticalAlignment="Center" HorizontalAlignment="Left" Margin="0 0 5 0" Text="{Binding InternalFactoryType, FallbackValue='???'}"></TextBlock>
                        <TextBlock Grid.Row="4" VerticalAlignment="Top" HorizontalAlignment="Left" Text="{Binding Temperature, StringFormat='{}{0}°C'}"></TextBlock>
                        <TextBlock Grid.Row="5" Grid.Column="0" VerticalAlignment="Center" HorizontalAlignment="Left" Text="{lex:Loc NecBlik.Core.GUI:SR:GPStatus}"></TextBlock>
                        <TextBlock Grid.Row="5" Grid.Column="1" VerticalAlignment="Center" HorizontalAlignment="Left" Margin="0 0 5 0" Text="{Binding Status}"></TextBlock>
                    </Grid>
</Grid.ToolTip>
```

REMARK!!! The control must implement IDeviceControl to be found by the assembly. 

##### Coordinator

Coordinator and Devices both have the same ContorolType (IDeviceControl) so if you want to change window/control of the coordinator just do the same as you would do if it was a device.

##### Network

Network Window can be customized with ViewModel (Behaviour).

```csharp
namespace NecBlik.Digi.GUI.Examples.ViewModels.Networks
{
    public class DigiNetworkExampleViewModel : Digi.GUI.ViewModels.DigiZigBeeNetworkViewModel
    {
        public DigiNetworkExampleViewModel(Network network) : base(network)
        {
            this.EditCommand = new Common.WpfExtensions.Base.RelayCommand((o) =>
            {
                var window = new Digi.GUI.Views.DigiNetworkWindow(this);
                window.Show();
            });
        }
    }
}
```

Here you can just change what window is being created. A network window should have a constructor that takes a NetworkViewModel (or inherited one) as a parameter and attach it as a data context just like:

```csharp
public partial class DigiNetworkWindow
    {
        public DigiZigBeeNetworkViewModel ViewModel { get; set; }

        public DigiNetworkWindow()
        {
            InitializeComponent();
        }

        public DigiNetworkWindow(DigiZigBeeNetworkViewModel vm) : this()
        {
            this.ViewModel = vm;
            this.DataContext = this;
        }
    }
```

#### Behaviour

##### Device

Keeping in mind our scenario we will now add a backend for our Device.

We create a class in Examples/ViewModels/Sources and we inherit from our main Module Device class (here DigiZigBeeViewModel)

```csharp
public class TemperatureDeviceViewModel : DigiZigBeeViewModel
```

Then we add desired properties:

```csharp
private double? temperature = null;
        public double? Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                if(value>this.maxRecordedTemperature)
                {
                    this.maxRecordedTemperature = value ?? this.maxRecordedTemperature;
                }
                if(value<this.minRecordedTemperature)
                {
                    this.minRecordedTemperature = value ?? this.minRecordedTemperature;
                }
                this.temperature = value;
                this.TemperatureColor = TemperatureDeviceViewModel.TemperatureToColor(this.temperature ?? 0, this.minRecordedTemperature, this.maxRecordedTemperature);
                this.OnPropertyChanged();
            }
        }

        private Color temperatureColor = Colors.White;
        public Color TemperatureColor {
            get 
            { 
                return this.temperatureColor; 
            }
            set { 
                this.temperatureColor = value; this.OnPropertyChanged(); 
            }
        } 
```

We then override OnRecievedDataSentFromSourceDevice so that when an incoming message comes we convert it to a double value and set our props.

```csharp
public override void OnRecievedDataSentFromSourceDevice(string data, string sourceAddress)
        {
            var splitData = data.Split(':');
            var parsed = 0.0d;
            if (splitData.Count() > 1)
                if(double.TryParse(splitData[1],out parsed))
                    this.Temperature = parsed;
            else if (double.TryParse(data, out parsed))
                    this.Temperature = parsed;
        }
```

Done.

Now either you will add a rule to this device during runtime so that it uses this particular viewmodel or you can hardcode it in network behavior.

For example you can do something like this:

```csharp
namespace NecBlik.Digi.GUI.Examples.ViewModels.Networks
{
    public class DigiNetworkExampleViewModel : Digi.GUI.ViewModels.DigiZigBeeNetworkViewModel
    {
        public DigiNetworkExampleViewModel(Network network) : base(network)
        {
            this.FactoryRules.Add(new Core.GUI.Factories.ViewModels.FactoryRuleViewModel(new Core.Factories.FactoryRule() { CacheObjectId="DeviceAddress", Property=Digi.GUI.Factories.DigiZigBeeGuiFactory.DeviceViewModelRuledProperties.ViewModel, Value=typeof(Digi.GUI.Examples.ViewModels.Sources.TemperatureDeviceViewModel).FullName}));
            this.Sync();//Remember to use sync so that the rules are added to model and saved to a file later
        }
    }
}
```

##### Coordinator

Extending coordinator is done in the same way as extending a Device. So we can simply add a new class in Examples/ViewModels/Coordinators and inherit from main coordinator in our Module.

```csharp
namespace NecBlik.Digi.GUI.Examples.ViewModels.Coordinators
{
    public class TemperatureCoordinatorViewModel: DigiZigBeeCoordinatorViewModel
    {
        public TemperatureCoordinatorViewModel(DeviceModel model, NetworkViewModel networkModel) : base(model, networkModel)
        {

        }

        
    }
}
```

Then we simply override a method NotifySubscriber that is called whenever an information comes to our Coordinator.

```csharp

namespace NecBlik.Digi.GUI.Examples.ViewModels.Coordinators
{
    public class TemperatureCoordinatorViewModel: DigiZigBeeCoordinatorViewModel
    {
        public TemperatureCoordinatorViewModel(DeviceModel model, NetworkViewModel networkModel) : base(model, networkModel)
        {

        }

        public override void NotifySubscriber(RecievedData updateInformation)
        {
            base.NotifySubscriber(updateInformation);

            var sourceVms = this.Network.GetDeviceViewModels().Where((d) => { return d.Address == updateInformation.SourceAddress; });
            if(sourceVms!=null)
            {
                if(sourceVms.Any())
                {
                    var sourceVm = sourceVms.First();
                    sourceVm?.OnRecievedDataSentFromSourceDevice(updateInformation.Data,updateInformation.SourceAddress);
                }
            }
        }
    }
```

By calling 

```csharp
base.NotifySubscriber(updateInformation);
```

we are calling a function that will add the RecievedData to incoming buffer entries.

##### Network

Changing the behavior of the network is quite simple. As mentioned in Presentation/Network section we can write a .cs class:

```csharp
namespace NecBlik.Digi.GUI.Examples.ViewModels.Networks
{
    public class DigiNetworkExampleViewModel : Digi.GUI.ViewModels.DigiZigBeeNetworkViewModel
    {
        public DigiNetworkExampleViewModel(Network network) : base(network)
        {
            this.EditCommand = new Common.WpfExtensions.Base.RelayCommand((o) =>
            {
                var window = new Digi.GUI.Views.DigiNetworkWindow(this);
                window.Show();
            });
        }
    }
}

```

In constructor we have access to most parent features and we can edit them. For example we can edit button commands or add our own (and use them in our custom network window). 

REMARK!!! Keep in mind that after adding a network in NecBlik you cant change it from the user's side, you may change coordinator class and control, you may change device viewmodels and controls but you cant change network viewmodel.

### Submodule extension

A submodule extension is almost as easy as a hardcoded extension. You now know how to create classes that represent physical devices and how to customize their behaviour. We will show you now how to add new windows and viewmodels in a separate project and how to make it visible for NecBlik.

1. Create new project (with NecBlik and Module name in it for example NecBlik.Digi.Example),

2. Change traget framework for:

3. ```xml
   <TargetFramework>net6.0-windows</TargetFramework>
   ```

4. Attach NecBlik.Digi.Gui to dependencies (Add project reference)

5. Create a viewmodel class (you can do the same for windows and other viewmodels) //code below.

6. Compile.

7. Take compiled binary (NecBlik.Digi.Example.dll) and add it to your NecBlik folder in Libraries/[modulename]/

8. Done.

Viewmodel code:

```csharp
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.Digi.GUI.Examples.Views.Sources;
using NecBlik.Digi.GUI.ViewModels;
using System.Windows.Media;

namespace NecBlik.Digi.Example.ViewModels.Sources
{
    public class ExternalTemperatureDeviceViewModel : DigiZigBeeViewModel
    {
        private double maxRecordedTemperature = 0;
        private double minRecordedTemperature = 0;

        private double? temperature = null;
        public double? Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                if (value > this.maxRecordedTemperature)
                {
                    this.maxRecordedTemperature = value ?? this.maxRecordedTemperature;
                }
                if (value < this.minRecordedTemperature)
                {
                    this.minRecordedTemperature = value ?? this.minRecordedTemperature;
                }
                this.temperature = value;
                this.TemperatureColor = ExternalTemperatureDeviceViewModel.TemperatureToColor(this.temperature ?? 0, this.minRecordedTemperature, this.maxRecordedTemperature);
                this.OnPropertyChanged();
            }
        }

        private Color temperatureColor = Colors.White;
        public Color TemperatureColor
        {
            get
            {
                return this.temperatureColor;
            }
            set
            {
                this.temperatureColor = value; this.OnPropertyChanged();
            }
        }

        public ExternalTemperatureDeviceViewModel(DeviceModel model, NetworkViewModel networkModel) : base(model, networkModel)
        {
            this.EditCommand = new RelayCommand((o) =>
            {
                var window = new TemperatureDeviceWindow(this);
                window.Show();
            });
        }

        public override void OnRecievedDataSentFromSourceDevice(string data, string sourceAddress)
        {
            var splitData = data.Split(':');
            var parsed = 0.0d;
            if (splitData.Count() > 1)
                if (double.TryParse(splitData[1], out parsed))
                    this.Temperature = parsed;
                else if (double.TryParse(data, out parsed))
                    this.Temperature = parsed;
        }

        private static Color TemperatureToColor(double temperature, double min, double max)
        {
            var color = new Color();
            color.A = 255;
            if (temperature >= 0)
            {
                color.R = 255;
                color.G = (byte)(255 - (temperature / max) * 255);
                color.B = (byte)(255 - (temperature / max) * 255);
            }
            else
            {
                color.R = (byte)(255 - (Math.Abs(temperature) / Math.Abs(min)) * 255);
                color.G = (byte)(255 - (Math.Abs(temperature) / Math.Abs(min)) * 255);
                color.B = 255;
            }
            return color;
        }
    }
}


```

This process can be repeated for Coordinator/Device/Network ViewModels (Behaviour) and Coordinator/Device/Network UserControls (Presentation). When you create all your vms, and ucs simply run NecBlik and configure your Network to use them in runtime or if done earlier in backend network constructor.

### Module extension

Extendind NecBlik with a module can add a functionality that will allow usage of other Networks. It is however not that easy and simple as it requires to create a new NetworkFactory and NetworkGuiFactory. 

It is however worth a while to analyze module extension because it allows us also to extend backend of an existing library - Models. (For example Coordinators are created by passing a backend factory that creates backend Models of Devices, when we create a new Module, we can override BuildNewSource function in factory and our coordinator will produce our new custom sources Models on discovery that will be later available for viewmodels).

This process can be simplified by using VirtualDeviceFactory and VirtualGuiDeviceFactory as base instead of default NecBlik.Core and NecBlik.Core.GUI classes.

REMARK!!! Nec blik was created in such a way that if you do not implement a feature (or forget to override a function) the application should still work and take default/classes/templates and try to apply them to your application.

#### Backend (NecBlik.[moduleName])

First of all we need to understand the basic architecture of factory/module model.

##### DeviceFactory

DeviceFactory in NecBlik.Core has some important functions. It's most important function is to save/load backend from .json files and generate new backend objects.

1. Constructor - parameterless, here we specify in base classes new unique factory identifier by writing this.internalFactoryType = "{myFactoryIdentifier}". It works like a vendor id in real life.

2. string GetVendorId() - it returns the unique identifier of the module. Must always be overriden when creating a new module.

3. IDeviceSource BuildNewSource() - it is used when discovering devices so here we should return a new Device() or a child of Device();

4. Coordinator BuildCoordinator() - here we return our coordinator backend.

5. Network BuildNetwork() - here we return a network backend.

6. IDeviceSource BuildSourceFromJsonFile(string pathToFile) - this is particularly important because after detecting a folder with network saved in it (where name of folder is equal to name of vendorId) this function is called to analyze the content's of the folder and return it.

7. virtual Coordinator BuildCoordinatorFromJsonFile(string pathToFile) - works in the same fashion as BuildSourceFromJsonFile but for coordinator located in network directory.

A good practical example of implementation of DeviceFactory is VirtualDeviceFactory.

#### Frontend (NecBlik.[moduleName].GUI)

##### DeviceGuiFactory

While DeviceFactory provides backend this class provides basic gui controls, but also ViewModels. 

Remark!!! DeviceGuiFactory.internalFactoryId should be the same as DeviceFactory.internalFactoryId and should be returned in GetVendorId().

We have several important functions to override here:

1. string GetVendorID()

2. UIElement GetDeviceControl(DeviceViewModel deviceViewModel) - returning the UserControl that will be visible on the map.

3. async Task<NetworkViewModel> NetworkViewModelFromWizard(Network network) - this function may open a WizardWindow and wait till user configures network parameters.

4. void Initalize(object args = null) - this function is called when the factory is being loaded into application it should for example create a submodules folder if necessary.

Remark!!! Note that DeviceGuiFactory does not support submodules at all and does not care about FactoryRules.

If you want to add a submodules mechanism you need to use VirtualDeviceGuiFactory as base for your Frontend part of the module.

##### VirtualDeviceGuiFactory

Here in VirtualDeviceGuiFactory we have a few other functions that will interest us during child implementation:

1. GetAvailableControls(), GetAvailableNetworkViewModels(), GetAvailableDeviceViewModels() - those functions must return all detected controls/viewmodels in this and other submodules (if supported),

2. DeviceViewModelFromRule(DeviceModel model, NetworkViewModel network, FactoryRule rule) and DeviceViewModelFromRules(DeviceModel model, NetworkViewModel network, List<FactoryRule> rules) must return a specific class based on given rules.

3. GetTypesTInOtherSubAssemblies<T>() - this function scans submodule directories (can be used in child guifactories under condition that they were properly assigned a specific internal factory type).

#### Example

##### Backend

A good example of backend implementation is DigiZigBeeFactory.cs, it skips all the process of implementing submodule/rules functionalities because it uses VirtualGuiFactory.cs as base.

Except for basic functions that MUST be implemented:

```csharp
public DigiZigBeeFactory()
        {
            this.internalFactoryType = Resources.Resources.DigiFactoryId;
        }

        public override IDeviceSource BuildNewSource()
        {
            return base.BuildNewSource();
        }

        public override Coordinator BuildCoordinator()
        {
            return new DigiZigBeeUSBCoordinator(this);
        }
```

 It also implements the way the network is being recovered from a .json file.

```csharp
public override async Task<Network> BuildNetworkFromDirectory(string pathToDirectory, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider)
        {
            if (pathToDirectory.Split('.').LastOrDefault() != this.GetVendorID())
            {
                return null;
            }

            var path = Path.GetDirectoryName(pathToDirectory);
            var fileName = Path.GetFileName(pathToDirectory);
            var connectionData = JsonConvert.DeserializeObject<DigiZigBeeUSBCoordinator.DigiUSBConnectionData>(File.ReadAllText(pathToDirectory + "\\"+Resources.Resources.CoordinatorFile));
            var network = JsonConvert.DeserializeObject<DigiZigBeeNetwork>(File.ReadAllText(pathToDirectory + "\\"+Resources.Resources.NetworkFile));
            if (network == null)
            {
                return null;
            }

            foreach (var file in Directory.EnumerateFiles(pathToDirectory + "\\Sources"))
            {
                IDeviceSource source = this.BuildSourceFromJsonFile(file);
                if (source == null)
                {
                    foreach (var factory in this.OtherFactories)
                    {
                        source = factory.BuildSourceFromJsonFile(file);
                        if (source != null)
                            break;
                    }
                }
                if (source != null)
                {
                    network.AddSource(source);
                }
            }

            network.ProgressResponseProvider = updatableResponseProvider;
            await network.Initialize(new DigiZigBeeUSBCoordinator(this, connectionData));
            return network;
        }
```

The way network is saved in .json file is defined in Network class.

##### Frontend

Now, let's talk about frontend example, also from Digi module, because it is pretty simple.

Except from basic functions that must be overriden:

```csharp
public DigiZigBeeGuiFactory()
        {
            this.internalFactoryType = NecBlik.Digi.Resources.Resources.DigiFactoryId;
        }

public override NetworkViewModel GetNetworkViewModel(Network zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() != this.internalFactoryType)
                return null;
            return new DigiZigBeeNetworkViewModel(zigBeeNetwork);
        }
```

We implement dataTemplate functions for network

```cshtml
public override DataTemplate GetNetworkDataTemplate(Network zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() == this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/NecBlik.Digi.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["DigiNetworkDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }

public override DataTemplate GetNetworkBriefDataTemplate(Network zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() == this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/NecBlik.Digi.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["DigiNetworkBriefDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }
```

Then we implement calling the Wizard

```csharp
public override async Task<NetworkViewModel> NetworkViewModelFromWizard(Network zigBeeNetwork)
        {
            var vm = new DigiNetworkWizardViewModel();
            var rp = new DigiNetworkWizard(vm);
            return await rp.ProvideResponseAsync();
        }
```

And now the hardest part - the rules/submodules which one should just take from DigiZigBeeGuiFactory.cs and analyze on his own. 

Following functions must be overriden here:

1. public override VirtualNetworkViewModel NetworkViewModelBySubType(Network network, string subType)

2. public override VirtualDeviceViewModel DeviceViewModelFromRule(DeviceModel model, NetworkViewModel network, FactoryRule rule)

3. public override VirtualDeviceViewModel DeviceViewModelFromRules(DeviceModel model, NetworkViewModel network, List<FactoryRule> rules)

4. public override UIElement GetDeviceControl(DeviceViewModel deviceViewModel)

5. public override List<string> GetAvailableControls()

6. public override List<string> GetAvailableNetworkViewModels()

7. public override List<string> GetAvailableDeviceViewModels()

Because in parent class the assembly won't be able to detect submodule components in current assembly.

When implementing a module please just copy-paste code from 7 functions above and adjust it to your [FactoryClassName] of course instead of rebuilding the submodules/rules functionality you can simply type

```csharp
if (model.DeviceSource.GetVendorID() != this.GetVendorID())
                return base.DeviceViewModelFromRules(model, network, rules);
```

or similar code, depending on the function.







#### General remarks concerning modules extensions

All modules are being loaded by DeviceGuiAnyFactory and DeviceAnyFactory.

 

### General remarks concerning Xbee application layer adjustments for NecBlik

It is worth noting that NecBlik uses some default commands and sends them in application layer data. The only command that is not dependent on extension of NecBlik is "ECHO". If an "ECHO" is sent to a remote XBee, then the XBee must reply with everything what was sent after the "ECHO" or some functionalities may not work or NecBlik may wrongly interpret device statuses.














































