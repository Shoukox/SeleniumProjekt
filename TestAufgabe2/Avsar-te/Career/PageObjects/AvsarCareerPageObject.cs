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

        private IWebElement GetTestAutomatizationVacancyElement() => _webDriver.FindElement(By.XPath("//a[@title=\"Testautomatisierer (m/w/d) Schwerpunkt-Continuous-Integration-Continuous-Testing\"]"));
        private IWebElement GetJuniorSoftwareEngineerVacancyElement() => _webDriver.FindElement(By.XPath("//a[@title=\"Junior Softwareentwickler – Schwerpunkt Automatisierung (m/w/d)\"]"));
        private IWebElement GetITWerkstudentVacancyElement() => _webDriver.FindElement(By.XPath("//a[@title=\"IT Werkstudentenstelle (m/w/d)\"]"));

        public string GetTestAutomatizationVacancyLink() => GetTestAutomatizationVacancyElement().GetAttribute("href");
        public string GetJuniorSoftwareEngineerVacancyLink() => GetJuniorSoftwareEngineerVacancyElement().GetAttribute("href");
        public string GetITWerkstudentVacancyLink() => GetITWerkstudentVacancyElement().GetAttribute("href");

        public readonly string TestAutomatizationPageTitle = "Testautomatisierer (m/w/d) Schwerpunkt-Continuous-Integration-Continuous-Testing - Avsar Test Engineering GmbH";
        public readonly string JuniorSoftwareEngineerPageTitle = "Testautomatisierer - Avsar Test Engineering GmbH";
        public readonly string ITWerkstudentPageTitle = "IT Werkstudentenstelle - Avsar Test Engineering GmbH";

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
