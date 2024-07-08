# Quali Torque CLI

---

## Intro

Torque CLI is a command-line tool for interacting with [Torque](https://www.quali.com/torque/), Quali's EaaS platform.
To learn more about Torque, visit [https://www.quali.com/torque/](https://www.quali.com/torque/).

## Installing

* If you have the previous version of torque-cli(from Pypi) installed on your system please remove it first with:

  `pip uninstall torque-cli`
* Right now there are two ways to use torque-cli:
  * Install torque-cli as a .NET tool. In this case you need to have **dotnet** (version 7.0 or higher) installed.
    To install dotnet, follow the link: https://dotnet.microsoft.com/en-us/download 

    ```dotnet tool install -g torque --version latest --add-source https://nuget.pkg.github.com/QualiNext/index.json```
  * Run torque-cli as a docker container:
  
    ```docker run -it qtorque/torque-cli:latest```

    ```docker run -it qtorque/torque-cli:latest -v ~/.torque:/root/.torque/ # to mount your local torque config file```
* The old configuration file will not work, so you will have to re-create it with command: ```torque config set```

## Configuration

To allow the torque-cli to authenticate with Torque, you must provide several parameters:
* *Space*: The Torque space to use
* *Repository*: (Optional) Represents the name of the git repository containing the bleprints and IaC files that will be used when calling Torque
* *Token*: The easiest way to generate a token is via the Torque UI. 
   1. In your space, go to **Settings > Integrations**.
   2. Click **Connect** under any of the CI tools.
   3. Click **New Token** to get an API token.

The ```Token```, ```Space``` and ```Repository``` parameters can be provided via a special command line flags (```--token```, ```--space```, and ```--repo```, respectively). You can also conveniently place these parameters in a config file relative to your user folder, so they don't need to be provided each time.

The config file can be created and managed using the interactive `torque config` command.
The CLI supports multiple profiles, and you can switch between them by setting the active profile for ease of use. To use a non-active profile, the ```--profile_``` command-line flag needs to be used to specify the profile name.

To add a new profile or update an existing one, run ```torque config set``` and follow the on-screen directions.
To see all profiles, run ```torque config list``` and the command will output a table of all the profiles that are currently configured. 

Example output:

```bash
$ torque config list
                     Torque user profiles

  Active │ Profile Name │  Space   │ Repository │   Token
 ────────┼──────────────┼──────────┼────────────┼────────────
    +    │     demo     │  sample  │            │ ******r-aI
         │     test     │   dev    │   myrepo   │ ******masd
 ```

If a profile is no longer needed, you can remove it by running: ```torque config remove <profile-name>```

The ```torque config``` command saves the config file relative to your home user directory ('~/.torque/config.yml' on Mac and Linux or in the '%UserProfile%\\.torque\\config.yaml' file on Windows).
To place the config file in a different location, specify that location via an environment variable:

```$ export TORQUE_CONFIG_PATH=/path/to/file```

You can also provide the different parameters as environment variables instead of using the config file:

```bash
export TORQUE_TOKEN = xxxzzzyyy
export TORQUE_SPACE = demo_space
# Optional
export TORQUE_REPO_NAME = my_repo
```

### Additional environment variables

It is possible to switch the client to a different Torque instance using a custom API endpoint:

```bash
export TORQUE_URL = "https://demo.qtorque.io"
```

## Basic Usage

There are some basic actions Torque CLI currently allows you to perform:

- Validate a blueprint (using the ```torque bp validate``` command)
- Get a list of blueprints (via ```torque bp list```)
- Start an environment (via ```torque env start```)

To see the help files, run:

```$ torque --help```

It will give you detailed output with usage:

```shell
$ torque -h
USAGE:
torque [OPTIONS] <COMMAND>

EXAMPLES:
torque bp get MyBp
torque bp list
torque env start demo --duration=100 --name=MyDemoEnv
torque env get
torque env end qwdj4jr9smf

OPTIONS:
-h, --help       Prints help information
-v, --version    Prints version information

COMMANDS:
blueprint      Get, List, Validate blueprints
environment    Start, End, View Torque environments
config         List, Add and Modify user profiles
agent          List, associate agents
space          Create, delete spaces, connect repo to space
```

You can get additional help information for a particular command by including  the ```--help``` flag after the command name, like:

```shell
$ torque env -h
DESCRIPTION:
Start, End, View Torque environments.

USAGE:
    torque environment [OPTIONS] <COMMAND>

EXAMPLES:
    torque env start demo --duration=100 --name=MyDemoEnv
    torque env get
    torque env end qwdj4jr9smf
    torque env list --show-ended
    torque env extend qwdj4jr9smf --duration 120

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    start <BLUEPRINT-NAME>     Start Environment
    get <ENVIRONMENT-ID>       Get Environment Details
    end <ENVIRONMENT-ID>       End Torque Environment
    list                       List Torque Environment
    extend <ENVIRONMENT-ID>    Extend Torque Environment
```

## Bulk deploy

The CLI allows for deplying multiple environments defined in a CSV file.

```shell
torque.exe env bulkstart <PATH-TO-CSV>
```
The lines of the CSV are itterated, starting an environment for each line. If a line specifies multiple owners, a separate environment (with the same parameters) will be started for each owner.
#### CSV format
```csv
Space,Blueprint,Repository,Duration (Minutes),Owners,Inputs
space_name,blueprint_name,blueprint_repo_name,duration_integer,owner_email,input_name1:input_value1;input_name2:input_value2
```
* The first line is assumed to contain headers and is skipped.
* The 'Owners' field may contain a list of emails separated with ; or a single value. an environment is createdf per owner.
* Inputs - A list of inputs matching the blueprint. the format is input_name1:input_value1;input_name2:input_value2