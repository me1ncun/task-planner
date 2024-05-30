FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 5113

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["taskplanner-ui/taskplanner-ui.csproj", "taskplanner-ui/"]
RUN dotnet restore "taskplanner-ui/taskplanner-ui.csproj"
COPY . .
WORKDIR "/src/taskplanner-ui"
RUN dotnet build "taskplanner-ui.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "taskplanner-ui.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "taskplanner-ui.dll"]
