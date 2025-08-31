using Grpc.Core;
using MeterReader.gRPC;
using static MeterReader.gRPC.MeterReadingService;

namespace MeterReader.Services
{
    public class MeterReadingService(
        IReadingRepository repository,
        ILogger<MeterReadingService> logger
        ) : MeterReadingServiceBase
    { 
        public override async Task<StatusMessage> AddReading(ReadingPacket request, ServerCallContext context)
        {   
            foreach (var reading in request.Readings)
            {
                var readingValue = new MeterReading()
                {
                    CustomerId = reading.CustomerId,
                    Value = reading.ReadingValue,
                    ReadingDate = reading.ReadingTime.ToDateTime(),
                };

                repository.AddEntity( readingValue );
            }

            if (await repository.SaveAllAsync())
            {
                return new StatusMessage
                {
                    Message = "Sucessfully added to DB",
                    Success = true
                };
            }
            return new StatusMessage
            {
                Message = "Failed to store readings in DB",
                Success = false
            };

        }
    }
}
