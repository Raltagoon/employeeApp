using FlyingCars.Services;

namespace FlyingCars.Api
{
    public class HistoryApi
    {
        public void Register(WebApplication app)
        {
            // read
            app.MapGet("/history", async (HistoryService service) =>
            {
                return Results.Ok(await service.GetAllAsync());
            });
        }
    }
}
