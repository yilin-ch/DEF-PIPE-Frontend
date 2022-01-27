<p align="center"><img width=50% src="https://raw.githubusercontent.com/DataCloud-project/toolbox/master/docs/img/datacloud_logo.png"></p>&nbsp;

[![GitHub Issues](https://img.shields.io/github/issues/DataCloud-project/DEF-PIPE-frontend.svg)](https://github.com/DataCloud-project/DEF-PIPE-frontend/issues)

# DEF-PIPE: Frontend

Graphic tool for designing data pipelines and tranforming them to DSL.

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

To the build the project, open the file *DataCloud.PipelineDesigner.sln* using Visual Studio for your platform press **`Ctrl+Shift+B` (Windows)** or **`⌘B, F6` (MacOS)** to build the entire solution.

Right-click on the project **DataCloud.PipelineDesigner.WebClient** and set it as the startup project, then press F5 to start the application.

## Working with the codebase
- The main part of the project is the client-side application. This is a standard ReactJS single-page-application. The source code for this client-side app is located at **DataCloud.PipelineDesigner.WebClient/ClientApp/src**.

- The backend web api is located at **DataCloud.PipelineDesigner.WebClient/Controllers**. From here, you can follow the reference to navigate to lower levels implementation.
