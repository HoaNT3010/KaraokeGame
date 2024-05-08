using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using KaraokeGame.Extensions;

namespace KaraokeGame.Api
{
    public class WebRequest
    {
        public static async Task<T> GetAsync<T>(string requestUrl, CancellationToken cancellationToken = default)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
            {
                try
                {
                    await request.SendWebRequestAsync(cancellationToken);

                    // Successful response (200)
                    if (request.responseCode == 200)
                    {
                        string text = request.downloadHandler.text;
                        return JsonConvert.DeserializeObject<T>(text);
                    }
                    // Failed response
                    else
                    {
                        Debug.LogWarning($"HTTP request failed with status code {request.responseCode}");
                        return default;
                    }
                }
                catch (JsonException ex)
                {
                    Debug.LogError($"JSON parsing error: {ex.Message}");
                    return default;
                }
                catch (OperationCanceledException)
                {
                    Debug.LogWarning("Operation canceled by user.");
                    return default;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Unexpected error during HTTP request: {ex.Message}");
                    return default;
                }
            }
        }

        public static async Task<long> HeadAsync(string requestUrl, CancellationToken cancellationToken = default)
        {
            var request = UnityWebRequest.Head(requestUrl);
            try
            {
                await request.SendWebRequestAsync(cancellationToken);
                return request.responseCode;
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return request.responseCode;
            }
            finally
            {
                request.Dispose();
            }
        }
    }
}
