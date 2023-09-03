FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs
WORKDIR /src
COPY ["DataCloud.PipelineDesigner.WebClient/DataCloud.PipelineDesigner.WebClient.csproj", "DataCloud.PipelineDesigner.WebClient/"]
COPY ["DataCloud.PipelineDesigner.CanvasModel/DataCloud.PipelineDesigner.CanvasModel.csproj", "DataCloud.PipelineDesigner.CanvasModel/"]
COPY ["DataCloud.PipelineDesigner.WorkflowModel/DataCloud.PipelineDesigner.WorkflowModel.csproj", "DataCloud.PipelineDesigner.WorkflowModel/"]
COPY ["DataCloud.PipelineDesigner.Services/DataCloud.PipelineDesigner.Services.csproj", "DataCloud.PipelineDesigner.Services/"]
RUN dotnet restore "DataCloud.PipelineDesigner.WebClient/DataCloud.PipelineDesigner.WebClient.csproj"
COPY . .
WORKDIR "/src/DataCloud.PipelineDesigner.WebClient"
RUN dotnet build "DataCloud.PipelineDesigner.WebClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataCloud.PipelineDesigner.WebClient.csproj"" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataCloud.PipelineDesigner.WebClient.dll"]
