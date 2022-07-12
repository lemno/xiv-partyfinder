using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PartyFinder;

public class Sandbox
{
    public volatile bool IsDataLoading;
    public volatile bool IsDataReady;

    private readonly HttpClient httpClient;

    public Sandbox()
    {
        this.httpClient = new HttpClient();
    }

    public static bool IsConfigSet()
    {
        return !string.IsNullOrEmpty(Service.Configuration.ClientId)
               && !string.IsNullOrEmpty(Service.Configuration.ClientSecret);
    }

/*    public async Task FetchGameData()
    {
        if (!this.IsTokenValid)
        {
            PluginLog.Error("FFLogs token not set.");
            return;
        }

        const string baseAddress = @"https://www.fflogs.com/api/v2/client";
        const string query = @"{""query"":""{worldData {expansions {name id zones {name id difficulties {name id} encounters {name id}}}}}""}";

        var content = new StringContent(query, Encoding.UTF8, "application/json");

        var dataResponse = await this.httpClient.PostAsync(baseAddress, content);
        try
        {
            var jsonContent = await dataResponse.Content.ReadAsStringAsync();
            Service.GameDataManager.SetDataFromJson(jsonContent);
        }
        catch (Exception e)
        {
            PluginLog.Error(e, "Error while fetching game data.");
        }
    }*/

    public async Task<dynamic?> FetchPF()
    {

        const string baseAddress = @"https://www.fflogs.com/api/v2/client";

        var query = "test";

        var content = new StringContent(query.ToString(), Encoding.UTF8, "application/json");

        var dataResponse = await this.httpClient.PostAsync(baseAddress, content);
        try
        {
            var jsonContent = await dataResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(jsonContent);
        }
        catch (Exception e)
        {
            /* PluginLog.Error(e, "Error while fetching data.");*/
            Console.WriteLine("Error parsing PF");
            return null;
        }
    }

    public void FetchData()
    {
        if (this.IsDataLoading)
        {
            return;
        }

        this.IsDataLoading = true;

        this.ResetData();

        Task.Run(async () =>
        {
            var rawData = await Service.FfLogsClient.FetchLogs(this).ConfigureAwait(false);
            if (rawData == null)
            {
                this.IsDataLoading = false;
                Service.MainWindow.SetErrorMessage("Could not reach FF Logs servers");
                PluginLog.Error("rawData is null");
                return;
            }

            if (rawData.data?.characterData?.character == null)
            {
                if (rawData.error != null && rawData.error == "Unauthenticated.")
                {
                    this.IsDataLoading = false;
                    Service.MainWindow.SetErrorMessage("API Client not valid, check config");
                    PluginLog.Log($"Unauthenticated: {rawData}");
                    return;
                }

                if (rawData.errors != null)
                {
                    this.IsDataLoading = false;
                    Service.MainWindow.SetErrorMessage("Malformed GraphQL query.");
                    PluginLog.Log($"Malformed GraphQL query: {rawData}");
                    return;
                }

                this.IsDataLoading = false;
                Service.MainWindow.SetErrorMessage("Character not found on FF Logs");
                return;
            }

            var character = rawData.data.characterData.character;

            if (character.hidden == "true")
            {
                this.IsDataLoading = false;
                Service.MainWindow.SetErrorMessage(
                    $"{this.FirstName} {this.LastName}@{this.WorldName}'s logs are hidden");
                return;
            }

            this.Encounters = new List<Encounter>();

            var properties = character.Properties();
            foreach (var prop in properties)
            {
                if (prop.Name != "hidden")
                {
                    this.ParseZone(prop.Value);
                }
            }

            this.IsDataReady = true;
            this.LoadedFirstName = this.FirstName;
            this.LoadedLastName = this.LastName;
            this.LoadedWorldName = this.WorldName;
        }).ContinueWith(t =>
        {
            this.IsDataLoading = false;
            if (!t.IsFaulted) return;
            if (t.Exception == null) return;
            Service.MainWindow.SetErrorMessage("Networking error, please try again");
            foreach (var e in t.Exception.Flatten().InnerExceptions)
            {
                PluginLog.Error(e, "Networking error");
            }
        });
    }

    public void ResetData()
    {
        this.IsDataReady = false;
    }
}