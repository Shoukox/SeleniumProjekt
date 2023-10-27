using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAufgabe2.Tests.Avsar_te.Career.PageObjects;

namespace TestAufgabe2.Tests.Avsar_te
{
    internal class AvsarCareerPageTests
    {
        private readonly AvsarCareerPageObject _avsarCareerPageObject = new AvsarCareerPageObject();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _avsarCareerPageObject.MaximizeWindow();
        }

        [Order(0)]
        [Test]
        public void ChooseTestAutomatizationVacancy()
        {
            //Arrange
            string vacancyPartTextIgnoringCaseAndSpaces = "testautomatisierer";

            //Act
            _avsarCareerPageObject.OpenAvsarCareerPage();

            IWebElement? vacancy = _avsarCareerPageObject.GetVacancyContainsText(vacancyPartTextIgnoringCaseAndSpaces, StringComparison.InvariantCultureIgnoreCase);
            if (vacancy is null)
                Assert.Fail("There is no Test Automatization vacancy");

            string vacancyLink = _avsarCareerPageObject.GetVacancyLink(vacancy!);
            _avsarCareerPageObject.OpenJobPage(vacancyLink);

            //Assert
            string actualJobPageHeadingIgnoringCaseAndSpaces = _avsarCareerPageObject.GetJobPageHeading().Text.Replace(" ", "").ToLowerInvariant();
            Assert.That(_avsarCareerPageObject.JobPageHeadingIgnoringCaseAndSpaces, Is.EqualTo(actualJobPageHeadingIgnoringCaseAndSpaces));
        }

        [Order(1)]
        [Test]
        public void ChooseJuniorSoftwareEngineerVacancy()
        {
            //Arrange
            string vacancyPartTextIgnoringCaseAndSpaces = "juniorsoftware";

            //Act
            _avsarCareerPageObject.OpenAvsarCareerPage();

            IWebElement? vacancy = _avsarCareerPageObject.GetVacancyContainsText(vacancyPartTextIgnoringCaseAndSpaces, StringComparison.InvariantCultureIgnoreCase);
            if (vacancy is null)
                Assert.Fail("There is no Junior Software Engineer vacancy");

            string vacancyLink = _avsarCareerPageObject.GetVacancyLink(vacancy!);
            _avsarCareerPageObject.OpenJobPage(vacancyLink);

            //Assert
            string actualJobPageHeadingIgnoringCaseAndSpaces = _avsarCareerPageObject.GetJobPageHeading().Text.Replace(" ", "").ToLowerInvariant();
            Assert.That(_avsarCareerPageObject.JobPageHeadingIgnoringCaseAndSpaces, Is.EqualTo(actualJobPageHeadingIgnoringCaseAndSpaces));
        }

        [Order(2)]
        [Test]
        public void ChooseITWerkstudentVacancy()
        {
            //Arrange
            string vacancyPartTextIgnoringCaseAndSpaces = "itwerkstudent";

            //Act
            _avsarCareerPageObject.OpenAvsarCareerPage();

            IWebElement? vacancy = _avsarCareerPageObject.GetVacancyContainsText(vacancyPartTextIgnoringCaseAndSpaces, StringComparison.InvariantCultureIgnoreCase);
            if (vacancy is null)
                Assert.Fail("There is no IT Werkstudent vacancy");

            string vacancyLink = _avsarCareerPageObject.GetVacancyLink(vacancy!);
            _avsarCareerPageObject.OpenJobPage(vacancyLink);

            //Assert
            string actualJobPageHeadingIgnoringCaseAndSpaces = _avsarCareerPageObject.GetJobPageHeading().Text.Replace(" ", "").ToLowerInvariant();
            Assert.That(_avsarCareerPageObject.JobPageHeadingIgnoringCaseAndSpaces, Is.EqualTo(actualJobPageHeadingIgnoringCaseAndSpaces));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _avsarCareerPageObject.QuitAndCloseBrowser();
        }
    }
}
