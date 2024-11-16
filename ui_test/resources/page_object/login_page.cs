using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ui_tests.resources;

namespace Ui_tests.resources.page_object
{
    public class login_page
    {
        private readonly IWebDriver _driver;
        private common _common;

        // Web elements
        private IWebElement UsernameField => _driver.FindElement(By.Id("user-name"));
        private IWebElement PasswordField => _driver.FindElement(By.Id("password"));
        private IWebElement LoginButton => _driver.FindElement(By.Id("login-button"));
        private By error_login_message_location = By.XPath("//*[@id=\"login_button_container\"]/div/form/div[3]/h3");

        public login_page(IWebDriver driver)
        {
            _driver = driver;
            _common = new common(_driver);
        }

        // Methods for interaction with elements
        public void EnterUsername(string username)
        {
            UsernameField.SendKeys(username);
            Logger.Log($"Enter USERNAME = '{username}'");
        }

        public void EnterPassword(string password)
        {
            PasswordField.SendKeys(password);
            Logger.Log($"Enter PASSWORD = '{password}'");
        }

        public void ClickLoginButton()
        {
            LoginButton.Click();
            Logger.Log("Click 'LOGIN' button");
        }

        public void Login(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLoginButton();
        }

        // function to get Error Message
        public string GetErrorMessage()
        {
            string return_message;
            return return_message = _common.GetMessage(error_login_message_location);
            Logger.Log("Get return message :" + return_message);
        }

    }


}
