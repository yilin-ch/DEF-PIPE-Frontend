<p align="center"><img width=50% src="https://raw.githubusercontent.com/DataCloud-project/toolbox/master/docs/img/datacloud_logo.png"></p>&nbsp;

[![GitHub Issues](https://img.shields.io/github/issues/DataCloud-project/DEF-PIPE-frontend.svg)](https://github.com/DataCloud-project/DEF-PIPE-frontend/issues)
[![License](https://img.shields.io/badge/license-Apache2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

# DEF-PIPE Frontend
The pipeline designer is deployed and accessible at https://pipelinedesign.azurewebsites.net/
Graphic tool for designing data pipelines and tranforming them to DSL. Because of several benefits such as ease of use, compability, and reusability, the web application approach was chosen to implement a pipeline designer. It consists of the following components:

## Front-End
- The main part of the application is the interface for designing big data pipelines.This interface is implemented as a single page application using ReactJS. the popularity and stability of ReactJS make it potentially more friendly with developers to continue with the project later on.
The project also use Bootstrap, a framework providing basic UI components building blocks which are easy to customize.

## Back-End
- The back-end is implemented in CSharp using Dor Net (.NET) framework from Microsoft. In particular, ASP.NET Core, which is the part of the >NET framework for web application, is being used.

## Database
- The database in Pipeline Designer is mainly used to persist pipelines and templates created by users. As such, there was no need for any kind of complex query capabilities. Based on this assesment Entity Framework which is an object-relational mapping (ORM) framework for .NET is used.
Graphic tool for designing data pipelines and tranforming them to DSL.

# Installation

## Toolings

For both Windows and MacOS, install the following:
- [NodeJS 14.16.0](https://nodejs.org/de/blog/release/v14.16.0/)

### Windows

- Download and install [Visual Studio 2022](https://visualstudio.microsoft.com/vs/). The free Community edition can be used in case a Visual Studio license is not available.
- Go to [.NET SDK Download](https://dotnet.microsoft.com/en-us/download/dotnet/5.0) and download & install the SDK 5.0.100 for Windows.
- Go to [.NET Download](https://dotnet.microsoft.com/en-us/download/dotnet/5.0/runtime) and download & install the Hosting Bundle for Windows.

### MacOS

- Download and install [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/).
- Go to [.NET SDK Download](https://dotnet.microsoft.com/en-us/download/dotnet/5.0) and download & install the SDK 5.0.100 for MacOS.
- Go to [.NET Download](https://dotnet.microsoft.com/en-us/download/dotnet/5.0/runtime) and download & install the Hosting Bundle for MacOS.

## Build

- To the build the project, open the file *DataCloud.PipelineDesigner.sln* using Visual Studio for your platform press **`Ctrl+Shift+B` (Windows)** or **`⌘B, F6` (MacOS)** to build the entire solution.
Right-click on the project **DataCloud.PipelineDesigner.WebClient** and set it as the startup project, then press F5 to start the application.

# Usage

## Main Components

The main components are:
- Pipeline Designer: Main component for designing data pipelines. It consists of three sub- components: Canvas Pane, Palette Pane, and Property Pane.
- Template Designer: Main component for designing templates. It consists of three sub-components: Canvas Pane, Palette Pane, and Property Pane.
- Canvas Pane: A shared component to be reused in both Pipeline Designer and Template Designer. This component contains all the logic for rendering visualization of the pipeline.
- Palatte Pane: A shared component to be reused in both Pipeline Designer and Template Designer. This component allows users to intertact with the library of available templates.
- Property Pane: A shared component to be reused in both Pipeline Designer and Template Designer. This component manages the properties of the each element in the pipeline.

## Working with the codebase

- The codebase consists of other 6 projects. Except from the project **DataCloud.PipelineDesigner.WebClient** , the other projects are Class Library type. The output of each Class Library project will be a compiled DLL which can be re-used by other systems.

- The main part of the project is the client-side application. This is a standard ReactJS single-page-application. The source code for this client-side app is located at **DataCloud.PipelineDesigner.WebClient/ClientApp/src**.

- The backend Web API is located at **DataCloud.PipelineDesigner.WebClient/Controllers**. From here, you can follow the reference to navigate to lower levels implementation.

- A shared component to be reused in both Pipeline Designer and Template Designer is located at **DataCloud.PipelineDesigner.CanvasModel**. This component contains all the logic for rendering visualization of the pipeline.

- The data access layer including the Entity Framework code and interface for data access is located at **DataCloud.PipelineDesigner.Repositories**. The abstraction of data storage technology allowing the system to switch from one type of storage to another without the need to change other upper layers of the code.

- The business logic layer containing the interface and implementation of the Workflow transformer and DSL transformer components is located at **DataCloud.PipelineDesigner.Services**. This is the main component for designing pipelines and it consists of three sub- components: Canvas Pane, Palette Pane, and Property Pane.

- **DataCloud.PipelineDesigner.Core** is a class library to hold all code that needs to be shared between all layers.
