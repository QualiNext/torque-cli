# The version is pinned because of a bug (https://github.com/dotnet/sdk/issues/28971#issuecomment-1309328718)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

LABEL org.opencontainers.image.source="https://github.com/QualiNext/torque-cli"
WORKDIR /Quali.Torque.Cli

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# RUN dotnet restore "./Quali.Torque.Cli/Quali.Torque.Cli.csproj"
# Build and publish a release
RUN dotnet publish -c Release -o out
# RUN dotnet publish "./Quali.Torque.Cli/Quali.Torque.Cli.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:9.0
LABEL org.opencontainers.image.source="https://github.com/QualiNext/torque-cli"
WORKDIR /Quali.Torque.Cli
COPY --from=build-env /Quali.Torque.Cli/out .

ENV PATH="/Quali.Torque.Cli:${PATH}"

# ENTRYPOINT ["dotnet", "torque-cli.dll"]
