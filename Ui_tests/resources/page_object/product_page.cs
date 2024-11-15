using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ui_tests.resources;



namespace Ui_tests.resources.page_object
{
    public class ProductPage
    {
        private readonly IWebDriver _driver;
        private common _common;


        private By product_page_label = By.XPath("//*[@id='header_container']/div[2]/span");   
        private By select_1st_product => By.XPath("//*[@id=\"add-to-cart-sauce-labs-backpack\"]");
        private By select_2nd_product => By.XPath("//*[@id=\"add-to-cart-sauce-labs-bolt-t-shirt\"]");
        private IWebElement CartButton => _driver.FindElement(By.XPath("//*[@id=\"shopping_cart_container\"]/a"));


        public ProductPage(IWebDriver driver)
        {
            _driver = driver;
            _common = new common(driver);  // Initialize Common class

        }

        //Verify product page
        public bool veriify_product_page()
        {
            return _common.VerifyPage(product_page_label, "Products");
        }

        // Method to add the first product to the cart
        public void AddFirstProductToCart()
        {
            IWebElement firstProductButton = _driver.FindElement(select_1st_product);
            firstProductButton.Click();
        }

        // Method to add the second product to the cart
        public void AddSecondProductToCart()
        {
            IWebElement secondProductButton = _driver.FindElement(select_2nd_product);
            secondProductButton.Click();
        }

        // Method to add both products to the cart (optional)
        public void AddBothProductsToCart()
        {
            AddFirstProductToCart();
            AddSecondProductToCart();
        }

        public void GoToCart()
        {
            CartButton.Click();
        }
    }


}
