using Clients.Console;
using Grpc.Net.Client;

Console.WriteLine("Hello, World!");

using var channel = GrpcChannel.ForAddress("http://localhost:5031");
var client = new ModbusFeed.ModbusFeedClient(channel);

Console.WriteLine("Press any key to get sensor names...");
Console.ReadKey();

var sensorNamesResponse = await client.GetSensorNamesAsync(new GetSensorNamesRequest());
foreach (var sensorName in sensorNamesResponse.SensorNames)
{
    Console.WriteLine(sensorName);
}

Console.Write("Provide a sensor name (or leave empty): ");
var providedSensorName = Console.ReadLine()?.ToUpperInvariant();
if (!string.IsNullOrWhiteSpace(providedSensorName) && !sensorNamesResponse.SensorNames.Contains(providedSensorName))
{
    Console.WriteLine($"Invalid sensor name: .{providedSensorName}.");
    return;
}

var measurementStream = client.SubscribeModbus(new ModbusRequest
{
    SensorName = providedSensorName
});

while (await measurementStream.ResponseStream.MoveNext(CancellationToken.None))
{
    var current = measurementStream.ResponseStream.Current;
    Console.WriteLine($"{DateTimeOffset.FromUnixTimeMilliseconds(current.Timestamp):T} -> " +
        $"{current.SensorName} = {current.Value}:F");
}