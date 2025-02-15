using Microsoft.Extensions.Http;
using ChefKnife.HttpService.ApiResponse;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;

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

    // Add additional header to service clients
    public void AddHeader(string key, string value);
}

public class HttpService : IHttpService
{
    readonly IHttpClientFactory _httpClientFactory; 
    string? _basicAuthHeaderValue;
    string? _bearerAuthHeaderValue;
    Dictionary<string, string> _additionalHeaders = new();

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

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error response directly.
                return await CreateErrorCodeResponse<T>(response);
            }

            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<T?>(content);

            return new ApiResponse<T?>(obj);
        }
        catch (HttpRequestException httpEx)
        {
            // Handle specific HTTP errors.
            return CreateErrorCodeResponse<T>(httpEx);
        }
        catch (Exception ex)
        {
            // Handle generic errors.
            return CreateFallbackResponse<T>(ex);
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

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error response directly.
                return await CreateErrorCodeResponse<Y>(response);
            }

            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Y?>(content);
            return new ApiResponse<Y?>(obj);
        }
        catch (Exception ex)
        {
            return CreateFallbackResponse<Y>(ex);
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

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error response directly.
                return await CreateErrorCodeResponse<Y>(response);
            }

            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Y?>(content);

            return new ApiResponse<Y?>(obj);
        }
        catch (Exception ex)
        {
            return CreateFallbackResponse<Y>(ex);
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

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error response directly.
                return await CreateErrorCodeResponse<Y>(response);
            }

            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Y?>(content);
            return new ApiResponse<Y?>(obj);
        }
        catch (Exception ex)
        {
            return CreateFallbackResponse<Y>(ex);
        }
    }

    // Generic DELETE request
    public async Task<ApiResponse<T?>> DeleteAsync<T>(string url)
    {
        try
        {
            var client = CreateClient();

            var response = await client.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error response directly.
                return await CreateErrorCodeResponse<T>(response);
            }

            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<T?>(content);
            return new ApiResponse<T?>(obj);
        }
        catch (Exception ex)
        {
            return CreateFallbackResponse<T>(ex);
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

    public void AddHeader(string key, string value)
    {
        _additionalHeaders[key] = value;
    }

    private HttpClient CreateClient()
    {
        var result = _httpClientFactory.CreateClient();

        result.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        result.DefaultRequestHeaders.Add("Accept", "application/json");

        if (_basicAuthHeaderValue != null)
        {
            result.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _basicAuthHeaderValue);
        }

        if (_bearerAuthHeaderValue != null)
        {
            result.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerAuthHeaderValue);
        }

        foreach (var header in _additionalHeaders)
        {
            result.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        return result;
    }

    private async Task<ApiResponse<T?>> CreateErrorCodeResponse<T>(HttpResponseMessage response)
    {
        var errorContent = await response.Content.ReadAsStringAsync();
        return new ApiResponse<T?>(
            responseMessage: $"Request failed with status code {(int)response.StatusCode}: {errorContent}",
            httpStatusCode: (int)response.StatusCode,
            responseException: new ApiException
            {
                Message = errorContent,
                Code = (int)response.StatusCode,
                StackTrace = null, // StackTrace is not applicable here.
                InnerExcpetion = null
            });
    }

    private ApiResponse<T?> CreateErrorCodeResponse<T>(HttpRequestException httpEx)
    {
        return new ApiResponse<T?>(
            responseMessage: httpEx.Message,
            httpStatusCode: (int)httpEx.Data["StatusCode"]!,
            responseException: new ApiException
            {
                Message = httpEx.Message,
                Code = (int)httpEx.Data["StatusCode"]!,
                StackTrace = httpEx.StackTrace,
                InnerExcpetion = httpEx.InnerException?.ToString()
            });
    }

    private static ApiResponse<T?> CreateFallbackResponse<T>(Exception? ex = null)
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