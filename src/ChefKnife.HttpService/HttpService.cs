using Microsoft.Extensions.Http;
using ChefKnife.HttpService.ApiResponse;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ChefKnife.HttpService;

public interface IHttpService
{
    // Generic GET request
    Task<ApiResponse<T?>> GetAsync<T>(string url);

    // Generic POST request
    Task<ApiResponse<Y?>> PostAsync<X, Y>(string url, X data);

    // FormUrlEncodedContent POST request
    Task<ApiResponse<Y?>> PostAsync<Y>(string url, FormUrlEncodedContent data);

    // Generic PUT request
    Task<ApiResponse<Y?>> PutAsync<X, Y>(string url, X data);

    // Generic DELETE request
    Task<ApiResponse<T?>> DeleteAsync<T>(string url);

    // Method to set Basic Authentication credentials
    void SetBasicAuthentication(string username, string password);

    // Method to set Bearer Authentication credentials
    void SetBearerAuthentication(string token);
}

public class HttpService : IHttpService
{
    readonly IHttpClientFactory _httpClientFactory; 
    string? _basicAuthHeaderValue;
    string? _bearerAuthHeaderValue;

    public HttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Generic GET request
    public async Task<ApiResponse<T?>> GetAsync<T>(string url)
    {
        try
        {
            var client = CreateClient();

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
            var client = CreateClient();
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

    // FormUrlEncodedContent POST request
    public async Task<ApiResponse<Y?>> PostAsync<Y>(string url, FormUrlEncodedContent formContent)
    {
        try
        {
            var client = CreateClient();

            

            // Set headers if necessary
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsync(url, formContent);

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
            var client = CreateClient();
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
            var client = CreateClient();

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

    public void SetBasicAuthentication(string username, string password)
    {
        // Encode username and password to Base64
        _basicAuthHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
    }

    public void SetBearerAuthentication(string token)
    {
        _bearerAuthHeaderValue = token;
    }

    private HttpClient CreateClient()
    {
        var result = _httpClientFactory.CreateClient();

        if (_basicAuthHeaderValue != null)
        {
            result.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _basicAuthHeaderValue);
        }

        if (_bearerAuthHeaderValue != null)
        {
            result.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerAuthHeaderValue);
        }

        return result;
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