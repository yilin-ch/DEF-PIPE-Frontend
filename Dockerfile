
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# Install NodeJs
RUN apt-get update && \
apt-get install -y wget && \
apt-get install -y gnupg2 && \
wget -qO- https://deb.nodesource.com/setup_16.x | bash - && \
apt-get install -y build-essential nodejs
# End Install
WORKDIR /source

ENV CONNECTION_STRING=NULL
# Copy csproj and restore as distinct layers
COPY /DataCloud.PipelineDesigner.WebClient/DataCloud.PipelineDesigner.WebClient.csproj ./ 
RUN dotnet restore

# copy everything else and build app
COPY . ./
WORKDIR /source
RUN dotnet publish DataCloud.PipelineDesigner.sln -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app/out
COPY --from=build /app ./
CMD ["dotnet", "DataCloud.PipelineDesigner.WebClient.dll"]