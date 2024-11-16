using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ui_tests.resources;

namespace Ui_tests.resources.page_object
{
    public class Cart_Page
    {
        private readonly IWebDriver _driver;
        private common _common;

        private IWebElement CheckoutButton => _driver.FindElement(By.Id("checkout"));
        private By cart_page_label = By.XPath("//*[@id=\"header_container\"]/div[2]/span");


        public Cart_Page(IWebDriver driver)
        {
            _driver = driver;
            _common = new common(driver);

        }

        public bool verify_cart_page()
        {
            return _common.VerifyPage(cart_page_label, "Your Cart");
        }

        public void ProceedToCheckout()
        {
            CheckoutButton.Click();
            Logger.Log("Proceed To click Checkout button");
        }
    }


}
