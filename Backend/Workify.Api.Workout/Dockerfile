FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

COPY /Workify.Api.Workout /Workify.Api.Workout
COPY /Workify.Utils /Workify.Utils

WORKDIR /Workify.Api.Workout

RUN dotnet restore
RUN dotnet publish --property:PublishDir=out /p:$Workify__EnvironmentName

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /Workify.Api.Workout
COPY --from=build /Workify.Api.Workout/out .
ENTRYPOINT ["dotnet", "Workify.Api.Workout.dll"]