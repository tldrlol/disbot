FROM microsoft/dotnet:2.1-sdk as build
WORKDIR /app
 
COPY src/Disbot/Disbot.csproj ./
RUN dotnet restore
 
COPY . ./
RUN dotnet publish -c Release -o out -r linux-x64
 
FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Disbot.dll"]