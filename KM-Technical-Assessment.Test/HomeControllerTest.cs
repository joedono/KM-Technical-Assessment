namespace KM_Technical_Assessment.Test
{
    using KM_Technical_Assessment.Controllers;
    using KM_Technical_Assessment.Models;
    using KM_Technical_Assessment.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;

    [TestClass]
    public class HomeControllerTest
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
        public void INVALID_SecondMove_AlreadyOhPath()
        {
            var node = new KMPoint(2, 3);
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
            Assert.AreEqual("Invalid move. You cannot add a node to the path that is already on it.", response.body.message);
            this.service.Verify(m => m.SetGameBoard(It.IsAny<KMGameBoard>()), Times.Never);
        }

        [TestMethod]
        public void INVALID_SecondMove_CannotConnect()
        {
            var node = new KMPoint(3, 0);
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
            Assert.AreEqual("Invalid move. Line cannot be drawn between selected nodes.", response.body.message);
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

        [TestMethod]
        public void VALID_FirstMove_Empty()
        {
            var node = new KMPoint(1, 1);
            var gameBoard = new KMGameBoard();

            this.service.Setup(m => m.GetGameBoard()).Returns(gameBoard);

            var response = this.controller.NodeClicked(node);

            Assert.AreEqual("VALID_START_NODE", response.msg);
            Assert.AreEqual("Select a second node to complete the line.", response.body.message);
            this.service.Verify(m => m.SetPreviousNode(It.IsAny<KMPoint>()), Times.Once);
            this.service.Verify(m => m.SetGameBoard(It.IsAny<KMGameBoard>()), Times.Never);
        }

        [TestMethod]
        public void VALID_FirstMove_PathStart()
        {
            var node = new KMPoint(0, 1);
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

            var response = this.controller.NodeClicked(node);

            Assert.AreEqual("VALID_START_NODE", response.msg);
            Assert.AreEqual("Select a second node to complete the line.", response.body.message);
            this.service.Verify(m => m.SetPreviousNode(It.IsAny<KMPoint>()), Times.Once);
            this.service.Verify(m => m.SetGameBoard(It.IsAny<KMGameBoard>()), Times.Never);
        }

        [TestMethod]
        public void VALID_FirstMove_PathEnd()
        {
            var node = new KMPoint(1, 3);
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

            var response = this.controller.NodeClicked(node);

            Assert.AreEqual("VALID_START_NODE", response.msg);
            Assert.AreEqual("Select a second node to complete the line.", response.body.message);
            this.service.Verify(m => m.SetPreviousNode(It.IsAny<KMPoint>()), Times.Once);
            this.service.Verify(m => m.SetGameBoard(It.IsAny<KMGameBoard>()), Times.Never);
        }

        [TestMethod]
        public void VALID_FirstMove_NextToPreviousFromStart()
        {
            var node = new KMPoint(0, 0);
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

            var expectedNodes = new List<KMPoint>
            {
                new KMPoint(0, 0),
                new KMPoint(0, 1),
                new KMPoint(0, 2),
                new KMPoint(1, 2),
                new KMPoint(2, 2),
                new KMPoint(2, 3),
                new KMPoint(1, 3)
            };

            var expectedGameBoard = new KMGameBoard
            {
                nodes = expectedNodes
            };

            this.service.Setup(m => m.GetGameBoard()).Returns(gameBoard);
            this.service.Setup(m => m.GetPreviousNode()).Returns(new KMPoint(0, 1));

            var response = this.controller.NodeClicked(node);

            Assert.AreEqual("VALID_END_NODE", response.msg);
            Assert.IsNull(response.body.message);
            this.service.Verify(m => m.SetGameBoard(It.Is<KMGameBoard>(board => board.Equals(expectedGameBoard))), Times.Once);
        }

        [TestMethod]
        public void VALID_FirstMove_NextToPreviousFromEnd()
        {
            var node = new KMPoint(0, 3);
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

            var expectedNodes = new List<KMPoint>
            {
                new KMPoint(0, 1),
                new KMPoint(0, 2),
                new KMPoint(1, 2),
                new KMPoint(2, 2),
                new KMPoint(2, 3),
                new KMPoint(1, 3),
                new KMPoint(0, 3)
            };

            var expectedGameBoard = new KMGameBoard
            {
                nodes = expectedNodes
            };

            this.service.Setup(m => m.GetGameBoard()).Returns(gameBoard);
            this.service.Setup(m => m.GetPreviousNode()).Returns(new KMPoint(1, 3));

            var response = this.controller.NodeClicked(node);

            Assert.AreEqual("VALID_END_NODE", response.msg);
            Assert.IsNull(response.body.message);
            this.service.Verify(m => m.SetGameBoard(It.Is<KMGameBoard>(board => board.Equals(expectedGameBoard))), Times.Once);
        }

        [TestMethod]
        public void VALID_FirstMove_LongPath()
        {
            var node = new KMPoint(3, 3);
            var nodes = new List<KMPoint>
            {
                new KMPoint(0, 0),
                new KMPoint(0, 1),
                new KMPoint(0, 2),
                new KMPoint(1, 2),
                new KMPoint(1, 1)
            };

            var gameBoard = new KMGameBoard
            {
                nodes = nodes
            };

            var expectedNodes = new List<KMPoint>
            {
                new KMPoint(0, 0),
                new KMPoint(0, 1),
                new KMPoint(0, 2),
                new KMPoint(1, 2),
                new KMPoint(1, 1),
                new KMPoint(2, 2),
                new KMPoint(3, 3)
            };

            var expectedGameBoard = new KMGameBoard
            {
                nodes = expectedNodes
            };

            this.service.Setup(m => m.GetGameBoard()).Returns(gameBoard);
            this.service.Setup(m => m.GetPreviousNode()).Returns(new KMPoint(1, 1));

            var response = this.controller.NodeClicked(node);

            Assert.AreEqual("VALID_END_NODE", response.msg);
            Assert.IsNull(response.body.message);
            this.service.Verify(m => m.SetGameBoard(It.Is<KMGameBoard>(board => board.Equals(expectedGameBoard))), Times.Once);
        }

        #endregion
    }
}
