using System.Net.Http.Json;
using shiftLogger._0lcm.Configuration;
using shiftLogger._0lcm.ServiceContracts;
using shiftLogger.Shared;

namespace shiftLogger._0lcm.Services;

public class EmployeeApiService(IHttpClientFactory httpClientFactory) : IEmployeeApiService
{
    private static readonly string BaseUrl = ApiSettings.BaseUrl;

    public async Task<string> RequestAllEmployees()
    {
        return await GetAsync(ApiSettings.GetAllEmployeesUrl);
    }

    public async Task<string> RequestEmployeeById(int employeeId)
    {
        return await GetAsync($"{ApiSettings.GetEmployeeByIdIncompleteUrl}{employeeId}");
    }

    public async Task<string> RequestEmployeeByName(string name)
    {
        return await GetAsync($"{ApiSettings.GetEmployeeByNameIncompleteUrl}{name}");
    }

    public async Task PostEmployee(CreateEmployeeDto employeeDto)
    {
        var client = httpClientFactory.CreateClient(BaseUrl);
        var response = await client.PostAsJsonAsync(ApiSettings.PostEmployeeUrl, employeeDto);

        await HandleResponse(response);
    }

    public async Task PutEmployee(int employeeId, CreateEmployeeDto employeeDto)
    {
        var client = httpClientFactory.CreateClient(BaseUrl);
        var url = $"{ApiSettings.PutEmployeeIncompleteUrl}{employeeId}";
        var response = await client.PutAsJsonAsync(url, employeeDto);

        await HandleResponse(response);
    }

    public async Task DeleteEmployee(int employeeId)
    {
        var client = httpClientFactory.CreateClient(BaseUrl);
        var url = $"{ApiSettings.DeleteEmployeeIncompleteUrl}{employeeId}";
        var response = await client.DeleteAsync(url);

        await HandleResponse(response);
    }

    //------- Helper Methods -------
    private async Task<string> GetAsync(string path)
    {
        var client = httpClientFactory.CreateClient(BaseUrl);
        var response = await client.GetAsync(path);

        return await HandleResponse(response);
    }

    private async Task<string> HandleResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

        if ((int)response.StatusCode >= 400 && (int)response.StatusCode < 500)
            throw new HttpRequestException($"Client side error occurred, status code: {response.StatusCode}.", null,
                response.StatusCode);

        if ((int)response.StatusCode >= 500)
            throw new HttpRequestException($"Server side error occurred, status code: {response.StatusCode}.", null,
                response.StatusCode);

        throw new HttpRequestException($"An unexpected error occurred with the status code {response.StatusCode}.",
            null, response.StatusCode);
    }
}