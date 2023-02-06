# torque-cli

## How to install from github packages

### Prerequisites

- Login to github
- go to account **Settings**
- Go to **Developer settings**
- Generate a classic personal access token with permissions to read packages and save it somewhere

### Install as a dotnet tool

```bash
dotnet nuget add source "https://nuget.pkg.github.com/QualiNext/index.json" --name "github" --username <YOUR_GH_USERNAME> --password <GH_TOKEN> --store-password-in-clear-text
dotnet tool install torque -g
torque -h
```

### Run as a docker image

```bash
# login to github container registry
export CR_PAT=<GH_TOKEN>
echo $CR_PAT | docker login ghcr.io -u <GH_USERNAME> --password-stdin
# run torque
docker run ghcr.io/qualinext/torque-cli:1.0.0 -h
```
