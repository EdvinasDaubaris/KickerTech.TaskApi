using FastEndpoints;
using KickerTech.TaskApi.Endpoints.PlaceBet;
using Microsoft.Extensions.Caching.Memory;

namespace KickerTech.TaskApi.Endpoints.GetBets
{
    public class GetBetsEndpoint : EndpointWithoutRequest<GetBetsResponse>
    {

        public override void Configure()
        {
            Get("/api/bets");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            

            
        }
    }
}
