# bunny-deploy
Deploy static files to BunnyCDN Storage Zones

## Usage

```
    bunnydeploy [command] [options]

    Options:
    --storagezone <storagezone>  The Zone containing static files.
    --storagekey <storagekey>    Your BunnyNet Storage Zone Password.
    --accesskey <accesskey>      --accesskey
    --version                    Show version information
    -?, -h, --help               Show help and usage information

    Commands:
    purge                   Purge a BunnyNet Storage Zone of all static files.
    deploy <--contentpath>  Deploy new static files to a BunnyNet Storage Zone.
```

### Building for windows

- Add a package management config:

    dotnet new nugetconfig

- Add these lines to it:
```
    <add key="dotnet-experimental" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-experimental/nuget/v3/index.json" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
```
- Run this:

    dotnet add package Microsoft.DotNet.ILCompiler -v 7.0.0-*

- Make the build:

    dotnet publish -r win-x64 -c release

- The build will be under the `bin` folder somewhere.


### Resources

[API Reference](https://docs.bunny.net/reference/bunnynet-api-overview)
