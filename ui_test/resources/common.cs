using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ui_tests.resources
{
    public class common
    {
        private readonly IWebDriver _driver;

        public common(IWebDriver driver)
        {
            _driver = driver;
        }

        public bool VerifyPage(By locator, string expectedText)
        {
            try
            {
                // Locate the element and check if the text matches the expected text
                IWebElement element = _driver.FindElement(locator);
                return element.Text == expectedText;
            }
            catch (NoSuchElementException)
            {
                // Return false if the element is not found
                return false;
            }
        }

        public string GetMessage(By locator)
        {
            try
            {
                IWebElement returnElement = _driver.FindElement(locator);
                return returnElement.Text; // get Element text
            }
            catch (NoSuchElementException)
            {
                return string.Empty; // if no Error Message return empty
            }
        }
    }

}