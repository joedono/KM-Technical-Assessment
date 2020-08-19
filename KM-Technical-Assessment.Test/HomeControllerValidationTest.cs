namespace KM_Technical_Assessment.Test
{
    using KM_Technical_Assessment.Controllers;
    using KM_Technical_Assessment.Models;
    using KM_Technical_Assessment.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class HomeControllerValidationTest
    {
        #region Fields

        private HomeController controller;

        private Mock<IGameBoardService> service;

        #endregion

        #region Initialize

        [TestInitialize]
        public void Initialize()
        {
            this.service = new Mock<IGameBoardService>();
            this.controller = new HomeController(this.service.Object);

            this.service.Setup(m => m.GetGameBoard()).Returns(() => new KMGameBoard());
            this.service.Setup(m => m.GetBoardDimension()).Returns(() => 4);
            this.service.Setup(m => m.GetCurrentPlayer()).Returns(() => 1);
            this.service.Setup(m => m.GetPreviousNode()).Returns(() => null);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void INVALID_ClickOutsideBoard()
        {
            var node = new KMPoint(-1, -1);
            var response = this.controller.NodeClicked(node);
            Assert.AreEqual("INVALID_START_NODE", response.msg);
            this.service.Verify(m => m.SetGameBoard(It.IsAny<KMGameBoard>()), Times.Never);
        }

        #endregion
    }
}
