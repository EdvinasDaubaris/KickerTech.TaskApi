using FastEndpoints;
using KickerTech.TaskApi.Endpoints.PlaceBet;
using KickerTech.TaskApi.Services;
using Microsoft.Extensions.Caching.Memory;

namespace KickerTech.TaskApi.Endpoints.GetBets
{
    public class GetBetsEndpoint : EndpointWithoutRequest<List<GetBetsResponse>>
    {
        private readonly IBetsService _betsService;

        public GetBetsEndpoint(IBetsService betsService)
        {
            _betsService = betsService;
        }

        public override void Configure()
        {
            Get("/api/bets");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            Response = _betsService.GetBets().Select(s => new GetBetsResponse
            {
                BetId = s.ResultCode == Models.ResultCode.Success ? s.Id : null,
                EventId = s.EventId,
                OddId = s.OddId,
                ResultCode = s.ResultCode.ToString()
            }).ToList(); 

        }
    }
}
