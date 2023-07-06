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
# Use the Jenkins agent base image
FROM jenkins/agent:latest

# Switch to root to install .NET Runtime
USER root

# Install required packages
RUN apt-get update \
    && apt-get install -y wget apt-transport-https

# Download the Microsoft package repository
RUN wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

# Install the Microsoft package repository
RUN dpkg -i packages-microsoft-prod.deb

# Update package lists
RUN apt-get update

# Install .NET Runtime
RUN apt-get install -y aspnetcore-runtime-7.0

# Switch back to the jenkins user
WORKDIR /Quali.Torque.Cli
COPY --from=build-env /Quali.Torque.Cli/out .

ENV PATH="/Quali.Torque.Cli:${PATH}"

USER jenkins
#docker run -d torque-cli sleep infinity
# docker image build -t torque-cli .

