using FakeItEasy;
using FastEndpoints;
using KickerTech.TaskApi.Endpoints.PlaceBet;
using KickerTech.TaskApi.Models;
using KickerTech.TaskApi.Services;
using Xunit;

namespace KickerTech.TaskApi.Tests
{
    public class PlaceBetsTests
    {
        private readonly IEventsService _eventsServiceFake;
        private readonly IPlayersService _playersServiceFake;
        private readonly IBetsService _betsServiceFake;
        private readonly IOddsService _oddsServiceFake;
        private readonly PlaceBetEndpoint _endpoint;

        public PlaceBetsTests()
        {
            _eventsServiceFake = A.Fake<IEventsService>();
            _playersServiceFake = A.Fake<IPlayersService>();
            _betsServiceFake = A.Fake<IBetsService>();
            _oddsServiceFake = A.Fake<IOddsService>();
            _endpoint = Factory.Create<PlaceBetEndpoint>(_eventsServiceFake,
                _playersServiceFake,
                _betsServiceFake,_oddsServiceFake);
           
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnSuccess_WhenRequestIsValid()
        {
            // Arrange
            var player = new Player { Id = 1, Balance = 100 };
            var eventObj = new Event
            {
                Id = 1,
                IsLive = false,
                Odds = new List<Odd> { new Odd { Id = 1, Value = 1.5m } }
            };
            var request = new PlaceBetRequest
            {
                PlayerId = 1,
                EventId = 1,
                OddId = 1,
                BetSum = 50,
                OddValue = 1.5m
            };


            A.CallTo(() => _playersServiceFake.GetPlayers()).Returns(new List<Player> { player });
            A.CallTo(() => _eventsServiceFake.GetEvents()).Returns(new List<Event> { eventObj });
            A.CallTo(() => _betsServiceFake.CreateBet(A<int>._, A<int>._, A<int>._, A<decimal>._, A<decimal>._, A<ResultCode>._)).DoesNothing();
            A.CallTo(() => _oddsServiceFake.GetOddTolerancePercentage(A<decimal>._,A<decimal>._)).Returns<decimal>(0m);
            // Act
            await _endpoint.HandleAsync(request, CancellationToken.None);

            // Assert
            A.CallTo(() => _betsServiceFake.CreateBet(1, 1, 1,1.5m, 50, ResultCode.Success)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _playersServiceFake.UpdatePlayerBalance(1, 50)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailed_WhenPlayerHasInsufficientBalance()
        {
            // Arrange
            var player = new Player { Id = 1, Balance = 10 };
            var eventObj = new Event
            {
                Id = 1,
                IsLive = false,
                Odds = new List<Odd> { new Odd { Id = 1, Value = 1.5m } }
            };
            var request = new PlaceBetRequest
            {
                PlayerId = 1,
                EventId = 1,
                OddId = 1,
                BetSum = 50,
                OddValue = 1.5m
            };

            A.CallTo(() => _playersServiceFake.GetPlayers()).Returns(new List<Player> { player });
            A.CallTo(() => _eventsServiceFake.GetEvents()).Returns(new List<Event> { eventObj });
            A.CallTo(() => _oddsServiceFake.GetOddTolerancePercentage(A<decimal>._, A<decimal>._)).Returns<decimal>(0m);
            // Act
            try
            {
                await _endpoint.HandleAsync(request, CancellationToken.None);
                var rsp = _endpoint.Response;
            }
            catch (Exception)
            {

            }
            
            // Assert
            A.CallTo(() => _betsServiceFake.CreateBet(1, 1, 1,1.5m, 50, ResultCode.Failed)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _playersServiceFake.UpdatePlayerBalance(A<int>._, A<decimal>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailed_WhenBetSumIsInvalid()
        {
            // Arrange
            var player = new Player { Id = 1, Balance = 100 };
            var eventObj = new Event
            {
                Id = 1,
                IsLive = false,
                Odds = new List<Odd> { new Odd { Id = 1, Value = 1.5m } }
            };
            var request = new PlaceBetRequest
            {
                PlayerId = 1,
                EventId = 1,
                OddId = 1,
                BetSum = -10, // Invalid BetSum
                OddValue = 1.5m
            };

            A.CallTo(() => _playersServiceFake.GetPlayers()).Returns(new List<Player> { player });
            A.CallTo(() => _eventsServiceFake.GetEvents()).Returns(new List<Event> { eventObj });
            A.CallTo(() => _oddsServiceFake.GetOddTolerancePercentage(A<decimal>._, A<decimal>._)).Returns<decimal>(0m);
            // Act
            try
            {
                await _endpoint.HandleAsync(request, CancellationToken.None);
            }
            catch (Exception)
            {

            }

            // Assert
            A.CallTo(() => _betsServiceFake.CreateBet(A<int>._, A<int>._, A<int>._, A<decimal>._, A<decimal>._, ResultCode.Failed)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _playersServiceFake.UpdatePlayerBalance(A<int>._, A<decimal>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailed_WhenOddValueOutOfToleranceLiveEvent()
        {
            // Arrange
            var player = new Player { Id = 1, Balance = 100 };
            var eventObj = new Event
            {
                Id = 1,
                IsLive = true,
                Odds = new List<Odd> { new Odd { Id = 1, Value = 1.5m } }
            };
            var request = new PlaceBetRequest
            {
                PlayerId = 1,
                EventId = 1,
                OddId = 1,
                BetSum = 10, 
                OddValue = 3m //invalid odd value
            };

            A.CallTo(() => _playersServiceFake.GetPlayers()).Returns(new List<Player> { player });
            A.CallTo(() => _eventsServiceFake.GetEvents()).Returns(new List<Event> { eventObj });
            A.CallTo(() => _oddsServiceFake.GetOddTolerancePercentage(A<decimal>._, A<decimal>._)).Returns<decimal>(100m);
            // Act
            try
            {
                await _endpoint.HandleAsync(request, CancellationToken.None);
            }
            catch (Exception)
            {

            }

            // Assert
            A.CallTo(() => _betsServiceFake.CreateBet(A<int>._, A<int>._, A<int>._, A<decimal>._, A<decimal>._, ResultCode.Failed)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _playersServiceFake.UpdatePlayerBalance(A<int>._, A<decimal>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailed_WhenOddValueOutOfToleranceNotLiveEvent()
        {
            // Arrange
            var player = new Player { Id = 1, Balance = 100 };
            var eventObj = new Event
            {
                Id = 1,
                IsLive = false,
                Odds = new List<Odd> { new Odd { Id = 1, Value = 1.5m } }
            };
            var request = new PlaceBetRequest
            {
                PlayerId = 1,
                EventId = 1,
                OddId = 1,
                BetSum = 10,
                OddValue = 3m //invalid odd value
            };

            A.CallTo(() => _playersServiceFake.GetPlayers()).Returns(new List<Player> { player });
            A.CallTo(() => _eventsServiceFake.GetEvents()).Returns(new List<Event> { eventObj });
            A.CallTo(() => _oddsServiceFake.GetOddTolerancePercentage(A<decimal>._, A<decimal>._)).Returns<decimal>(100m);
            // Act
            try
            {
                await _endpoint.HandleAsync(request, CancellationToken.None);
            }
            catch (Exception)
            {

            }

            // Assert
            A.CallTo(() => _betsServiceFake.CreateBet(A<int>._, A<int>._, A<int>._, A<decimal>._, A<decimal>._, ResultCode.Failed)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _playersServiceFake.UpdatePlayerBalance(A<int>._, A<decimal>._)).MustNotHaveHappened();
        }
    }
}
