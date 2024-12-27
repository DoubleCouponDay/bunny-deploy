namespace bunnydeploy;

using System;
using System.CommandLine;

public static class CommandNames {
    public const string Purge = "purge";
    public const string Deploy = "deploy";
}

public static class CommandParams {
    public const string PullZoneId = "--pullzoneid";
    public const string AccessKey = "--accesskey";
    public const string StorageZone = "--storagezone";
    public const string StorageKey = "--storagekey";
    public const string ContentPath = "--contentpath";
    public const string Help = "--help";
}

public static class CommandHelp {
    public const string Purge = "Purge a BunnyNet Storage Zone of all static files.";
    public const string Deploy = "Deploy new static files to a BunnyNet Storage Zone.";
    public const string StorageZone = "The Zone containing static files.";
    public const string AccessKey = "Your BunnyNet API Key";
    public const string StorageKey = "Your BunnyNet Storage Zone Password.";
    public const string ContentPath = "The relative path to a folder contain static files for a website.";
}
