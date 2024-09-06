using FastEndpoints;
using KickerTech.TaskApi.Models;
using KickerTech.TaskApi.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;

namespace KickerTech.TaskApi.Endpoints.PlaceBet
{
    public class PlaceBetEndpoint : Endpoint<PlaceBetRequest, PlaceBetResponse>
    {
        private readonly IEventsService _eventsService;
        private readonly IPlayersService _playersService;
        private readonly IBetsService _betsService;
        private readonly IOddsService _oddsService;


        public PlaceBetEndpoint(IEventsService eventsService, IPlayersService playersService, IBetsService betsService, IOddsService oddsService)
        {
            _eventsService = eventsService;
            _playersService = playersService;
            _betsService = betsService;
            _oddsService = oddsService;
        }

        public override void Configure()
        {
            Post("/api/bets/placeBet");
            AllowAnonymous();
        }

        public override async Task HandleAsync(PlaceBetRequest request, CancellationToken ct)
        {
            var eventObj = _eventsService.GetEvents().FirstOrDefault(s => s.Id == request.EventId);
            var player = _playersService.GetPlayers().FirstOrDefault(s => s.Id == request.PlayerId);
            var odds = eventObj?.Odds?.FirstOrDefault(s => s.Id == request.OddId);

            //validate if request is valid.
            ValidateRequest(eventObj, player, odds);

            // validate if bet match criteria
            ValidateBet(eventObj, player, odds, request);

            _betsService.CreateBet(player.Id, eventObj.Id, odds.Id,request.OddValue, request.BetSum, ResultCode.Success);
            _playersService.UpdatePlayerBalance(player.Id, player.Balance - request.BetSum);

            Response = new PlaceBetResponse
            {
                ResultCode = ResultCode.Success.ToString()
            };
        }

        private void ValidateBet(Event? eventObj, Player? player, Odd? odds, PlaceBetRequest request)
        {
            if (player.Balance < request.BetSum)
            {
                AddError(r => r.BetSum, "Not enough balance for this bet!");
            }
            if (request.BetSum <= 0)
            {
                AddError(r => r.BetSum, "Bet value must be over 0");
            }
            if(request.OddValue <= 0)
            {
                AddError(r => r.OddValue, "Odd value must be not zero");
            }

            var oddTolerancePercentage = _oddsService.GetOddTolerancePercentage(odds.Value, request.OddValue);

            if (eventObj.IsLive && oddTolerancePercentage > 10)
            {
                AddError(r => r.OddValue, "Odd tolerance is out of accepted bounds.");
            }
            else if(!eventObj.IsLive && oddTolerancePercentage > 5)
            {
                AddError(r => r.OddValue, "Odd tolerance is out of accepted bounds.");
            }

            if (ValidationFailed)
            {
                _betsService.CreateBet(player.Id, eventObj.Id, odds.Id, request.OddValue, request.BetSum, ResultCode.Failed);
            }

            ThrowIfAnyErrors();
        }


        private void ValidateRequest(Event? eventObj, Player? player, Odd? odds)
        {
            if (player == null)
            {
                AddError(r => r.PlayerId, "Player is not found!");
            }
            if (eventObj == null)
            {
                AddError(r => r.EventId, "Event is not found!");
            }
            if (odds == null)
            {
                AddError(r => r.OddId, "Event odd is not found!");
            }
            ThrowIfAnyErrors(); // throws error is request is not valid
        }
    }
}
