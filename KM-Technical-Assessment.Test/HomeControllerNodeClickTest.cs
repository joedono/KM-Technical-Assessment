namespace KM_Technical_Assessment.Test
{
    using KM_Technical_Assessment.Controllers;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class HomeControllerNodeClickTest
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
        }

        #endregion

        #region Tests
        
        [TestMethod]
        public void TestMethod1()
        {
        }

        #endregion
    }
}
