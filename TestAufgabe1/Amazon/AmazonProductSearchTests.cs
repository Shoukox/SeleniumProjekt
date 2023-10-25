using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAufgabe1.Tests.Amazon.PageObjects;

//Testfall 1
namespace TestAufgabe1.Tests.Amazon
{
    internal class AmazonProductSearchTests
    {
        private readonly AmazonPageObject _amazonPageObject = new AmazonPageObject();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _amazonPageObject.MaximizeWindow();
        }

        [Order(0)]
        [Test]
        public void OpenAmazon_Success()
        {
            _amazonPageObject.OpenAmazon_Success();
        }

        [Order(1)]
        [Test]
        public void SearchShoes_Minumum5Shoes_Success()
        {
            string query = "Adidas Schuhe";
            int minCount = 5;
            _amazonPageObject.SearchShoes_MinumumNShoes_Success(query, minCount);
        }

        [Order(2)]
        [Test]
        public void SearchShoes_OpenFirstShoesLink_Success()
        {
            _amazonPageObject.SearchShoes_OpenFirstShoesLink_Success();
        }

        [Order(3)]
        [Test]
        public void ChooseAnySizeAndAddToCart_Success()
        {
            _amazonPageObject.ChooseAnySizeAndAddToCart_Success();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _amazonPageObject.QuitAndCloseBrowser();
        }
    }
}
