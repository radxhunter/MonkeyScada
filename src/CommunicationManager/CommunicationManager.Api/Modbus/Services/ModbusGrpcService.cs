using CommunicationManager.Api.Modbus.Models;
using Grpc.Core;
using MonkeyScada.CommunicationManager.Api.Modbus;
using System.Collections.Concurrent;

namespace CommunicationManager.Api.Modbus.Services
{
    internal sealed class ModbusGrpcService : ModbusFeed.ModbusFeedBase
    {
        private readonly BlockingCollection<MeasurementPair<double, long>> _measurementPairs = new();
        private readonly IModbusCommunicator _modbusCommunicator;

        public ModbusGrpcService(IModbusCommunicator modbusCommunicator)
        {
            _modbusCommunicator = modbusCommunicator;
        }

        public override Task<GetSensorNamesResponse> GetSensorNames(GetSensorNamesRequest request, ServerCallContext context)
            => Task.FromResult(new GetSensorNamesResponse()
                { SensorNames = { _modbusCommunicator.GetSensorNames() } });

        public override async Task SubscribeModbus(ModbusRequest request, 
            IServerStreamWriter<ModbusResponse> responseStream, ServerCallContext context)
        {
            _modbusCommunicator.MeasurementUpdated += OnModbusUpdated;

            while (!context.CancellationToken.IsCancellationRequested)
            {
                if (!_measurementPairs.TryTake(out var measurementPair))
                {
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(request.SensorName) && request.SensorName != measurementPair.SensorName)
                {
                    continue;
                }

                await responseStream.WriteAsync(new ModbusResponse
                {
                    SensorName = measurementPair.SensorName,
                    Value = (int) (measurementPair.Value),
                    Timestamp = measurementPair.Time
                });
            }

            _modbusCommunicator.MeasurementUpdated -= OnModbusUpdated;

            void OnModbusUpdated(object? sender, MeasurementPair<double, long> measurementPair)
                => _measurementPairs.TryAdd(measurementPair);
        }
    }
}
