// See https://aka.ms/new-console-template for more information

namespace cdndeploy;

using BunnyCDN.Net.Storage;
using System;
using System.Threading.Tasks;
using System.CommandLine;
using bunnydeploy;
using BunnyCDN.Net.Storage.Models;
using System.Net.Http.Json;
using System.Net;

public class CdnDeploy {

    public static async Task Main(string[] argv) {
        //get arguments
        var storagezone = new Option<string>(
            CommandParams.StorageZone,
            CommandHelp.StorageZone
        );

        var storagekey = new Option<string>(
            CommandParams.StorageKey,
            CommandHelp.StorageKey
        );

        var contentpath = new Argument<string>(
            CommandParams.ContentPath,
            CommandHelp.ContentPath
        );

        var accesskey = new Option<string>(
            CommandParams.AccessKey,
            CommandParams.AccessKey
        );

        var pullZoneId = new Option<string>(
            CommandParams.PullZoneId,
            CommandParams.PullZoneId
        );

        //default command
        var rootcommand = new RootCommand();
        rootcommand.AddGlobalOption(storagezone);
        rootcommand.AddGlobalOption(storagekey);
        rootcommand.AddGlobalOption(accesskey);
        rootcommand.AddGlobalOption(pullZoneId);

        //purge command
        var purgecommand = new Command(CommandNames.Purge, CommandHelp.Purge);
        rootcommand.AddCommand(purgecommand);

        purgecommand.SetHandler(PurgeCDNAsync, accesskey, pullZoneId);

        //deploy command
        var deploycommand = new Command(CommandNames.Deploy, CommandHelp.Deploy);
        rootcommand.AddCommand(deploycommand);
        deploycommand.AddArgument(contentpath);

        deploycommand.SetHandler(async (string storagekey, string storagezone, string contentpath) => {
            var context = GetContext(storagezone, storagekey);
            await DeployContentAsync(context, storagezone, contentpath);
        }, storagekey, storagezone, contentpath);

        if(argv.Length == 0) {
            await rootcommand.InvokeAsync(CommandParams.Help);
        }

        else {
            await rootcommand.InvokeAsync(argv);
        }
    }

    private static BunnyCDNStorage GetContext(string storagezone, string accesskey) {
        var context = new BunnyCDNStorage(storagezone, accesskey, string.Empty);
        return context;
    }

    private static async Task PurgeCDNAsync(string accesskey, string zoneId) {
        using var http = new HttpClient();

        {
            Console.WriteLine("Purging url...");

            var message = new HttpRequestMessage(HttpMethod.Post, "https://api.bunny.net/purge?async=false&url=https%3A%2F%2Fwww.fluffydice.cloud")
            {
                Content = new StringContent(string.Empty),
            };
            message.Headers.Add("AccessKey", accesskey);
            using var response = await http.SendAsync(message);

            Console.Write($"Response: {await response.Content.ReadAsStringAsync()}");

            if(response.StatusCode == HttpStatusCode.OK) {
                Console.WriteLine("200 OK");
                Console.WriteLine("URL purged.");
            }

            else {
                throw new Exception($"failed to purge url: {response.StatusCode}, {response.ReasonPhrase}");
            }
        }

        {
            Console.WriteLine($"Purging Pull Zone '{zoneId}'...");
            var message = new HttpRequestMessage(HttpMethod.Post, $"https://api.bunny.net/pullzone/{zoneId}/purgeCache")
            {
                Content = new StringContent(string.Empty)
            };
            message.Headers.Add("AccessKey", accesskey);
            using var response = await http.SendAsync(message);

            Console.Write($"Response: {await response.Content.ReadAsStringAsync()}");

            if(response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.NoContent) { //request succeeded but no response given
                Console.WriteLine("200 OK");
                Console.WriteLine("Pull Zone purged.");
            }

            else {
                throw new Exception($"failed to purge pullzone: {response.StatusCode}");
            }
        }
    }

    private static async Task DeployContentAsync(BunnyCDNStorage context, string storagezone, string contentpath) {
        Console.WriteLine($"Deploying static content. contentpath: {contentpath}");
        var files = Directory.GetFiles(contentpath);
        
        for(var i = 0; i < files.Length; i++) {
            var current = Path.GetFileName(files[i]);
            Console.WriteLine($"Uploading file: {current}");
            await context.UploadAsync(files[i], $"/{storagezone}/{current}");
        }
        Console.WriteLine("Content deployed.");
    }
}

