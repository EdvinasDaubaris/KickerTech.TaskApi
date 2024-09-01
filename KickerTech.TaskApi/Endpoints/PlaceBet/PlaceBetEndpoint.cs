using FastEndpoints;
using Microsoft.Extensions.Caching.Memory;

namespace KickerTech.TaskApi.Endpoints.PlaceBet
{
    public class PlaceBetEndpoint : Endpoint<PlaceBetRequest>
    {

        public override void Configure()
        {
            Post("/api/bets/placeBet");
            AllowAnonymous();
        }

        public override async Task HandleAsync(PlaceBetRequest request, CancellationToken ct)
        {
            

            
        }
    }
}
