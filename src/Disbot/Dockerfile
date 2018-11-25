FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/Disbot/Disbot.csproj", "Disbot/"]
RUN dotnet restore "Disbot/Disbot.csproj"
COPY . .
WORKDIR "/src/Disbot"
RUN dotnet build "Disbot.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Disbot.csproj" -c Release -o /app -r linux-x64

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Disbot.dll"]