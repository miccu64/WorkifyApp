FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

COPY /Workify.Api.Stats /Workify.Api.Stats
COPY /Workify.Utils /Workify.Utils

WORKDIR /Workify.Api.Stats

RUN dotnet restore
RUN dotnet publish --property:PublishDir=out /p:$Workify__EnvironmentName

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /Workify.Api.Stats
COPY --from=build /Workify.Api.Stats/out .
ENTRYPOINT ["dotnet", "Workify.Api.Stats.dll"]