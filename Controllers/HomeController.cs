namespace KM_Technical_Assessment.Controllers
{
    using KM_Technical_Assessment.Constants;
    using KM_Technical_Assessment.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Linq;

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
            this.memoryCache.Set(CacheKeys.EnforceSize, false);

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

        #region Validation

        private KMResponse Validate(KMPoint newNode)
        {
            var currentNodes = this.memoryCache.Get<KMGameBoard>(CacheKeys.GameBoard);
            var currentPlayer = this.memoryCache.Get<int>(CacheKeys.CurrentPlayer);
            var currentTurn = this.memoryCache.Get<int>(CacheKeys.CurrentTurn);
            var boardDimension = this.memoryCache.Get<int>(CacheKeys.BoardDimension);
            var enforceSize = this.memoryCache.Get<bool>(CacheKeys.EnforceSize);
            var previousNode = this.memoryCache.Get<KMPoint>(CacheKeys.PreviousNode);
            var playerString = $"Player {currentPlayer}";
            var errorMsg = previousNode == null ? "INVALID_START_NODE" : "INVALID_END_NODE";

            // Validate that the point is on the board
            if (enforceSize && (newNode.x < 0 || newNode.y < 0 || newNode.x >= boardDimension || newNode.y >= boardDimension))
            {
                return new KMResponse
                {
                    msg = errorMsg,
                    body = new KMResponseBody
                    {
                        heading = playerString,
                        message = "Not a valid position."
                    }
                };
            }

            if (previousNode == null)
            {
                // Picking first node
                if (currentNodes.nodes.Any())
                {
                    if (!newNode.Equals(currentNodes.nodes[0]) && !newNode.Equals(currentNodes.nodes[currentNodes.nodes.Count - 1]))
                    {
                        // New node is not the first or last node in the path
                        return new KMResponse
                        {
                            msg = errorMsg,
                            body = new KMResponseBody
                            {
                                heading = playerString,
                                message = "You must start on either end of the path."
                            }
                        };
                    }
                }
                else
                {
                    // First move. Always valid
                    return null;
                }
            }
            else
            {

            }

            return null;
        }

        #endregion

        #region Helpers

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
