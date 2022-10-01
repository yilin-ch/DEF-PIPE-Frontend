<p align="center"><img width=50% src="https://raw.githubusercontent.com/DataCloud-project/toolbox/master/docs/img/datacloud_logo.png"></p>&nbsp;

[![GitHub Issues](https://img.shields.io/github/issues/DataCloud-project/DEF-PIPE-frontend.svg)](https://github.com/DataCloud-project/DEF-PIPE-frontend/issues)
[![License](https://img.shields.io/badge/license-Apache2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

# DEF-PIPE Frontend

The DEF-PIPE Frontend is a graphic pipeline designer tool for defining Big Data pipelines and tranforming them to DSL. The pipeline designer test version is deployed and accessible at https://crowdserv.sys.kth.se

Because of several benefits such as ease of use, compability, and reusability, the web application approach was chosen to implement a pipeline designer.

## Front-End
- The main part of the application is the interface for designing big data pipelines. This interface is implemented as a single page application using ReactJS. The popularity and stability of ReactJS make it potentially more friendly with developers to continue with the project later on.
The project also use Bootstrap, a framework providing basic UI components building blocks which are easy to customize.

## Back-End
- The back-end is implemented in CSharp using Dot Net (.NET) framework from Microsoft. In particular, ASP.NET Core, which is the part of the NET framework for web application, is being used. It implements a web API providing a central interface for operations such as managing pipelines and templates data, transforming pipelines into DSL.

## Database
- The database in Pipeline Designer is used to persist steps and workflow created by users. As the visual workflows are represented in JSON format, MongoDB is used.

# Deployment

The solution is configured to work in a docker container, the build configuration is located in the [Dockerfile](./Dockerfile) and a [docker-compose.yml](./docker-compose.yml) is available at the root of the repository.

After setting the environement variables required bellow, run `docker-compose up` to build the project and start the container. The application will be reachable on the port 80. 

ℹ️ *An official docker image will be soon published on docker hub*

For infomation how to configure or use docker, see the [official documentation](https://docs.docker.com/).

## Environment

### Docker

In the [docker-compose.yml](./docker-compose.yml), change the following variables

| Variable | Description |
|---|---|
|MANGO_CONNECTION_STRING| Connection string for MongoDB |
|KEYCLOAK_AUTHORITY| Url of the KeyCloak authorization server  |

### ReactJS

For the frontend, you need to add a `.env` file in [DataCloud.PipelineDesigner.WebClient/ClientApp](./DataCloud.PipelineDesigner.WebClient/ClientApp) and set the following variables:

| Variable | Description |
|---|---|
|REACT_APP_KEYCLOAK_URL| KeyCloak base url |
|REACT_APP_KEYCLOAK_REALM| KeyCloak realm  |
|REACT_APP_KEYCLOAK_CLIENT_ID| KeyCloak client id |

A [.env.example](DataCloud.PipelineDesigner.WebClient/ClientApp/.env.example) file is available for example.

# C# Environment

## Toolings

For both Windows and MacOS, install the following:
- [NodeJS 16.15.0](https://nodejs.org/de/blog/release/v16.15.0/)

### Windows

- Download and install [Visual Studio 2022](https://visualstudio.microsoft.com/vs/). The free Community edition can be used in case a Visual Studio license is not available.
- Go to [.NET SDK Download](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and download & install the SDK 6.0.1 for Windows.
- Go to [.NET Download](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime) and download & install the Hosting Bundle for Windows.

### MacOS

- Download and install [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/).
- Go to [.NET SDK Download](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and download & install the SDK 6.0.1 for MacOS.
- Go to [.NET Download](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime) and download & install the Hosting Bundle for MacOS.

## Build

- To the build the project, open the file *DataCloud.PipelineDesigner.sln* using Visual Studio for your platform press **`Ctrl+Shift+B` (Windows)** or **`⌘B, F6` (MacOS)** to build the entire solution.
Right-click on the project **DataCloud.PipelineDesigner.WebClient** and set it as the startup project, then press F5 to start the application.

# Usage

## Main Components

The main components are:
- Pipeline Designer: Main component for designing data pipelines. It consists of three sub- components: Canvas Pane, Palette Pane, and Property Pane.
- Template Designer: Main component for designing templates. It consists of three sub-components: Canvas Pane, Palette Pane, and Property Pane.
- Canvas Pane: A shared component to be reused in both Pipeline Designer and Template Designer. This component contains all the logic for rendering visualization of the pipeline.
- Palette Pane: A shared component to be reused in both Pipeline Designer and Template Designer. This component allows users to interact with the library of available templates.
- Property Pane: A shared component to be reused in both Pipeline Designer and Template Designer. This component manages the properties of each element in the pipeline.

## Working with the codebase

- The codebase consists of other 6 projects. Except from the project **DataCloud.PipelineDesigner.WebClient** , the other projects are Class Library type. The output of each Class Library project will be a compiled DLL which can be re-used by other systems.

- The main part of the project is the client-side application. This is a standard ReactJS single-page-application. The source code for this client-side app is located at **DataCloud.PipelineDesigner.WebClient/ClientApp/src**.

- The backend Web API is located at **DataCloud.PipelineDesigner.WebClient/Controllers**. From here, you can follow the reference to navigate to lower levels implementation.

- A shared component to be reused in both Pipeline Designer and Template Designer is located at **DataCloud.PipelineDesigner.CanvasModel**. This component contains all the logic for rendering visualization of the pipeline.

- The data access layer including the Entity Framework code and interface for data access is located at **DataCloud.PipelineDesigner.Repositories**. The abstraction of data storage technology allowing the system to switch from one type of storage to another without the need to change other upper layers of the code.

- The business logic layer containing the interface and implementation of the Workflow transformer and DSL transformer components is located at **DataCloud.PipelineDesigner.Services**. This is the main component for designing pipelines and it consists of three sub- components: Canvas Pane, Palette Pane, and Property Pane.

- **DataCloud.PipelineDesigner.Core** is a class library to hold all code that needs to be shared between all layers.
