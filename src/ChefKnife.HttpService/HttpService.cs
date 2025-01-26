using Microsoft.Extensions.Http;
using ChefKnife.HttpService.ApiResponse;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChefKnife.HttpService;

public interface IHttpService
{
    // Generic GET request
    Task<ApiResponse<T?>> GetAsync<T>(string url);

    // Generic POST request
    Task<ApiResponse<Y?>> PostAsync<X, Y>(string url, X data);

    // Generic PUT request
    Task<ApiResponse<Y?>> PutAsync<X, Y>(string url, X data);

    // Generic DELETE request
    Task<ApiResponse<T?>> DeleteAsync<T>(string url);
}

public class HttpService : IHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Generic GET request
    public async Task<ApiResponse<T?>> GetAsync<T>(string url)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<T?>(content);
            return new ApiResponse<T?>(obj);
        }
        catch (Exception ex)
        {
            return CreateServerErrorApiResponse<T>(ex);
        }
    }

    // Generic POST request
    public async Task<ApiResponse<Y?>> PostAsync<X, Y>(string url, X data)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(url, jsonContent);

            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Y?>(content);
            return new ApiResponse<Y?>(obj);
        }
        catch (Exception ex)
        {
            return CreateServerErrorApiResponse<Y>(ex);
        }
    }

    // Generic PUT request
    public async Task<ApiResponse<Y?>> PutAsync<X, Y>(string url, X data)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PutAsync(url, jsonContent);

            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Y?>(content);
            return new ApiResponse<Y?>(obj);
        }
        catch (Exception ex)
        {
            return CreateServerErrorApiResponse<Y>(ex);
        }
    }

    // Generic DELETE request
    public async Task<ApiResponse<T?>> DeleteAsync<T>(string url)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.DeleteAsync(url);

            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<T?>(content);
            return new ApiResponse<T?>(obj);
        }
        catch (Exception ex)
        {
            return CreateServerErrorApiResponse<T>(ex);
        }
    }

    private static ApiResponse<T?> CreateServerErrorApiResponse<T>(Exception? ex = null)
    {
        return new ApiResponse<T?>()
        {
            StatusCode = 500,
            Message = ex?.Message ?? string.Empty,
            IsSuccessful = false,
            Data = default,
            Exception = null,
        };
    }
}