using System;
using System.Net.Http;
using System.Text.Json;
using APZ_IoT.Models;
using APZ_IoT.Settings;


namespace APZ_IoT.Authorization
{
    public static class Authorization
    {
        public static async Task<(bool, bool)> Authenticate(string email, string password)
        {
            using (HttpClient client = new())
            {
                try
                {
                    var adminSettings = AppSettingsHelper.GetAdminSettings();

                    string baseUrl = adminSettings.AuthorizationAdress;

                    string url = $"{baseUrl}?email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}";

                    HttpContent content = new StringContent("");

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (responseBody != "Wrong email or password")
                    {
                        Console.WriteLine("Success");
                        User user = JsonSerializer.Deserialize<User>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (user.Role == "Admin")
                            return (true, true);

                        return (true, false);
                    }
                    Console.WriteLine("Wrong email or password");
                    return (false, false);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                    return (false, false);
                }
            }
        }
    }
}
