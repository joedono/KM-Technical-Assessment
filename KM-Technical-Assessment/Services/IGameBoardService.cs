namespace KM_Technical_Assessment.Services
{
    using KM_Technical_Assessment.Models;

    public interface IGameBoardService
    {
        #region Initialize

        void Initialize(int boardSize);

        #endregion

        #region Getters

        int GetCurrentPlayer();

        KMGameBoard GetGameBoard();

        int GetBoardDimension();

        KMPoint GetPreviousNode();

        #endregion

        #region Setters

        void SetPreviousNode(KMPoint node);

        void SetGameBoard(KMGameBoard board);

        void SetCurrentPlayer(int currentPlayer);

        #endregion
    }
}
