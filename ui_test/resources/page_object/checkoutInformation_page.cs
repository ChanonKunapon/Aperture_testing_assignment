using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        private By checkout_info_label => By.XPath("//*[@id=\"header_container\"]/div[2]/span");
        private By checkout_info_error_location => By.XPath("//*[@id=\"checkout_info_container\"]/div/form/div[1]/div[4]");

        public bool verify_checkoutinfomation_page()
        {
            return _common.VerifyPage(checkout_info_label, "Checkout: Your Information");
        }


        public void FillOutCheckoutInformation(string firstName, string lastName, string zipcode)
        {
            FirstNameField.SendKeys(firstName);
            Logger.Log("Proceed To field fristname : " + firstName);
            LastNameField.SendKeys(lastName);
            Logger.Log("Proceed To field lastName : " + lastName);
            ZipcodeField.SendKeys(zipcode);
            Logger.Log("Proceed To field zipcode : " + zipcode);
        }

        public void FinishCheckoutInfoimationPage()
        {
            ContinueButton.Click();
            Logger.Log("click Finish Checkout by click continue button");
        }


        public string GetErrorMessage_checkOutInfo()
        {
            string error_msg;
            return error_msg = _common.GetMessage(checkout_info_error_location);
            Logger.Log("check error message :" + error_msg);

        }

    }
}
