Publish to Linix

```shell
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -p:DebugType=None -p:DebugSymbols=false  -o  /c/Work/dotcli/Quali.Torque.Cli/bin/publish/linux-x64

```

## Config Commands

```shell
config set -p default -t 5mH-w3Jli2QvPMIMYPkVyjH76AoV0kWU8uZu5YqUgAU -s demo -r ProductionBPs
config activate default
```