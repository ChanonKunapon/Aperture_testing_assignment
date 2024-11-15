using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ui_tests.resources;

namespace Ui_tests.resources.page_object
{
    internal class CheckoutInformation_Page
    {
        private readonly IWebDriver _driver;
        private common _common;


        public CheckoutInformation_Page(IWebDriver driver)
        {
            _driver = driver;
            _common = new common(driver);
        }

        private IWebElement FirstNameField => _driver.FindElement(By.Id("first-name"));
        private IWebElement LastNameField => _driver.FindElement(By.Id("last-name"));
        private IWebElement ZipcodeField => _driver.FindElement(By.Id("postal-code"));
        private IWebElement ContinueButton => _driver.FindElement(By.Id("continue"));
        private By checkout_infio_label => By.XPath("//*[@id=\"header_container\"]/div[2]/span");

        public bool verify_checkoutinfomation_page()
        {
            return _common.VerifyPage(checkout_infio_label, "Checkout: Your Information");
        }


        public void FillOutCheckoutInformation(string firstName, string lastName, string zipcode)
        {
            FirstNameField.SendKeys(firstName);
            LastNameField.SendKeys(lastName);
            ZipcodeField.SendKeys(zipcode);            
        }

        public void FinishCheckoutInfoimationPage()
        {
            ContinueButton.Click();
        }

    }
}
