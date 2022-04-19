using Microsoft.AspNetCore.Mvc;
using SimpleTrader.Domain.Services;
using StockTrader.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockTrader.API.Controllers
{
    [ApiController]
    public class MajorIndexesController : ControllerBase
    {
        private readonly IMajorIndexService _majorIndexService;

        public MajorIndexesController(
            IMajorIndexService majorIndexService)
        {
            _majorIndexService = majorIndexService;
        }

        [HttpGet("major-index/get-all")]
        public async Task<IEnumerable<MajorIndex>> GetAllIndexesAsync()
        {
            return await Task.WhenAll(
                Enum.GetValues<MajorIndexType>()
                .Select(async t => await _majorIndexService.GetMajorIndex(t)));
        }

        [HttpGet("major-index/dow-jones")]
        public async Task<MajorIndex> GetDowJonesAsync()
        {
            return await _majorIndexService.GetMajorIndex(MajorIndexType.DowJones);
        }

        [HttpGet("major-index/nasdaq")]
        public async Task<MajorIndex> GetNasdaqAsync()
        {
            return await _majorIndexService.GetMajorIndex(MajorIndexType.Nasdaq);
        }

        [HttpGet("major-index/sp500")]
        public async Task<MajorIndex> GetSP500Async()
        {
            return await _majorIndexService.GetMajorIndex(MajorIndexType.SP500);
        }
    }
}
