using System.Net.Http.Json;
using System.Text.Json;

namespace PdfService.Test;

public static class HttpClientExtensions
{
    private static JsonSerializerOptions TrainCloudJsonSerializerOptions { get; set; } = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

    public static async Task<TResponse?> GetJsonAsync<TResponse>(this HttpClient client, string requestUri)
    {
        try
        {
            using HttpResponseMessage response = await client.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            Stream contentStream = await response.Content.ReadAsStreamAsync();
            TResponse? model = await JsonSerializer.DeserializeAsync<TResponse>(contentStream, TrainCloudJsonSerializerOptions);

            return model;
        }
        catch (Exception ex)
        {
            return default;
        }
    }

    public static async Task<TResponse?> PostJsonAsync<TPost, TResponse>(this HttpClient client, string requestUri, TPost param)
    {
        try
        {
            HttpContent parameterContent = JsonContent.Create(param, typeof(TPost));

            using HttpResponseMessage response = await client.PostAsync(requestUri, parameterContent);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }


            MemoryStream contentStream = (MemoryStream)await response.Content.ReadAsStreamAsync();
            if (typeof(TResponse) == typeof(byte[]))
            {
                TResponse myResponse = (TResponse)Convert.ChangeType(contentStream.ToArray(), typeof(TResponse));
                return myResponse;
                //byte[] myByteArray = new BinaryReader(contentStream).ReadBytes((int) contentStream.Length);
            }
            TResponse? model = await JsonSerializer.DeserializeAsync<TResponse>(contentStream, TrainCloudJsonSerializerOptions);

            return model;
        }
        catch (Exception ex)
        {
            return default;
        }
    }

    public static async Task<TResponse?> PatchJsonAsync<TPatch, TResponse>(this HttpClient client, string requestUri, TPatch param)
    {
        try
        {
            HttpContent parameterContent = JsonContent.Create(param, typeof(TPatch));

            using HttpResponseMessage response = await client.PatchAsync(requestUri, parameterContent);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            Stream contentStream = await response.Content.ReadAsStreamAsync();
            TResponse? model = await JsonSerializer.DeserializeAsync<TResponse>(contentStream, TrainCloudJsonSerializerOptions);

            return model;
        }
        catch (Exception ex)
        {
            return default;
        }
    }

    public static async Task<bool> DeleteAsyncX(this HttpClient client, string requestUri)
    {
        try
        {
            using HttpResponseMessage response = await client.DeleteAsync(requestUri);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
