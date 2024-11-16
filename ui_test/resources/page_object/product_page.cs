using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Communication;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ui_tests.resources;
using SeleniumExtras.WaitHelpers;




namespace Ui_tests.resources.page_object
{
    public class ProductPage
    {
        private readonly IWebDriver _driver;
        private common _common;
        private WebDriverWait wait;


        private By product_page_label = By.XPath("//*[@id='header_container']/div[2]/span");
        private By select_1st_product => By.XPath("//*[@id=\"add-to-cart-sauce-labs-backpack\"]");
        private By select_2nd_product => By.XPath("//*[@id=\"add-to-cart-sauce-labs-bolt-t-shirt\"]");
        private IWebElement CartButton => _driver.FindElement(By.XPath("//*[@id=\"shopping_cart_container\"]/a"));


        public ProductPage(IWebDriver driver)
        {
            _driver = driver;
            _common = new common(driver);  // Initialize Common class
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));  // Initialize WebDriverWait
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
            Logger.Log("Add First Product To Cart");
        }

        // Method to add the second product to the cart
        public void AddSecondProductToCart()
        {
            IWebElement secondProductButton = _driver.FindElement(select_2nd_product);
            secondProductButton.Click();
            Logger.Log("Add Second Product To Cart");
        }

        // Method to add both products to the cart (optional)
        public void AddBothProductsToCart()
        {
            AddFirstProductToCart();
            AddSecondProductToCart();
        }

        //press cart button
        public void GoToCart()
        {
            CartButton.Click();
            Logger.Log("Click cart button");
        }

        public void click_treebar_button()
        {
            try
            {
                // Wait for the treebar button to be clickable
                IWebElement treebar_button = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"react-burger-menu-btn\"]")));
                treebar_button.Click();
                Logger.Log("click 3 bar");
            }
            catch (Exception ex)
            {
                Logger.Log("Error clicking treebar button: " + ex.Message);
            }
        }

        public void click_logout()
        {
            try
            {
                // Wait for the logout button to be clickable
                IWebElement logout_button = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"logout_sidebar_link\"]")));
                logout_button.Click();
                Logger.Log("click log out");
            }
            catch (Exception ex)
            {
                Logger.Log("Error clicking logout button: " + ex.Message);
            }
        }

    }


}
