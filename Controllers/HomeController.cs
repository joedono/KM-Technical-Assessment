namespace KM_Technical_Assessment.Controllers
{
    using KM_Technical_Assessment.Constants;
    using KM_Technical_Assessment.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using System;

    [ApiController]
    public class HomeController : Controller
    {
        #region Fields

        private IMemoryCache memoryCache;

        #endregion

        #region Constructor

        public HomeController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        #endregion

        #region Actions

        [HttpGet]
        [Route("initialize")]
        public KMResponse Initialize()
        {
            this.memoryCache.Set(CacheKeys.GameBoard, new KMGameBoard());
            this.memoryCache.Set(CacheKeys.CurrentPlayer, 1);
            this.memoryCache.Set(CacheKeys.CurrentTurn, 1);
            this.memoryCache.Set<KMPoint>(CacheKeys.PreviousNode, null);
            this.memoryCache.Set(CacheKeys.BoardDimension, 4);

            return new KMResponse
            {
                msg = "Initialize",
                body = new KMResponseBody
                {
                    heading = "Player 1",
                    message = "Awaiting Player 1's Move"
                }
            };
        }

        [HttpPost]
        [Route("node-clicked")]
        public KMResponse NodeClicked([FromBody] KMPoint point)
        {
            // TODO Validate clicked node, add selected nodes to board, check winner
            var valid = this.Validate(point);

            if (valid != null)
            {
                this.memoryCache.Set<KMPoint>(CacheKeys.PreviousNode, null);
                return valid;
            }

            var response = this.AddNodeToBoard(point);

            if (this.CheckWinner())
            {
                var currentPlayer = this.memoryCache.Get<int>(CacheKeys.CurrentPlayer);

                response.msg = "GAME_OVER";
                response.body.heading = "Game Over";
                response.body.message = $"Player {currentPlayer} Wins!";
            }

            return response;
        }

        [HttpPost]
        [Route("error")]
        public void Error([FromBody] KMError error)
        {
            Console.WriteLine(error.error);
        }

        #endregion

        #region Helpers

        private KMResponse Validate(KMPoint point)
        {
            return null;
        }

        private KMResponse AddNodeToBoard(KMPoint point)
        {
            return null;
        }

        private bool CheckWinner()
        {
            return false;
        }

        #endregion
    }
}
