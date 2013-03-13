using System;
using NUnit.Framework;
using Selenium.Webdriver.Domify.GUITests.Core;
using Selenium.Webdriver.Domify.GUITests.Pages;

namespace Selenium.Webdriver.Domify.GUITests
{
    [TestFixture]
    public class WaitForElementTests: BrowserScenario
    {
        protected override void Given()
        {
            Document.GoTo<HomeIndex>();
        }

        protected override void When()
        {
            Document.GetCurrentPage<HomeIndex>().DelayedTextBox.WaitUntil(c => !string.IsNullOrEmpty(c.Value));
        }

        [Then]
        public void TheTextShouldBeDisplayed()
        {
            Assert.That(Document.GetCurrentPage<HomeIndex>().DelayedTextBox.Value, Is.EqualTo("5 seconds elapsed"));
        }

    }

    [TestFixture]
    public class IfNothingHappensWaitUntilTimesout : BrowserScenario
    {
        protected override void Given()
        {
            Document.GoTo<HomeIndex>();
        }

        protected override void When()
        {
            Document.GetCurrentPage<HomeIndex>().DelayedTextBox.Value = "";
        }

        [Then]
        [ExpectedException(typeof(TimeoutException))]
        public void TimeoutExceptionShouldBeThrown()
        {
            Document.GetCurrentPage<HomeIndex>().DelayedTextBox.WaitUntil(c => !string.IsNullOrEmpty(c.Value), 3);
        }

    }
}