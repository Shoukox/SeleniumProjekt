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
            _avsarCareerPageObject.OpenAvsarCareerPage();

            //Act
            string vacancyLink = _avsarCareerPageObject.GetTestAutomatizationVacancyLink();
            _avsarCareerPageObject.OpenJobPage(vacancyLink);

            //Assert
            string expectedTitle = _avsarCareerPageObject.TestAutomatizationPageTitle;
            Assert.That(_avsarCareerPageObject.GetCurrentTitle(), Is.EqualTo(expectedTitle));
        }

        [Order(1)]
        [Test]
        public void ChooseJuniorSoftwareEngineerVacancy()
        {
            _avsarCareerPageObject.OpenAvsarCareerPage();

            //Act
            string vacancyLink = _avsarCareerPageObject.GetJuniorSoftwareEngineerVacancyLink();
            _avsarCareerPageObject.OpenJobPage(vacancyLink);

            //Assert
            string expectedTitle = _avsarCareerPageObject.JuniorSoftwareEngineerPageTitle;
            Assert.That(_avsarCareerPageObject.GetCurrentTitle(), Is.EqualTo(expectedTitle));
        }

        [Order(2)]
        [Test]
        public void ChooseITWerkstudentVacancy()
        {
            _avsarCareerPageObject.OpenAvsarCareerPage();

            //Act
            string vacancyLink = _avsarCareerPageObject.GetITWerkstudentVacancyLink();
            _avsarCareerPageObject.OpenJobPage(vacancyLink);

            //Assert
            string expectedTitle = _avsarCareerPageObject.ITWerkstudentPageTitle;
            Assert.That(_avsarCareerPageObject.GetCurrentTitle(), Is.EqualTo(expectedTitle));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _avsarCareerPageObject.QuitAndCloseBrowser();
        }
    }
}
