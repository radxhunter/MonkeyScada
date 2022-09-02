// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();
services.AddHttpClient();
var serviceProvider = services.BuildServiceProvider();

var client = serviceProvider.GetService<HttpClient>();

var response = await client.GetFromJsonAsync<string>("https://randomuser.me/api/?results=1");

Console.WriteLine(response);

    