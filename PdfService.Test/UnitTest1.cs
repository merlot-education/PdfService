using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text.Json;

namespace PdfService.Test
{
    [TestClass]
    public class UnitTest1
    {
        protected WebApplicationFactory<Program> ApplicationFactory { get; init; }

        public UnitTest1()
        {
            ApplicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                //builder.UseEnvironment("Test");
            });
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            HttpClient client = ApplicationFactory.CreateClient();
            Dictionary<string, string> myDict = new();
            myDict.Add("Test1", "Test2");
            byte[]? result = await client.PostJsonAsync<Dictionary<string, string>, byte[]>("/PdfProcessor/PdfContract", myDict);
            if (result is not null)
            {
                System.IO.File.WriteAllBytes(@"D:\myfile.pdf", result);
            }
            Assert.IsNotNull(result);
        }
    }

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


                MemoryStream contentStream = (MemoryStream) await response.Content.ReadAsStreamAsync();
                if(typeof(TResponse) == typeof(byte[]))
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
}