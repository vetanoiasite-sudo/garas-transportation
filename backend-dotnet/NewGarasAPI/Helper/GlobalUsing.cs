//global using for all files in the project (you do not need to add add using in any athor file )

global using Microsoft.AspNetCore.Mvc;
global using NewGarasAPI.Models.Common;
global using System.Runtime.Serialization;
global using NewGaras.Infrastructure.Entities;
global using NewGaras.Infrastructure.Interfaces.ServicesInterfaces;
global using NewGarasAPI.Helper;
//---------------------------------global variables-----------------------------------------------

public static class Globals
{
    public static IConfigurationRoot MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    public static string baseURL = MyConfig.GetValue<string>("AppSettings:baseURL");
};