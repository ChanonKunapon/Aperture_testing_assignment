using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Ui_tests.resources.page_object;
using NUnit.Framework;
using OpenQA.Selenium.DevTools.V128.SystemInfo;
using Ui_tests.resources;
using static System.Net.WebRequestMethods;


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

                Logger.Log("Test setup open Broswer");
                _driver.Navigate().GoToUrl("https://www.saucedemo.com/");
                Logger.Log("Navigate to https://www.saucedemo.com/");

                // Verify the correct Login page by checking the presence of the Login button
                IWebElement loginButton = _driver.FindElement(By.XPath("//*[@id='login-button']"));
                Logger.Log("Verify the correct website");
                Assert.IsTrue(loginButton.Displayed, "Login button should be displayed on the Login page.");
            }

            // test for multiple users
            [Category("positive login tests")]
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

            //Negative login case
            [Category("negative login tests")]
            [Test]
            public void emptyLogin_ShouldShowErrorMessage()
            {
                // Attempt to login with invalid credentials
                _loginPage.Login("", "");

                // Verify error message is displayed
                string errorMessage = _loginPage.GetErrorMessage();
                Assert.AreEqual("Epic sadface: Username is required", errorMessage);
            }

            [Category("negative login tests")]
            [Test]
            public void emptyUsername_ShouldShowErrorMessage()
            {
                // Attempt to login with invalid credentials
                _loginPage.Login("", "secret_sauce");

                // Verify error message is displayed
                string errorMessage = _loginPage.GetErrorMessage();
                Assert.AreEqual("Epic sadface: Username is required", errorMessage);
            }

            [Category("negative login tests")]
            [Test]
            public void emptyPassword_ShouldShowErrorMessage()
            {
                // Attempt to login with invalid credentials
                _loginPage.Login("standard_user", "");

                // Verify error message is displayed
                string errorMessage = _loginPage.GetErrorMessage();
                Assert.AreEqual("Epic sadface: Password is required", errorMessage);
            }


            //Empty check out tests 
            [Category("checkout test")]
            [Test]
            public void emptyCheckout_shouldShowErrorMessage()
            {

                _loginPage.Login("standard_user", "secret_sauce");

                // Verify correct login
                Assert.IsTrue(_productPage.veriify_product_page(), "Failed to navigate to the product page after login.");

                // Go to cart
                _productPage.GoToCart();

                // Verify cart page
                Assert.IsTrue(_cartPage.verify_cart_page(), "Failed to navigate to cart page after clicking cart button");

                //go to cart without selecting item

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
                Assert.AreNotEqual("Thank you for your order!", confirmationText);


            }

            [Category("checkout test")]
            [TestCase("", "kunapon", "5678")]
            [TestCase("chanon", "", "5678")]
            [TestCase("chanon", "kunapon", "")]
            [TestCase("", "", "")]
            public void emptyInfomationCheckout_shouldShowErrorMessage(string fristname, string lastname, string zip_code)
            {

                _loginPage.Login("standard_user", "secret_sauce");

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

                // Fill Empty checkout information
                _checkoutInformationPage.FillOutCheckoutInformation(fristname, lastname, zip_code);

                // Preform  click continue button
                _checkoutInformationPage.FinishCheckoutInfoimationPage();

                if (fristname == "")
                {
                    //Expected to get error message
                    Assert.AreEqual(_checkoutInformationPage.GetErrorMessage_checkOutInfo(), "Error: First Name is required");
                }
                else if (lastname == "")
                {
                    //Expected to get error message
                    Assert.AreEqual(_checkoutInformationPage.GetErrorMessage_checkOutInfo(), "Error: Last Name is required");
                }
                else if (zip_code == "")
                {
                    //Expected to get error message
                    Assert.AreEqual(_checkoutInformationPage.GetErrorMessage_checkOutInfo(), "Error: Postal Code is required");
                }
                else if (fristname == "" | lastname == "" | zip_code == "")
                {
                    //Expected to get error message
                    Assert.AreEqual(_checkoutInformationPage.GetErrorMessage_checkOutInfo(), "Error: First Name is required");
                }

            }


            [Category("logout test")]
            [Test]
            public void loggoutButton_shoudLoggoffToLoginPage()
            {
                _loginPage.Login("standard_user", "secret_sauce");

                // Verify correct login
                Assert.IsTrue(_productPage.veriify_product_page(), "Failed to navigate to the product page after login.");

                //press loggout button
                _productPage.click_treebar_button();
                _productPage.click_logout();


                //verifyuser back to login page
                IWebElement loginButton = _driver.FindElement(By.XPath("//*[@id='login-button']"));
                Assert.IsTrue(loginButton.Displayed, "Login button should be displayed on the Login page.");

            }




            [TearDown]
            public void TearDown()
            {
                _driver.Quit();
            }
        }
    }
}
