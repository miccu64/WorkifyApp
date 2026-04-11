FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

COPY /Workify.Api.ExerciseStat /Workify.Api.ExerciseStat
COPY /Workify.Utils /Workify.Utils

WORKDIR /Workify.Api.ExerciseStat

RUN dotnet restore
RUN dotnet publish --property:PublishDir=out /p:$Workify__EnvironmentName

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /Workify.Api.ExerciseStat
COPY --from=build /Workify.Api.ExerciseStat/out .
ENTRYPOINT ["dotnet", "Workify.Api.ExerciseStat.dll"]
