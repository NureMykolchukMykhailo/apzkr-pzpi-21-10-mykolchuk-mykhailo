using APZ_IoT.Settings.Admin;
using APZ_IoT.Settings.User;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using APZ_IoT.Authorization;

namespace APZ_IoT.Settings
{
    internal static class AppSettingsHelper
    {
        public static bool UserAuthorized = false;
        public static bool UserIsAdmin = false;

        public static void UpdateUserSettings(UserSettings newSettings) 
        {
            var newConfig = new
            {
                UserSettings = newSettings
            };

            var json = JsonSerializer.Serialize(newConfig);
            File.WriteAllText("../../../Settings/User/userSettings.json", json);

            Console.WriteLine("Settings updated");
        }

        public static void UpdateAdminSettings(AdminSettings newSettings)
        {
            var newConfig = new
            {
                AdminSettings = newSettings
            };

            var json = JsonSerializer.Serialize(newConfig);
            File.WriteAllText("../../../Settings/Admin/adminSettings.json", json);

            Console.WriteLine("Settings updated");
        }

        public static AdminSettings GetAdminSettings()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName)
                .AddJsonFile("Settings/Admin/adminSettings.json", optional: false, reloadOnChange: true)
                .Build();

            AdminSettings adminSettings = new();
            configuration.GetSection("AdminSettings").Bind(adminSettings);
            return adminSettings;
        }

        public static UserSettings GetUserSettings()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName)
                .AddJsonFile("Settings/User/userSettings.json", optional: false, reloadOnChange: true)
                .Build();

            UserSettings userSettings = new();
            configuration.GetSection("UserSettings").Bind(userSettings);
            return userSettings;
        }

        public static void ConsoleWriteAdminSettings()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName)
                .AddJsonFile("Settings/Admin/adminSettings.json", optional: false, reloadOnChange: true)
                .Build();

            AdminSettings adminSettings = new();
            configuration.GetSection("AdminSettings").Bind(adminSettings);
            Console.WriteLine("KafkaTopic:" + " " + adminSettings.KafkaTopic);
            Console.WriteLine("BootstrapServers:" + " " + adminSettings.BootstrapServers);
            Console.WriteLine("AuthorizationAdress:" + " " + adminSettings.AuthorizationAdress);
        }

        public static void ConsoleWriteUserSettings()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName)
                .AddJsonFile("Settings/User/userSettings.json", optional: false, reloadOnChange: true)
                .Build();

            UserSettings userSettings = new();
            configuration.GetSection("UserSettings").Bind(userSettings);
            Console.WriteLine("DeviceId:" + " " + userSettings.DeviceId);
            Console.WriteLine("MinEngineSpeed:" + " " + userSettings.MinEngineSpeed);
            Console.WriteLine("MaxEngineSpeed:" + " " + userSettings.MaxEngineSpeed);
            Console.WriteLine("MinAcceleration:" + " " + userSettings.MinAcceleration);
            Console.WriteLine("MaxAcceleration:" + " " + userSettings.MaxAcceleration);
            Console.WriteLine("MaxSpeed:" + " " + userSettings.MaxSpeed);
        }

        public static async Task StartSettingsDialog()
        {
            while (true)
            {
                Console.WriteLine("This is start menu. Press q to start tracking vehicle states. Press a to go to settings");
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Q)
                    break;
                if (key.Key == ConsoleKey.A)
                {
                    Console.WriteLine("Press q to work with user settings. Press a to  work with admin settings");
                    key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Q)
                    {
                        if (!UserAuthorized)
                        {
                            Console.WriteLine("Authorize");
                            Console.WriteLine("Email:");
                            string email = Console.ReadLine();
                            Console.WriteLine("Password:");
                            string password = Console.ReadLine();

                            (UserAuthorized, UserIsAdmin) = await Authorization.Authorization.Authenticate(email, password);
                            if (!UserAuthorized)
                                continue;
                        }
                        Console.WriteLine("Press q to view user settings. Press a to change settings");

                        key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Q)
                        {
                            ConsoleWriteUserSettings();
                        }
                        else if (key.Key == ConsoleKey.A)
                        {
                            UserSettings newSettings = new();

                            Console.WriteLine("New DeviceId");
                            newSettings.DeviceId = Console.ReadLine();
                            Console.WriteLine("New MaxEngineSpeed");
                            newSettings.MaxEngineSpeed = Console.ReadLine();
                            Console.WriteLine("New MinEngineSpeed");
                            newSettings.MinEngineSpeed = Console.ReadLine();
                            Console.WriteLine("New MaxAcceleration");
                            newSettings.MaxAcceleration = Console.ReadLine();
                            Console.WriteLine("New MinAcceleration");
                            newSettings.MinAcceleration = Console.ReadLine();
                            Console.WriteLine("New MaxSpeed");
                            newSettings.MaxSpeed = Console.ReadLine();
                            UpdateUserSettings(newSettings);
                        }
                        continue;
                    }
                    if (key.Key == ConsoleKey.A)
                    {
                        if (!UserAuthorized)
                        {
                            Console.WriteLine("Authorize");
                            Console.WriteLine("Email:");
                            string email = Console.ReadLine();
                            Console.WriteLine("Password:");
                            string password = Console.ReadLine();

                            (UserAuthorized, UserIsAdmin) = await Authorization.Authorization.Authenticate(email, password);
                            if (!UserAuthorized)
                                continue;
                            if (!UserIsAdmin)
                            {
                                Console.WriteLine("You dont have admin rights");
                                continue;
                            }
                        }
                        Console.WriteLine("Press q to view admin settings. Press a to change settings");

                        key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Q)
                        {
                            ConsoleWriteAdminSettings();
                        }
                        else if (key.Key == ConsoleKey.A)
                        {
                            AdminSettings newSettings = new();

                            Console.WriteLine("New BootstrapServers");
                            newSettings.BootstrapServers = Console.ReadLine();
                            Console.WriteLine("New KafkaTopic");
                            newSettings.KafkaTopic = Console.ReadLine();
                            Console.WriteLine("New AuthorizationAdress");
                            newSettings.AuthorizationAdress = Console.ReadLine();
                            UpdateAdminSettings(newSettings);
                        }
                        continue;
                    }
                }
            }

        }
    }
}
