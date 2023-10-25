using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAufgabe1.Tests.Amazon.PageObjects;

//Testfall 1
namespace TestAufgabe1.Tests.Amazon
{
    internal class AmazonProductSearchTestsWithDynamicParameters
    {
        private readonly AmazonPageObject _amazonPageObject = new AmazonPageObject();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _amazonPageObject.MaximizeWindow();
        }

        [Order(0)]
        [TestCase("Adidas Herren Questar Flow Laufschuhe", 1)]
        [TestCase("Puma Tazon 6", 1)]
        [TestCase("Nike Air Max", 1)]
        public void AddToCart_Success(string productName, int minCount)
        {
            _amazonPageObject.OpenAmazon_Success();
            _amazonPageObject.SearchShoes_MinumumNShoes_Success(productName, minCount);
            _amazonPageObject.SearchShoes_OpenFirstShoesLink_Success();
            _amazonPageObject.ChooseAnySizeAndAddToCart_Success();
        }

        [Order(1)]
        [Test]
        public void CheckCart_3Items_Success()
        {
            //Act
            _amazonPageObject.OpenCart();
            IEnumerable<IWebElement> cartItems = _amazonPageObject.GetCartItems();

            //Assert
            Assert.That(cartItems.Count(), Is.EqualTo(3));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _amazonPageObject.QuitAndCloseBrowser();
        }
    }
}
