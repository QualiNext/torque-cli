Publish to Linix

```shell
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -p:DebugType=None -p:DebugSymbols=false  -o  /c/Work/dotcli/Quali.Torque.Cli/bin/publish/linux-x64

```

## Config Commands

```shell
config set -p default -t 5mH-w3Jli2QvPMIMYPkVyjH76AoV0kWU8uZu5YqUgAU -s demo -r ProductionBPs
config activate default
```

## Push to Github nuget feed

```shell
dotnet nuget push torque.2.0.3.nupkg --source https://nuget.pkg.github.com/QualiNext/index.json --api-key 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

```