# DataCloud.PipelineDesigner

- Graphic tool for designing data pipelines and tranforming them to DSL.

## Web Application
- Because of several benefits such as ease of use, compability, and reusability, the web application approach was chosen to implement Pipeline Designer.

## Front-End
- The main part of the application is the interface for designing big data pipelines.This interface is implemented as a single page application using ReactJS. the popularity and stability of ReactJS make it potentially more friendly with developers to continue with the project later on.
The project also use Bootstrap, a framework providing basic UI components building blocks which are easy to customize.

## Back-End
- The back-end is implemented in CSharp using Dor Net (.NET) framework from Microsoft. In particular, ASP.NET Core, which is the part of the >NET framework for web application, is being used.

## Database
- The database in Pipeline Designer is mainly used to persist pipelines and templates created by users. As such, there was no need for any kind of complex query capabilities. Based on this assesment Entity Framework which is an object-relational mapping (ORM) framework for .NET is used.

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

## Working with the codebase
- The codebase consists of other 6 projects. except from the project **DataCloud.PipelineDesigner.WebClient** , the orher projects are Class Library type. The output of each Class Library project will be a compiled DLL which can be re-used by other systems.

- The main part of the project is the client-side application. This is a standard ReactJS single-page-application. The source code for this client-side app is located at **DataCloud.PipelineDesigner.WebClient/ClientApp/src**.

- The backend web api is located at **DataCloud.PipelineDesigner.WebClient/Controllers**. From here, you can follow the reference to navigate to lower levels implementation.

- The classes and data structure in the Canvas Model is located at **DataCloud.PipelineDesigner.CanvasModel**.

- The data access layer including the Entity Framework code and interface for data access is located at **DataCloud.PipelineDesigner.Repositories**. The abstraction of data storage technology allowing the system to switch from one type of storage to another without the need to change other upper layers of the code.

- The business logic layer containing the interface and implementation of the Workflow Transformer and DSL Transformer components is located at **DataCloud.PipelineDesigner.Services**.

- **DataCloud.PipelineDesigner.Core** is a class library to hold all code that needs to be shared between all layers.
