using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V116.Network;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAufgabe1.Tests.Amazon.PageObjects
{
    internal class AmazonPageObject
    {
        private readonly string amazonPageUri = "https://www.amazon.de/";
        private readonly IWebDriver _webDriver;
        private readonly TimeSpan defaultWaitTime = TimeSpan.FromSeconds(15);

        private bool wereCookiesAccepted = false;
        public readonly string amazonPageTitle = "Amazon.de: Günstige Preise für Elektronik & Foto, Filme, Musik, Bücher, Games, Spielzeug & mehr";

        public AmazonPageObject()
        {
            _webDriver = new ChromeDriver();
        }

        public string GetCurrentTitle() => _webDriver.Title;
        public void MaximizeWindow() => _webDriver.Manage().Window.Maximize();
        public void QuitAndCloseBrowser() => _webDriver.Quit();

        public IWebElement GetAcceptCookiesButton() => _webDriver.FindElement(By.XPath("//input[@id=\"sp-cc-accept\"]"));
        public IWebElement GetSearchInput() => _webDriver.FindElement(By.XPath("//input[@id=\"twotabsearchtextbox\"]"));
        public IWebElement GetSearchButton() => _webDriver.FindElement(By.XPath("//input[@id=\"nav-search-submit-button\"]"));
        
        public IEnumerable<IWebElement> GetFoundShoes() => _webDriver.FindElements(By.XPath("//div[@data-component-type=\"s-search-result\"]"));
        public string GetShoesBrandText(IWebElement shoe) => shoe.FindElement(By.XPath("//span[@class=\"a-size-base-plus a-color-base\"]")).Text;
        public string GetShoesNameText(IWebElement shoe) => shoe.FindElement(By.XPath("//span[@class=\"a-size-base-plus a-color-base a-text-normal\"]")).Text;
        public string GetShoesTitle(IWebElement shoe) => GetShoesBrandText(shoe) + " " + GetShoesNameText(shoe);

        public void OpenShoesProductPage(IWebElement shoe) => shoe.Click();
        public string GetShoesProductTitle() => _webDriver.FindElement(By.XPath("//*[@id=\"productTitle\"]")).Text;

        public IWebElement GetSizeSelectElement() => _webDriver.FindElement(By.XPath("//select[@id=\"native_dropdown_selected_size_name\"]"));
        public IWebElement GetAddToCartButton() => _webDriver.FindElement(By.XPath("//input[@id=\"add-to-cart-button\"]"));

        public IWebElement GetSuccessfullyAddedToCartMessage() => _webDriver.FindElement(By.XPath("//*[@id=\"NATC_SMART_WAGON_CONF_MSG_SUCCESS\"]/span"));

        public IWebElement GetCartButton() => _webDriver.FindElement(By.XPath("//a[@id=\"nav-cart\"]"));
        public IEnumerable<IWebElement> GetCartItems() => _webDriver.FindElements(By.XPath("//div[@class=\"a-row sc-list-item sc-java-remote-feature\"]"));

        public void WaitUntil(Func<IWebDriver, IWebElement> func)
        {
            WebDriverWait wait = new WebDriverWait(_webDriver, defaultWaitTime);
            wait.Until(func);
        }

        public void SearchProducts(string query, IWebElement searchInput, IWebElement searchButton)
        {
            searchInput.SendKeys(query);
            searchButton.Click();
        }

        public void OpenCart() => GetCartButton().Click();    



        public void OpenAmazon_Success()
        {
            //Arrange
            string expectedTitle = this.amazonPageTitle;

            //Act
            _webDriver.Navigate().GoToUrl(amazonPageUri);
            if (!wereCookiesAccepted)
            {
                WaitUntil((WebDriver) => GetAcceptCookiesButton());

                IWebElement cookiesAcceptButton = GetAcceptCookiesButton();
                cookiesAcceptButton.Click();
                wereCookiesAccepted = true;
            }

            //Assert
            Assert.That(this.GetCurrentTitle(), Is.EqualTo(expectedTitle));
        }

        public void SearchShoes_MinumumNShoes_Success(string productName, int minCount)
        {
            //Arrange
            IWebElement searchInput = this.GetSearchInput();
            IWebElement searchButton = this.GetSearchButton();

            //Act
            this.SearchProducts(productName, searchInput, searchButton);

            //Assert
            int shoesCount = this.GetFoundShoes().Count();
            Assert.That(shoesCount, Is.GreaterThanOrEqualTo(minCount));
        }

        public void SearchShoes_OpenFirstShoesLink_Success()
        {
            //Arrange
            IWebElement firstShoe = this.GetFoundShoes().First();
            string expectedProductTitle = this.GetShoesTitle(firstShoe);

            //Act
            this.OpenShoesProductPage(firstShoe);

            //Assert
            string actualProductTitle = this.GetShoesProductTitle();
            Assert.That(expectedProductTitle, Is.EqualTo(actualProductTitle));
        }

        public void ChooseAnySizeAndAddToCart_Success()
        {
            //Arrange
            IWebElement selectWebElement = this.GetSizeSelectElement();

            SelectElement selectElement = new SelectElement(selectWebElement);

            //Act
            IEnumerable<IWebElement> options = selectElement.Options.Where(m => m.Enabled);
            int randomIndex = Random.Shared.Next(0, options.Count());
            IWebElement chosenOption = options.ElementAt(randomIndex);
            selectElement.SelectByText(chosenOption.Text);

            this.WaitUntil(driver => driver.FindElement(By.XPath("//div[@id=\"qualifiedBuybox\"]")));

            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    this.GetAddToCartButton().Click();
                    break;
                }
                catch (StaleElementReferenceException) { }
            }

            //Assert
            Assert.DoesNotThrow(() => this.GetSuccessfullyAddedToCartMessage());
        }
    }
}
