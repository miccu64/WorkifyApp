FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

COPY /Workify.Api.Gateway /Workify.Api.Gateway

WORKDIR /Workify.Api.Gateway

RUN dotnet restore
RUN dotnet publish --property:PublishDir=out /p:$Workify__EnvironmentName

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /Workify.Api.Gateway
COPY --from=build /Workify.Api.Gateway/out .
ENTRYPOINT ["dotnet", "Workify.Api.Gateway.dll"]