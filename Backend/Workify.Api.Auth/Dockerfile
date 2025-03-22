FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

COPY /Workify.Api.Auth /Workify.Api.Auth
COPY /Workify.Utils /Workify.Utils

WORKDIR /Workify.Api.Auth

RUN dotnet restore
RUN dotnet publish --property:PublishDir=out /p:$Workify__EnvironmentName

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /Workify.Api.Auth
COPY --from=build /Workify.Api.Auth/out .
ENTRYPOINT ["dotnet", "Workify.Api.Auth.dll"]