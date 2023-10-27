using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V116.Network;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestAufgabe1.Tests.Amazon.PageObjects
{
    public class AmazonPageObject
    {
        private readonly string amazonPageUri = "https://www.amazon.de/";
        private readonly IWebDriver _webDriver;
        private readonly TimeSpan defaultWaitTime = TimeSpan.FromSeconds(20);

        private bool wereCookiesAccepted = false;

        public AmazonPageObject()
        {
            _webDriver = new ChromeDriver();
        }
        #region getters
        public string GetFooterText() => _webDriver.FindElement(By.XPath("//div[@id=\"navFooter\"]/div[5]/span")).Text;
        public string GetCurrentTitle() => _webDriver.Title;
        public void MaximizeWindow() => _webDriver.Manage().Window.Maximize();
        public void SetWindowSizeRandomly() => _webDriver.Manage().Window.Size = new System.Drawing.Size(Random.Shared.Next(1500, 1920), Random.Shared.Next(800, 1080));
        public void QuitAndCloseBrowser() => _webDriver.Quit();

        public IWebElement GetAcceptCookiesButton() => _webDriver.FindElement(By.XPath("//input[@id=\"sp-cc-accept\"]"));
        public IWebElement GetSearchInput() => _webDriver.FindElement(By.XPath("//input[@id=\"twotabsearchtextbox\"]"));
        public IWebElement GetSearchButton() => _webDriver.FindElement(By.XPath("//input[@id=\"nav-search-submit-button\"]"));

        public IEnumerable<IWebElement> GetFoundShoes() => _webDriver.FindElements(By.XPath("//div[@data-component-type=\"s-search-result\"]"));
        public IWebElement? GetUpdatedBuyBox()
        {
            IWebElement? element = null;
            try
            {
                element = _webDriver.FindElement(By.XPath("//div[@id=\"qualifiedBuybox\"]"));

            }
            catch (Exception) { }
            return element;
        }
        public string GetShoesBrandText(IWebElement shoe) => shoe.FindElement(By.XPath("//span[@class=\"a-size-base-plus a-color-base\"]")).Text;
        public string GetShoesNameText(IWebElement shoe) => shoe.FindElement(By.XPath("//span[@class=\"a-size-base-plus a-color-base a-text-normal\"]")).Text;
        public string GetShoesTitle(IWebElement shoe) => /*GetShoesBrandText(shoe) + " " + */GetShoesNameText(shoe);

        public void OpenShoesProductPage(IWebElement shoe) => shoe.Click();
        public string GetShoesProductTitle() => string.Join(" ", _webDriver.FindElement(By.XPath("//*[@id=\"productTitle\"]")).Text.Split(" ").Skip(1));

        public IWebElement? GetSizeSelectElement()
        {
            try
            {
                return _webDriver.FindElement(By.XPath("//select[@id=\"native_dropdown_selected_size_name\"]"));
            }
            catch (Exception) { return null; }
        }
        public IWebElement GetAddToCartButton() => _webDriver.FindElement(By.XPath("//input[@id=\"add-to-cart-button\"]"));

        public IWebElement GetSuccessfullyAddedToCartMessage() => _webDriver.FindElement(By.XPath("//*[@id=\"NATC_SMART_WAGON_CONF_MSG_SUCCESS\"]/span"));

        public IWebElement GetCartButton() => _webDriver.FindElement(By.XPath("//a[@id=\"nav-cart\"]"));
        public IEnumerable<IWebElement> GetCartItems() => _webDriver.FindElements(By.XPath("//div[@class=\"a-row sc-list-item sc-java-remote-feature\"]"));

        public void OpenCart() => GetCartButton().Click();
        #endregion
        #region helpmethods
        public IWebElement? WaitUntil(Func<IWebDriver, IWebElement?> func, TimeSpan? timeout = null)
        {
            WebDriverWait wait = new WebDriverWait(_webDriver, timeout ?? defaultWaitTime);
            wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));
            return wait.Until(func);
        }

        public void SearchProducts(string query, IWebElement searchInput, IWebElement searchButton)
        {
            searchInput.SendKeys(query);
            searchButton.Click();
        }
        public void OpenAmazon_Success()
        {
            //Arrange
            string expectedFooterPartText = $"©1996-{DateTime.Now.Year} Amazon.com";

            //Act
            _webDriver.Navigate().GoToUrl(amazonPageUri);

            if (wereCookiesAccepted)
                return;

            //falls captcha code ist angefordert => fail
            try
            {
                GetAcceptCookiesButton();
            }
            catch (Exception)
            {
                Console.WriteLine("No cookie-accept-button found. Possible reasons: Amazon require a captcha-verification");
                return;
                //Thread.Sleep(7000);
            }

            IWebElement cookiesAcceptButton = GetAcceptCookiesButton();
            cookiesAcceptButton.Click();
            wereCookiesAccepted = true;

            //Assert
            string actualFooterText = GetFooterText();
            Assert.That(actualFooterText.Contains(expectedFooterPartText));
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
            Assert.That(expectedProductTitle.Equals(actualProductTitle, StringComparison.InvariantCultureIgnoreCase));
        }

        public void ChooseAnySizeAndAddToCart_Success()
        {
            //Arrange
            IWebElement? selectWebElement = WaitUntil((driver) => this.GetSizeSelectElement())!;
            if (selectWebElement is not null)
            {
                SelectElement selectElement = new SelectElement(selectWebElement);

                //Act
                IEnumerable<IWebElement> options = selectElement.Options.Where((m, i) => m.Enabled && (i != 0));

                for (int i = 0; i <= options.Count() - 1; i++)
                {
                    int randomIndex = Random.Shared.Next(0, options.Count());
                    IWebElement chosenOption = options.ElementAt(randomIndex);
                    selectElement.SelectByText(chosenOption.Text);

                    IWebElement? updatedBuyBox = WaitUntil(driver => GetUpdatedBuyBox(), TimeSpan.FromSeconds(5));

                    //falls updatedBuyBox nicht vorhanden ist, eine andere Option aus selectList auswählen
                    if (updatedBuyBox is not null)
                        break;
                    else
                        options = options.Where((opt, ind) => ind != randomIndex);
                }
            }

            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    this.GetAddToCartButton().Click();
                    break;
                }
                catch (Exception) { }
            }

            //Assert
            Assert.DoesNotThrow(() => this.GetSuccessfullyAddedToCartMessage());
        }
        #endregion
    }
}
