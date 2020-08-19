namespace KM_Technical_Assessment.Test
{
    using KM_Technical_Assessment.Controllers;
    using KM_Technical_Assessment.Models;
    using KM_Technical_Assessment.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;

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

            this.service.Setup(m => m.GetGameBoard()).Returns(new KMGameBoard());
            this.service.Setup(m => m.GetBoardDimension()).Returns(4);
            this.service.Setup(m => m.GetCurrentPlayer()).Returns(1);
            this.service.Setup(m => m.GetPreviousNode()).Returns<KMPoint>(null);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void INVALID_ClickOutsideBoard()
        {
            var node = new KMPoint(-1, -1);
            var response = this.controller.NodeClicked(node);

            Assert.AreEqual("INVALID_START_NODE", response.msg);
            Assert.AreEqual("Not a valid position.", response.body.message);
            this.service.Verify(m => m.SetGameBoard(It.IsAny<KMGameBoard>()), Times.Never);
        }

        [TestMethod]
        public void INVALID_FirstMove_ClickInsideOfPath()
        {
            var node = new KMPoint(0, 2);
            var nodes = new List<KMPoint>
            {
                new KMPoint(0, 1),
                new KMPoint(0, 2),
                new KMPoint(1, 2)
            };

            var gameBoard = new KMGameBoard
            {
                nodes = nodes
            };

            this.service.Setup(m => m.GetGameBoard()).Returns(gameBoard);

            var response = this.controller.NodeClicked(node);

            Assert.AreEqual("INVALID_START_NODE", response.msg);
            Assert.AreEqual("You must start on either end of the path.", response.body.message);
            this.service.Verify(m => m.SetGameBoard(It.IsAny<KMGameBoard>()), Times.Never);
        }

        [TestMethod]
        public void INVALID_FirstMove_ClickOutsideOfPath()
        {
            var node = new KMPoint(3, 3);
            var nodes = new List<KMPoint>
            {
                new KMPoint(0, 1),
                new KMPoint(0, 2),
                new KMPoint(1, 2)
            };

            var gameBoard = new KMGameBoard
            {
                nodes = nodes
            };

            this.service.Setup(m => m.GetGameBoard()).Returns(gameBoard);

            var response = this.controller.NodeClicked(node);

            Assert.AreEqual("INVALID_START_NODE", response.msg);
            Assert.AreEqual("You must start on either end of the path.", response.body.message);
            this.service.Verify(m => m.SetGameBoard(It.IsAny<KMGameBoard>()), Times.Never);
        }

        [TestMethod]
        public void INVALID_SecondMove_CrossPath()
        {
            var node = new KMPoint(1, 1);
            var nodes = new List<KMPoint>
            {
                new KMPoint(0, 1),
                new KMPoint(0, 2),
                new KMPoint(1, 2),
                new KMPoint(2, 2),
                new KMPoint(2, 3),
                new KMPoint(1, 3)
            };

            var gameBoard = new KMGameBoard
            {
                nodes = nodes
            };

            this.service.Setup(m => m.GetGameBoard()).Returns(gameBoard);
            this.service.Setup(m => m.GetPreviousNode()).Returns(new KMPoint(1, 3));

            var response = this.controller.NodeClicked(node);

            Assert.AreEqual("INVALID_END_NODE", response.msg);
            Assert.AreEqual("Invalid move. You cannot cross the path.", response.body.message);
            this.service.Verify(m => m.SetGameBoard(It.IsAny<KMGameBoard>()), Times.Never);
        }

        #endregion
    }
}
