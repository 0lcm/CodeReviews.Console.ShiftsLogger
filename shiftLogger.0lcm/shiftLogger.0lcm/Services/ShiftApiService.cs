using System.Net.Http.Json;
using shiftLogger._0lcm.Configuration;
using shiftLogger._0lcm.ServiceContracts;
using shiftLogger.Shared;

namespace shiftLogger._0lcm.Services;

public class ShiftApiService(IHttpClientFactory httpClientFactory, DateFormatSettings dateFormatSettings)
    : IShiftApiService
{
    private static readonly string BaseUrl = ApiSettings.BaseUrl;
    private readonly string _dateIso = dateFormatSettings.DateIso;

    public async Task<string> GetUpcomingShifts()
    {
        return await GetAsync(ApiSettings.GetUpcomingShiftsUrl);
    }

    public async Task<string> GetAvailableShifts()
    {
        return await GetAsync(ApiSettings.GetAvailableShiftsUrl);
    }

    public async Task<string> GetShiftsByDate(DateOnly date)
    {
        return await GetAsync($"{ApiSettings.GetShiftsByDateIncompleteUrl}{date.ToString(_dateIso)}");
    }

    public async Task<string> GetShiftById(int shiftId)
    {
        return await GetAsync($"{ApiSettings.GetShiftByIdIncompleteUrl}{shiftId}");
    }

    public async Task<string> GetShiftsForEmployee(int employeeId)
    {
        return await GetAsync($"{ApiSettings.GetShiftsForEmployeeIncompleteUrl}{employeeId}");
    }

    public async Task PostShift(CreateShiftDto shiftDto)
    {
        var client = httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync(ApiSettings.PostShiftUrl, shiftDto);

        await HandleResponse(response);
    }

    public async Task AssignShift(int shiftId, int employeeId)
    {
        await PutAsync($"{ApiSettings.PutAssignShiftIncompleteUrl}{shiftId}/employee{employeeId}");
    }

    public async Task RemoveShift(int shiftId)
    {
        await PutAsync($"{ApiSettings.PutRemoveShiftIncompleteUrl}{shiftId}");
    }

    public async Task RemoveAllShiftsFromEmployee(int employeeId)
    {
        await PutAsync($"{ApiSettings.PutRemoveAllShiftsFromEmployeeIncompleteUrl}{employeeId}");
    }

    public async Task CompleteShift(int shiftId)
    {
        await PutAsync($"{ApiSettings.PutCompleteShiftIncompleteUrl}{shiftId}");
    }

    public async Task EditShift(int shiftId, CreateShiftDto shiftDto)
    {
        await PutAsync($"{ApiSettings.PutEditShiftIncompleteUrl}{shiftId}", shiftDto);
    }

    public async Task DeleteShift(int shiftId)
    {
        var client = httpClientFactory.CreateClient(BaseUrl);
        var url = $"{ApiSettings.DeleteShiftIncompleteUrl}{shiftId}";
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

    private async Task PutAsync(string path, CreateShiftDto? shiftDto = null)
    {
        var client = httpClientFactory.CreateClient(BaseUrl);

        HttpResponseMessage response;
        if (shiftDto != null)
            response = await client.PutAsJsonAsync(path, shiftDto);
        else
            response = await client.PutAsync(path, null);

        await HandleResponse(response);
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