using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ui_tests.resources;

namespace Ui_tests.resources.page_object
{
    internal class checkoutOverview_page
    {
        private readonly IWebDriver _driver;
        private common _common;

        private By checkout_overvirew_page_label = By.XPath("//*[@id=\"header_container\"]/div[2]/span");
        private IWebElement FinishButton => _driver.FindElement(By.Id("finish"));

        public checkoutOverview_page(IWebDriver driver)
        {
            _driver = driver;
            _common = new common(_driver);
        }

        public bool verify_checkout_overview_page()
        {
            return _common.VerifyPage(checkout_overvirew_page_label, "Checkout: Overview");
        }


        public void FinishCheckout()
        {
            FinishButton.Click();
            Logger.Log("click Finish Checkout by click 'Finish' button");
        }
    }
}
