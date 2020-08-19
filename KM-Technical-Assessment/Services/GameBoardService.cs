namespace KM_Technical_Assessment.Services
{
    using KM_Technical_Assessment.Constants;
    using KM_Technical_Assessment.Models;
    using Microsoft.Extensions.Caching.Memory;

    public class GameBoardService : IGameBoardService
    {
        #region Fields

        private IMemoryCache memoryCache;

        #endregion

        #region Constructor

        public GameBoardService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        #endregion

        #region Initialize

        public void Initialize(int boardSize)
        {
            this.memoryCache.Set(CacheKeys.GameBoard, new KMGameBoard());
            this.memoryCache.Set(CacheKeys.CurrentPlayer, 1);
            this.memoryCache.Set<KMPoint>(CacheKeys.PreviousNode, null);
            this.memoryCache.Set(CacheKeys.BoardDimension, boardSize);
        }

        #endregion

        #region Getters

        public int GetCurrentPlayer()
        {
            return this.memoryCache.Get<int>(CacheKeys.CurrentPlayer);
        }

        public KMGameBoard GetGameBoard()
        {
            return this.memoryCache.Get<KMGameBoard>(CacheKeys.GameBoard);
        }

        public int GetBoardDimension()
        {
            return this.memoryCache.Get<int>(CacheKeys.BoardDimension);
        }

        public KMPoint GetPreviousNode()
        {
            return this.memoryCache.Get<KMPoint>(CacheKeys.PreviousNode);
        }

        #endregion

        #region Setters

        public void SetPreviousNode(KMPoint node)
        {
            this.memoryCache.Set(CacheKeys.PreviousNode, node);
        }

        public void SetGameBoard(KMGameBoard board)
        {
            this.memoryCache.Set(CacheKeys.GameBoard, board);
        }

        public void SetCurrentPlayer(int currentPlayer)
        {
            this.memoryCache.Set(CacheKeys.CurrentPlayer, currentPlayer);
        }

        #endregion
    }
}
