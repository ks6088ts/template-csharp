FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /App

COPY . ./

RUN dotnet restore \
    && dotnet publish src/TemplateCsharp.Cli/TemplateCsharp.Cli.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "TemplateCsharp.Cli.dll"]
