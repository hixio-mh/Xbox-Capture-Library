using XboxGameClipLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using XboxGameClipLibrary.Models.Screenshots;
using XboxGameClipLibrary.Views.Pages;

namespace XboxGameClipLibrary.API
{
    public class XboxApiDataService
    {
        public class ApiException : Exception
        {
            public int StatusCode { get; set; }
            public string Content { get; set; }
        }

        private static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default;

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jtr);
                return searchResult;
            }
        }

        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }

        public static async Task<JObject> GetProfileFromStringCallAsync(CancellationToken cancellationToken)
        {
            var content = ""; 

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, "https://xapi.us/v2/profile"))
            {
                request.Headers.Add("X-Auth", Properties.Settings.Default.xboxApiKey);

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    try
                    {
                        var stream = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            return JObject.Parse(stream);
                        }
                        else
                        {
                            throw new ApiException();
                        }
                    }
                    catch (ApiException)
                    {
                        ExceptionPage exceptionPage = new ExceptionPage();
                        exceptionPage.StatusCode = (int) response.StatusCode;
                        exceptionPage.Content = content;

                        Navigation.Navigation.Navigate(exceptionPage);
                    }

                    return null;
                }
            }
        }

        public static async Task<List<Screenshot>> GetScreenshotsFromStreamCallAsync(CancellationToken cancellationToken, string xuid)
        {
            var content = "";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, "https://xapi.us/v2/" + xuid + "/screenshots"))
            {
                request.Headers.Add("X-Auth", Properties.Settings.Default.xboxApiKey);

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    try
                    {
                        var stream = await response.Content.ReadAsStreamAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            return DeserializeJsonFromStream<List<Screenshot>>(stream);
                        }
                        content = await StreamToStringAsync(stream);
                    }
                    catch (ApiException e)
                    {
                    }

                    return null;
                }
            }
        }

        public static async Task<List<GameClip>> GetGameClipsFromStreamCallAsync(CancellationToken cancellationToken, string xuid)
        {
            var content = "";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, "https://xapi.us/v2/" + xuid + "/game-clips"))
            {
                request.Headers.Add("X-Auth", Properties.Settings.Default.xboxApiKey);

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    try
                    {
                        var stream = await response.Content.ReadAsStreamAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            return DeserializeJsonFromStream<List<GameClip>>(stream);
                        }

                        content = await StreamToStringAsync(stream);
                    }
                    catch (ApiException e)
                    {
                    }

                    return null;
                }
            }
        }
    }
}