using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ui_tests.resources;

namespace Ui_tests.resources.page_object
{
    internal class complete_page
    {
        private readonly IWebDriver _driver;
        private common _common;

        public complete_page(IWebDriver driver)
        {
            _driver = driver;
            _common = new common(_driver);
        }

        private By check_out_complete_label = By.XPath("//*[@id=\"header_container\"]/div[2]/span");

        public bool verify_complete_page()
        {
            return _common.VerifyPage(check_out_complete_label, "Checkout: Complete!");
        }

        private IWebElement OrderConfirmationText => _driver.FindElement(By.ClassName("complete-header"));

        public string GetOrderConfirmationText()
        {
            return OrderConfirmationText.Text;
            Logger.Log("Get Order Confirmation Text : " + OrderConfirmationText.Text);
        }
    }
}
