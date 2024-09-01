using FastEndpoints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;

namespace KickerTech.TaskApi.Endpoints.PlaceBet
{
    public class GetBetsResponse 
    {
        public int? BetId { get; set; }
        public int EventId { get; set; }
        public int OddId { get; set; }
        public string ResultCode { get; set; }
    }
}
