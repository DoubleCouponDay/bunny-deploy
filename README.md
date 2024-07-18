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
