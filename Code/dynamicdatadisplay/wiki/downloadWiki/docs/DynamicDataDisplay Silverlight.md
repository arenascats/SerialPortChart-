# Silverlight version of WPF DynamicDataDisplay

## The following features are supported by Silverlight version:
* Numeric and DateTime Axes
* Line graphs
* Flexible data sources
* Setting custom legend position
* Setting custom navigation buttons location

## Major differences from WPF version of DynamicDataDisplay
* Use **TextBlock** objects in a xaml layout definition inside the **VerticalAxisTitle** and **HorizontalAxisTitle** instead of strings
* Use **VerticalAxisTitle**(**TextBlock** content) constructor to create a **VerticalAxisTitle** in C#
* Use **LineGraphSettings** and **ChartPlotterSettings** classes to perform custom configuration of **LineGraph** and **ChartPlotter** along with LineGraph(IPointDataSource pointSource, LineGraphSettings settings) and ChartPlotter(ChartPlotterSettings settings) constructors

## How to use DynamicDataDisplaySilverlight in your projects?
# Install Microsoft® Silverlight™ 2 or 3 Tools for Visual Studio 2008 SP1.
# Download latest release and extract **DynamicDataDisplaySilverlight.dll**
# Reference the library in your project
# Use _Microsoft.Research.DynamicDataDisplay_ namespace to access the library

## Links to Microsoft® Silverlight™ Tools for Visual Studio 2008 SP1:
Silverlight™ 2 Tools - [http://www.microsoft.com/downloads/details.aspx?familyid=C22D6A7B-546F-4407-8EF6-D60C8EE221ED&displaylang=en](http://www.microsoft.com/downloads/details.aspx?familyid=C22D6A7B-546F-4407-8EF6-D60C8EE221ED&displaylang=en)
Silverlight™ 3 Tools - [http://www.microsoft.com/downloads/details.aspx?familyid=9442b0f2-7465-417a-88f3-5e7b5409e9dd&displaylang=en](http://www.microsoft.com/downloads/details.aspx?familyid=9442b0f2-7465-417a-88f3-5e7b5409e9dd&displaylang=en)

## DynamicDataDisplay Silverlight controls in action:
### Animation Sample
{silverlight:url=http://home-server.redirectme.net:81/d3sl/AnimationSample.xap,height=600,width=800}

### Allergology sample
{silverlight:url=http://home-server.redirectme.net:81/d3sl/AllergologySample.xap,height=600,width=800}

### Line Graph Customization Sample
{silverlight:url=http://home-server.redirectme.net:81/d3sl/LineGraphCustomization.xap,height=600,width=1018}