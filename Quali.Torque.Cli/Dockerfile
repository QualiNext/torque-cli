FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
LABEL org.opencontainers.image.source="https://github.com/QualiNext/torque-cli"
WORKDIR /Quali.Torque.Cli

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
LABEL org.opencontainers.image.source="https://github.com/QualiNext/torque-cli"
WORKDIR /Quali.Torque.Cli
COPY --from=build-env /Quali.Torque.Cli/out .
ENTRYPOINT ["dotnet", "torque-cli.dll"]
