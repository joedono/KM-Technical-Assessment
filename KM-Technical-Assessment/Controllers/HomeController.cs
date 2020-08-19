namespace KM_Technical_Assessment.Controllers
{
    using KM_Technical_Assessment.Models;
    using KM_Technical_Assessment.Services;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [ApiController]
    public class HomeController : Controller
    {
        #region Fields

        private IGameBoardService gameBoardService;

        #endregion

        #region Constructor

        public HomeController(IGameBoardService gameBoardService)
        {
            this.gameBoardService = gameBoardService;
        }

        #endregion

        #region Actions

        /// <summary>
        /// Endpoint to initialize the board
        /// </summary>
        /// <returns>Initialization success message</returns>
        [HttpGet]
        [Route("initialize")]
        public KMResponse Initialize(int boardSize = 4)
        {
            this.gameBoardService.Initialize(boardSize);

            return new KMResponse
            {
                msg = "Initialize",
                body = new KMResponseBody
                {
                    heading = "Player 1",
                    message = "Awaiting Player 1's Move."
                }
            };
        }

        /// <summary>
        /// Endpoint for when a user clicks a point on the board.
        /// Validates that the move is valid by examining existing board,
        /// adds the clicked point (and points between it and the previously clicked point) to the board,
        /// and checks whether there's a winner.
        /// </summary>
        /// <param name="point">Point on the board that was clicked</param>
        /// <returns>Valid, invalid, or game over response</returns>
        [HttpPost]
        [Route("node-clicked")]
        public KMResponse NodeClicked([FromBody] KMPoint point)
        {
            var valid = this.Validate(point);

            if (valid != null)
            {
                this.gameBoardService.SetPreviousNode(null);
                return valid;
            }

            var response = this.ClickNode(point);

            if (this.CheckWinner())
            {
                var currentPlayer = this.gameBoardService.GetCurrentPlayer();

                response.msg = "GAME_OVER";
                response.body.heading = "Game Over";
                response.body.message = $"Player {currentPlayer} Wins!";
            }

            return response;
        }

        /// <summary>
        /// Endpoint for the client to report errors.
        /// </summary>
        /// <param name="error">Reported error</param>
        [HttpPost]
        [Route("error")]
        public void Error([FromBody] KMError error)
        {
            Console.WriteLine(error.error);
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validates that the clicked node is a valid move.
        /// </summary>
        /// <param name="newNode">The clicked node</param>
        /// <returns>A <see cref="KMResponse"/> if the choice was invalid, null if the choice was valid</returns>
        private KMResponse Validate(KMPoint newNode)
        {
            var currentNodes = this.gameBoardService.GetGameBoard();
            var currentPlayer = this.gameBoardService.GetCurrentPlayer();
            var boardDimension = this.gameBoardService.GetBoardDimension();
            var previousNode = this.gameBoardService.GetPreviousNode();
            var playerString = $"Player {currentPlayer}";
            var errorMsg = previousNode == null ? "INVALID_START_NODE" : "INVALID_END_NODE";

            // Validate that the point is on the board
            if (newNode.x < 0 || newNode.y < 0 || newNode.x >= boardDimension || newNode.y >= boardDimension)
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
                    if (!newNode.Equals(currentNodes.nodes.First()) && !newNode.Equals(currentNodes.nodes.Last()))
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
                // Picking the second node. Node must be adjacent to or on a direct line from previous node
                if (currentNodes.nodes.Contains(newNode))
                {
                    // Node picked is already on the path. Also covers if previous node is clicked twice
                    return new KMResponse
                    {
                        msg = errorMsg,
                        body = new KMResponseBody
                        {
                            heading = playerString,
                            message = "Invalid move."
                        }
                    };
                }

                var xDifference = Math.Abs(previousNode.x - newNode.x);
                var yDifference = Math.Abs(previousNode.y - newNode.y);

                if (xDifference > 0 && yDifference > 0 && xDifference != yDifference)
                {
                    // A straight line cannot be drawn from prevous node to new node
                    return new KMResponse
                    {
                        msg = errorMsg,
                        body = new KMResponseBody
                        {
                            heading = playerString,
                            message = "Invalid move."
                        }
                    };
                }

                if (xDifference > 1 || yDifference > 1)
                {
                    // Nodes are more than one space apart
                    // Check the in-between nodes
                    // If they're on the path, it's an invalid move
                    var pathNodes = this.GetNodePath(previousNode, newNode);

                    foreach(var pathNode in pathNodes)
                    {
                        if (currentNodes.nodes.Contains(pathNode))
                        {
                            // Line would intersect with existing path.
                            return new KMResponse
                            {
                                msg = errorMsg,
                                body = new KMResponseBody
                                {
                                    heading = playerString,
                                    message = "Invalid move."
                                }
                            };
                        }
                    }
                }
            }

            return null;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Processes the clicked node.
        /// If this is the first click, record it as PreviousNode.
        /// If this is the second click, add it and any nodes on the path
        /// to PreviousNode to the game board and switch player turns.
        /// This method assumes that validation has already been run on the clicked node
        /// and that it is a valid move.
        /// </summary>
        /// <param name="clickedNode">The clicked node</param>
        /// <returns>Either a VALID_START_NODE response or a VALID_END_NODE response.</returns>
        private KMResponse ClickNode(KMPoint clickedNode)
        {
            var currentNodes = this.gameBoardService.GetGameBoard();
            var currentPlayer = this.gameBoardService.GetCurrentPlayer();
            var previousNode = this.gameBoardService.GetPreviousNode();
            var playerString = $"Player {currentPlayer}";
            var nextPlayerString = $"Player {(currentPlayer == 1 ? 2 : 1)}";

            if (previousNode == null)
            {
                // First click
                this.gameBoardService.SetPreviousNode(clickedNode);
                return new KMResponse
                {
                    msg = "VALID_START_NODE",
                    body = new KMResponseBody
                    {
                        heading = playerString,
                        message = "Select a second node to complete the line."
                    }
                };
            }
            else
            {
                // Second Click
                if (!currentNodes.nodes.Any())
                {
                    // First Move. Make sure to add the previousNode
                    currentNodes.nodes.Add(previousNode);
                }

                // Add in-between nodes
                var addToBeginning = previousNode.Equals(currentNodes.nodes.First());
                var pathNodes = this.GetNodePath(previousNode, clickedNode);

                foreach (var pathNode in pathNodes)
                {
                    if (addToBeginning)
                    {
                        currentNodes.nodes.Insert(0, pathNode);
                    }
                    else
                    {
                        currentNodes.nodes.Add(pathNode);
                    }
                }

                this.ChangePlayer(currentNodes, currentPlayer);
                return new KMResponse
                {
                    msg = "VALID_END_NODE",
                    body = new KMResponseBody
                    {
                        newLine = new
                        {
                            start = previousNode,
                            end = clickedNode
                        },
                        heading = nextPlayerString
                    }
                };
            }
        }

        /// <summary>
        /// Saves the current board to memory and changes which player's turn it is
        /// </summary>
        /// <param name="board">The current board</param>
        /// <param name="currentPlayer">The player whose turn just ended</param>
        private void ChangePlayer(KMGameBoard board, int currentPlayer)
        {
            this.gameBoardService.SetGameBoard(board);
            this.gameBoardService.SetCurrentPlayer(currentPlayer == 1 ? 2 : 1);
            this.gameBoardService.SetPreviousNode(null);
        }

        /// <summary>
        /// Returns a list of <see cref="KMPoint"/> representing the nodes between
        /// previousNode and the clicked node.
        /// List includes the clicked node but not previousNode, since previousNode
        /// only needs to be added on the very first move of the game, which is taken care
        /// of in <see cref="ClickNode(KMPoint)"/>
        /// </summary>
        /// <param name="start">The newly clicked node</param>
        /// <param name="end">The previousNode</param>
        /// <returns>List of nodes drawing a path between start and end</returns>
        private List<KMPoint> GetNodePath(KMPoint start, KMPoint end)
        {
            var path = new List<KMPoint>();

            var xDirection = Math.Clamp(end.x - start.x, -1, 1);
            var yDirection = Math.Clamp(end.y - start.y, -1, 1);

            if (xDirection == 0 && yDirection == 0)
            {
                throw new ArgumentException("Attempted to get path from same node");
            }

            var xCheck = start.x + xDirection;
            var yCheck = start.y + yDirection;

            while (xCheck != end.x || yCheck != end.y)
            {
                path.Add(new KMPoint(xCheck, yCheck));
                xCheck += xDirection;
                yCheck += yDirection;
            }

            path.Add(end);

            return path;
        }

        /// <summary>
        /// Checks the win state of the board.
        /// </summary>
        /// <returns>
        /// True if there are no available moves and someone has won.
        /// False if there are available moves and the game goes on
        /// </returns>
        private bool CheckWinner()
        {
            var currentNodes = this.gameBoardService.GetGameBoard();

            if (!currentNodes.nodes.Any())
            {
                // Empty board
                return false;
            }

            // Check first and last node for available moves
            var hasAvailableMoves = this.CheckNode(currentNodes.nodes.First(), currentNodes) ||
                this.CheckNode(currentNodes.nodes.Last(), currentNodes);

            return !hasAvailableMoves;
        }

        /// <summary>
        /// Checks if a move can be made from a given node.
        /// A move can be made if there are any nodes immediately
        /// next to the given node that are not already on the board.
        /// </summary>
        /// <param name="point">The node to be checked</param>
        /// <param name="board">The current game board</param>
        /// <returns>
        /// True if there are available moves from the given node.
        /// Falst if there are no available moves from the given node.
        /// </returns>
        private bool CheckNode(KMPoint point, KMGameBoard board)
        {
            var boardDimension = this.gameBoardService.GetBoardDimension();
            var moveAvailable = false;
            KMPoint checkpoint;

            for (var x = point.x - 1; x <= point.x + 1; x++)
            {
                for (var y = point.y - 1; y <= point.y + 1; y++)
                {
                    // Point is off the board towards the top or left
                    if (x < 0 || y < 0)
                    {
                        continue;
                    }

                    // Point is off the board towards the bottom or right
                    if (x >= boardDimension || y >= boardDimension)
                    {
                        continue;
                    }

                    checkpoint = new KMPoint(x, y);
                    if (!board.nodes.Contains(checkpoint)) // Point is not on the path and can be drawn to
                    {
                        moveAvailable = true;
                    }
                }
            }

            return moveAvailable;
        }

        #endregion
    }
}
