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


        public PlaceBetEndpoint(IEventsService eventsService, IPlayersService playersService, IBetsService betsService)
        {
            _eventsService = eventsService;
            _playersService = playersService;
            _betsService = betsService;
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

            _betsService.CreateBet(player.Id, eventObj.Id, odds.Id, request.BetSum, ResultCode.Success);
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
                AddError(r => r.PlayerId, "Not enough balance for this bet!");
            }
            if(request.BetSum <= 0)
            {
                AddError(r => r.PlayerId, "Bet value must be over 0");
            }

            //here should be valid odds validation that i do not understand.
            //var OddsPercentage = _eventsService.CalculateOddPercentage(eventObj, odds.Id);
            //if (OddsPercentage && eventObj.IsLive)
            //{

            //}
            if (ValidationFailed)
            {
                _betsService.CreateBet(player.Id, eventObj.Id, odds.Id, request.BetSum, ResultCode.Failed);
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
                AddError(r => r.PlayerId, "Event is not found!");
            }
            if (odds == null)
            {
                AddError(r => r.PlayerId, "Event odd is not found!");
            }
            ThrowIfAnyErrors(); // throws error is request is not valid

        }
    }
}
