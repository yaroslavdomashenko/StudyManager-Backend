FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["StudyManager/StudyManager.csproj", "StudyManager/"]
COPY ["StudyManager.Services/StudyManager.Services.csproj", "StudyManager.Services/"]
COPY ["StudyManager.Data/StudyManager.Data.csproj", "StudyManager.Data/"]
RUN dotnet restore "StudyManager/StudyManager.csproj"
COPY . .
WORKDIR "/src/StudyManager"
RUN dotnet build "StudyManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StudyManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudyManager.dll"]