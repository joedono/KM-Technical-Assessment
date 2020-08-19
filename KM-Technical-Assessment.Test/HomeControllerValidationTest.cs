namespace KM_Technical_Assessment.Test
{
    using KM_Technical_Assessment.Constants;
    using KM_Technical_Assessment.Controllers;
    using KM_Technical_Assessment.Models;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class HomeControllerValidationTest
    {
        #region Fields

        private HomeController controller;

        private Mock<IMemoryCache> memoryCache;

        #endregion

        #region Initialize

        [TestInitialize]
        public void Initialize()
        {
            this.memoryCache = new Mock<IMemoryCache>();
            this.controller = new HomeController(this.memoryCache.Object);

            this.memoryCache.Setup(m => m.Get(It.Is<string>(s => s == CacheKeys.GameBoard))).Returns(() => new KMGameBoard());
            this.memoryCache.Setup(m => m.Get(It.Is<string>(s => s == CacheKeys.BoardDimension))).Returns(() => 4);
            this.memoryCache.Setup(m => m.Get(It.Is<string>(s => s == CacheKeys.CurrentPlayer))).Returns(() => 1);
            this.memoryCache.Setup(m => m.Get(It.Is<string>(s => s == CacheKeys.PreviousNode))).Returns(() => null);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void INVALID_ClickOutsideBoard()
        {
            var node = new KMPoint(-1, -1);
            var response = this.controller.NodeClicked(node);
            Assert.AreEqual("INVALID_START_NODE", response.msg);
            this.memoryCache.Verify(m => m.Set(It.Is<string>(s => s == CacheKeys.GameBoard), It.IsAny<KMGameBoard>()), Times.Never);
        }

        #endregion
    }
}
