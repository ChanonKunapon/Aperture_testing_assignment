//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium;
//using Ui_tests.resources.page_object;

//namespace Ui_tests.tests
//{
//    public class Tests
//    {
//        [TestFixture]
//        public class CheckoutTests
//        {
//            private IWebDriver _driver;
//            private login_page _loginPage;
//            private ProductPage _productPage;
//            private Cart_Page _cartPage;
//            private CheckoutInformation_Page _checkoutInformationPage;
//            private checkoutOverview_page _checkoutOverviewPage;
//            private complete_page _completePage;

//            [SetUp]
//            public void SetUp()
//            {
//                _driver = new ChromeDriver();
//                _loginPage = new login_page(_driver);
//                _productPage = new ProductPage(_driver);
//                _cartPage = new Cart_Page(_driver);
//                _checkoutInformationPage = new CheckoutInformation_Page(_driver);
//                _checkoutOverviewPage = new checkoutOverview_page(_driver);
//                _completePage = new complete_page(_driver);
//                _driver.Navigate().GoToUrl("https://www.saucedemo.com/");

//                // Verify that we are on the Login page by checking the presence of the Login button
//                IWebElement loginButton = _driver.FindElement(By.XPath("//*[@id='login-button']"));
//                Assert.IsTrue(loginButton.Displayed, "Login button should be displayed on the Login page.");

//            }


//            [Test]

//            public void collectuser_ShouldBeLogin()
//            {
//                // Login
//                _loginPage.Login("standard_user", "secret_sauce");

//                //Verify collect login
//                Assert.IsTrue(_productPage.veriify_product_page(), "Failed to navigate to the product page after login.");
//            }

//            public void CompleteCheckout_ShouldCompleteOrderSuccessfully()
//            {
//                // Login
//                _loginPage.Login("standard_user", "secret_sauce");

//                //Verify collect login
//                Assert.IsTrue(_productPage.veriify_product_page(), "Failed to navigate to the product page after login.");

//                // Add product to cart
//                _productPage.AddBothProductsToCart();

//                // Go to cart
//                _productPage.GoToCart();

//                //verify cart page
//                Assert.IsTrue(_cartPage.verify_cart_page(),"Fail to navigate to cart page after go to cart after click cart button");

//                // Proceed to checkout
//                _cartPage.ProceedToCheckout();

//                //verify checkout infomation page
//                Assert.IsTrue(_checkoutInformationPage.verify_checkoutinfomation_page(),"Fail to navigate to checkout information page after press checkout");

//                // Fill out checkout information
//                _checkoutInformationPage.FillOutCheckoutInformation("Chaon", "kunaponsudjalit", "51000");

//                //Finish Check out information page
//                _checkoutInformationPage.FinishCheckoutInfoimationPage();

//                //verify checkout overview page
//                Assert.IsTrue(_checkoutOverviewPage.verify_checkout_overview_page(), "Fail to navigate to checkout overview page after click continue");

//                // Finish checkout
//                _checkoutOverviewPage.FinishCheckout();

//                //verufy checkout complete page
//                Assert.IsTrue(_completePage.verify_complete_page(),"Fail to navigate to checkout complete page");

//                // Verify order completion
//                string confirmationText = _completePage.GetOrderConfirmationText();
//                Assert.AreEqual("Thank you for your order!", confirmationText);
//            }






//                [TearDown]
//            public void TearDown()
//            {
//                _driver.Quit();
//            }
//        }


//    }
//}






using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Ui_tests.resources.page_object;
using NUnit.Framework;

namespace Ui_tests.tests
{
    public class Tests
    {
        [TestFixture]
        public class CheckoutTests
        {
            private IWebDriver _driver;
            private login_page _loginPage;
            private ProductPage _productPage;
            private Cart_Page _cartPage;
            private CheckoutInformation_Page _checkoutInformationPage;
            private checkoutOverview_page _checkoutOverviewPage;
            private complete_page _completePage;

            [SetUp]
            public void SetUp()
            {
                _driver = new ChromeDriver();
                _loginPage = new login_page(_driver);
                _productPage = new ProductPage(_driver);
                _cartPage = new Cart_Page(_driver);
                _checkoutInformationPage = new CheckoutInformation_Page(_driver);
                _checkoutOverviewPage = new checkoutOverview_page(_driver);
                _completePage = new complete_page(_driver);
                _driver.Navigate().GoToUrl("https://www.saucedemo.com/");

                // Verify that we are on the Login page by checking the presence of the Login button
                IWebElement loginButton = _driver.FindElement(By.XPath("//*[@id='login-button']"));
                Assert.IsTrue(loginButton.Displayed, "Login button should be displayed on the Login page.");
            }

            // Parameterized test for multiple users
            [TestCase("standard_user", "secret_sauce")]
            [TestCase("locked_out_user", "secret_sauce")]
            [TestCase("problem_user", "secret_sauce")]
            [TestCase("performance_glitch_user", "secret_sauce")]
            [TestCase("error_user", "secret_sauce")]  // Add this test case for error handling
            [TestCase("visual_user", "secret_sauce")]  // Add visual test case
            public void CompleteCheckout_ShouldCompleteOrderSuccessfully(string username, string password)
            {
                // Login with parameterized user credentials
                _loginPage.Login(username, password);

                // Verify successful login for valid users (standard_user, problem_user, etc.)
                if (username != "locked_out_user")
                {
                    // Verify correct login
                    Assert.IsTrue(_productPage.veriify_product_page(), "Failed to navigate to the product page after login.");

                    // Add products to cart
                    _productPage.AddBothProductsToCart();

                    // Go to cart
                    _productPage.GoToCart();

                    // Verify cart page
                    Assert.IsTrue(_cartPage.verify_cart_page(), "Failed to navigate to cart page after clicking cart button");

                    // Proceed to checkout
                    _cartPage.ProceedToCheckout();

                    // Verify checkout information page
                    Assert.IsTrue(_checkoutInformationPage.verify_checkoutinfomation_page(), "Failed to navigate to checkout information page after pressing checkout");

                    // Fill out checkout information
                    _checkoutInformationPage.FillOutCheckoutInformation("Chaon", "Kunaponsudjalit", "51000");

                    // Finish Checkout Information Page
                    _checkoutInformationPage.FinishCheckoutInfoimationPage();

                    // Verify checkout overview page
                    Assert.IsTrue(_checkoutOverviewPage.verify_checkout_overview_page(), "Failed to navigate to checkout overview page after clicking continue");

                    // Finish checkout
                    _checkoutOverviewPage.FinishCheckout();

                    // Verify checkout complete page
                    Assert.IsTrue(_completePage.verify_complete_page(), "Failed to navigate to checkout complete page");

                    // Verify order completion
                    string confirmationText = _completePage.GetOrderConfirmationText();
                    Assert.AreEqual("Thank you for your order!", confirmationText);
                }
                else
                {
                    // If the user is locked out, verify that login fails
                    Assert.IsFalse(_productPage.veriify_product_page(), "Locked out user should not be able to access the product page.");
                }
            }

            [TearDown]
            public void TearDown()
            {
                _driver.Quit();
            }
        }
    }
}
