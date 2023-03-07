# Quali Torque CLI

---

## Intro

Torque CLI is the command-line tool for interacting with [Torque](https://qtorque.io), the EaaS platform.
To learn more about Torque, visit [https://qtorque.io](https://qtorque.io).

## Installing

* If you have the previous version of torque-cli(from Pypi) installed on your system please remove it first with:

  `pip uninstall torque-cli`
* Right now there are two ways to use torque-cli:
  * install it as a dotnet tool. In this case you need to have **dotnet** >=7.0 installed.
    To install dotnet follow the link: https://dotnet.microsoft.com/en-us/download 

    `dotnet tool install -g torque-cli`
  * You also can run torque-cli as a docker container:
  
    `docker run -it qtorque/torque-cli:latest`

    `docker run -it qtorque/torque-cli:latest -v ~/.torque:/root/.torque/ # to mount your local torque config file`
* old configuration file will not work, so you will have to re-create it with `torque config set` command

## Configuration

In order to allow the CLI tool to authenticate with Torque you must provide several parameters:
* *Token* The easiest way to generate a token is via the Torque UI. Navigate to *Settings (in your space) -> Integrations ->
  click “Connect” under any of the CI tools -> click “New Token”* to get an API token.
* *Space* The space in the Torque to use
* *Repository* (optional) represents the name of Blueprint repository that will be used in a context of each call to Torque server

The *Token*, *Space* and *Repository* parameters can be provided via special command line flags (*--token*, *--space*,  
and *--repo* respectively) but can be conveniently placed in a config file relative to your user folder,
so they don't need to be provided each time.

The config file can be easily created and managed using the interactive `torque config` command.
The CLI supports multiple profiles, and you can switch between them by setting up the active for ease of use. To use a non-active profile the _--profile_ command line flag needs to be used to specify the profile name.

To add a new profile or update an existing one run ```torque config set``` and follow the on-screen directions.
To see all profiles run ```torque config list``` and the command will output a table of all the profiles that are currently configured. Example output:

```bash
$ torque config list
                     Torque user profiles

  Active │ Profile Name │  Space   │ Repository │   Token
 ────────┼──────────────┼──────────┼────────────┼────────────
    +    │     demo     │  sample  │            │ ******r-aI
         │     test     │   dev    │   myrepo   │ ******masd
 ```

If a profile is no longer needed it can be easily removed by running ```torque config remove <profile-name>```

The `torque config` command will save the config file relative to your home user directory ('~/.torque/config.yml' on Mac and Linux or in '%UserProfile%\\.torque\\config.yaml' on Windows).
If you wish to place the config file in a different location, you can specify that location via an environment variable:

`$ export TORQUE_CONFIG_PATH=/path/to/file`


The different parameters may also be provided as environment variables instead of using the config file:

```bash
export TORQUE_TOKEN = xxxzzzyyy
export TORQUE_SPACE = demo_space
# Optional
export TORQUE_ACCOUNT = MYACCOUNT
```

The different parameters may also be provided as environment variables instead of using the config file:

```bash
export TORQUE_TOKEN = xxxzzzyyy
export TORQUE_SPACE = demo_space
# Optional
export TORQUE_REPO_NAME = my_repo
```

### Additional environment variables

It is possible to switch the client to different Torque instance setting custom API endpoint:

```bash
export TORQUE_URL = "https://demo.qtorque.io"
```

## Basic Usage

Torque CLI r

There are some basic actions Torque CLI currently allows you to perform:

- Validate a Blueprint (using the `torque bp validate` command)
- Get a list of blueprints (via `torque bp list`)
- Start an Environment (via `torque env start`)

In order to get help run:

`$ torque --help`

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

You can get additional help information for a particular command by specifying *--help* flag after command name, like:

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
