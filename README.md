# Two.Factor.Cli

Two.Factor.Cli is a cross-platform command-line tool for managing two-factor authentication (2FA) secrets and generating TOTP codes from the terminal.

The tool is designed for developers and power users who prefer a fast, scriptable, and local-first workflow for working with 2FA entries. It supports adding, listing, and generating one-time passwords directly from the command line.

## Features

- Store and manage TOTP entries locally
- Generate time-based one-time passwords from the terminal
- List saved 2FA entries
- Add new entries through a simple CLI interface
- Designed to be distributed and installed as a .NET global tool

## Installation

The recommended way to install Two.Factor.Cli is via [dotnet tool][1]:
```ps
dotnet tool install --global 2fa
```

## Local Installation
Run
```ps
dotnet pack
```
in Solution folder

```ps
dotnet tool install -g Two.Factor.Cli --add-source %path/to/nupkg/file/created/in/previous/step%
```
Ommit `-g` if you don`t want to install tool globally

## Usage

After installation, the tool can be used from the command line:
```ps
2fa --help
```
## Requirements

- .NET 10 SDK or later

## Notes

This project is intended to be packaged and distributed as a .NET tool, making installation and updates straightforward for users who work with the .NET ecosystem.

[1]: https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools-how-to-create
