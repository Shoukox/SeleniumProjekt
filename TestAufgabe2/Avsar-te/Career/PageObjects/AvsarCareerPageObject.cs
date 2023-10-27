using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAufgabe2.Tests.Avsar_te.Career.PageObjects
{
    internal class AvsarCareerPageObject
    {
        private readonly string avsarCareerPageUri = "https://www.avsar-te.de/karriere/";

        private readonly IWebDriver _webDriver;
        private readonly int defaultWaitTime = 10;

        private bool wereCookiesAccepted = false;

        public AvsarCareerPageObject()
        {
            _webDriver = new EdgeDriver();
        }

        public string GetCurrentTitle() => _webDriver.Title;
        public void MaximizeWindow() => _webDriver.Manage().Window.Maximize();
        public void QuitAndCloseBrowser() => _webDriver.Quit();

        public IEnumerable<IWebElement> GetAllVacanciesWebElements() => _webDriver.FindElements(By.XPath("//div[@class=\"vc_row wpb_row vc_inner vc_row-fluid vc_row-o-equal-height vc_row-o-content-middle vc_row-flex\"]")).SkipLast(2);
        public IWebElement? GetVacancyContainsText(string text, StringComparison stringComparison) => GetAllVacanciesWebElements().FirstOrDefault(vacancy => vacancy.Text.Replace(" ", "").Contains(text, stringComparison));
        public string GetVacancyLink(IWebElement vacancy) => vacancy.FindElement(By.TagName("a")).GetAttribute("href");

        public string JobPageHeadingIgnoringCaseAndSpaces = "Warum Avsar Test Engineering?".Replace(" ", "").ToLowerInvariant();
        public IWebElement GetJobPageHeading() => _webDriver.FindElement(By.XPath("//div[contains(@id, \"ultimate-heading-\")]"));
        public void OpenAvsarCareerPage()
        {
            //Arrange
            string expectedTitle = "Karriere - Avsar Test Engineering GmbH";

            //Act
            _webDriver.Navigate().GoToUrl(avsarCareerPageUri);
            this.AcceptCookiesIfNeeded();

            //Assert
            Assert.That(this.GetCurrentTitle(), Is.EqualTo(expectedTitle));
        }
        public void OpenJobPage(string uri) => _webDriver.Navigate().GoToUrl(uri);
        public void AcceptCookiesIfNeeded()
        {
            if (!wereCookiesAccepted)
            {
                IWebElement acceptCookiesButton = _webDriver.FindElement(By.XPath("//a[@data-cookie-accept-all]"));
                acceptCookiesButton.Click();
                wereCookiesAccepted = true;
            }
        }
    }
}
