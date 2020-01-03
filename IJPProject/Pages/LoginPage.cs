using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IJPProject.Pages
{
    public class LoginPage
    {
        public IWebDriver _driver;       
        Synchronization sync;
        public static By SignIn = By.XPath("//div[contains(@class,'myAccountTab')]");
        public static By LoginLink = By.XPath("//a[text()='login']");
        public static By LoginEmail = By.Name("username");
        public static By Continue = By.Id("checkUser");
        public static By LoginPassword = By.Id("j_password_login_uc");
        public static By Login = By.Id("submitLoginUC");
        
        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            sync = new Synchronization(_driver);
        }
        public void LoginToTheApplication(string uname, string pwd)
        {
            sync.WaitUntilVisible(SignIn, 20);
            _driver.FindElement(SignIn).Click();
            _driver.FindElement(LoginLink).Click();
            sync.WaitUntilClickable(By.XPath("//iframe[@name='iframeLogin']"), 5);
            _driver.SwitchTo().Frame(_driver.FindElement(By.XPath("//iframe[@name='iframeLogin']")));//Login Frame
            sync.WaitUntilVisible(LoginEmail, 10);
            _driver.FindElement(LoginEmail).SendKeys(ConfigurationManager.AppSettings["Username"]);
            _driver.FindElement(Continue).Click();
            Thread.Sleep(500);
            _driver.FindElement(LoginPassword).SendKeys(ConfigurationManager.AppSettings["Password"]);
            _driver.FindElement(Login).Click();
            _driver.SwitchTo().DefaultContent();
        }  
    }
}
