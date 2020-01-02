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
        IList<string> productNames = new List<string>();
        IList<string> ProductCosts = new List<string>();
        IList<string> Names = new List<string>();
        IList<string> Costs = new List<string>();
        Synchronization sync;
        public static By SignIn = By.XPath("//div[contains(@class,'myAccountTab')]");
        public static By LoginLink = By.XPath("//a[text()='login']");
        public static By LoginEmail = By.Name("username");
        public static By Continue = By.Id("checkUser");
        public static By LoginPassword = By.Id("j_password_login_uc");
        public static By Login = By.Id("submitLoginUC");
        public static By Search = By.Id("inputValEnter");
        public static By Sort = By.XPath("//div[@class='sort-drop clearfix']");
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
            _driver.SwitchTo().Frame(_driver.FindElement(By.XPath("//iframe[@name='iframeLogin']")));
            sync.WaitUntilClickable(LoginEmail, 10);
            _driver.FindElement(LoginEmail).SendKeys(ConfigurationManager.AppSettings["Username"]);
            _driver.FindElement(Continue).Click();
            Thread.Sleep(500);
            _driver.FindElement(LoginPassword).SendKeys(ConfigurationManager.AppSettings["Password"]);
            _driver.FindElement(Login).Click();
            _driver.SwitchTo().DefaultContent();
        }

        public void SearchForAProduct(string product)
        {
            Thread.Sleep(2000);
            sync.WaitUntilClickable(Search, 10);
            _driver.FindElement(Search).SendKeys(product);
            _driver.FindElement(Search).SendKeys(Keys.Enter);
        }

        public void ApplyFilter(string Filter)
        {
            Thread.Sleep(200);
            sync.WaitUntilVisible(Sort, 10);
            _driver.FindElement(Sort).Click();
            _driver.FindElement(By.XPath("//li[contains(.,'" + Filter + "')]")).Click();
        }

        public void AddProductsToCart(int count)
        {
            Thread.Sleep(2000);
            string parentWindow = _driver.CurrentWindowHandle;
            IList<IWebElement> products = _driver.FindElements(By.XPath("//div[@id='products']/section[@data-dpwlbl='Product Grid']/div"));
            sync.WaitUntilVisible(By.XPath("//div[@id='products']/section[@data-dpwlbl='Product Grid']/div[1]"),100);
            for (int i=0;i<count;i++)
            {
                products[i].Click();
                Thread.Sleep(2000);
            }
            IList<string> windows = _driver.WindowHandles;
            for(int i=windows.Count-1;i>0;i--)
            {
                _driver.SwitchTo().Window(windows[i]);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView();", _driver.FindElement(By.ClassName("containerBreadcrumb")));
                IWebElement name = _driver.FindElement(By.XPath("//h1[@itemprop='name']"));
                IWebElement cost = _driver.FindElement(By.XPath("//span[@itemprop='price']"));
                //productNames[i - 1] = name.Text.Trim();
                productNames.Add(name.Text.Trim());
                //ProductCosts[i - 1] = cost.Text.Trim();
                ProductCosts.Add(cost.Text.Trim());
                _driver.FindElement(By.XPath("//span[text()='add to cart']")).Click();
                Thread.Sleep(500);
            }           
        }

        public void VerifyProductsIntheCart()
        {
            char[] charsToTrim = { 'R','s','.', '4' };
            _driver.FindElement(By.XPath("//div[contains(@class,'cartContainer ')]")).Click();
            Thread.Sleep(2000);
            IList<IWebElement> PNames = _driver.FindElements(By.XPath("//div[@class='item-description']/div[@class='item-name-wrapper']/a[@class='item-name']"));
            IList<IWebElement> Pcosts = _driver.FindElements(By.XPath("//span[@class='item-price']"));
            for (int i = 0; i < PNames.Count; i++)
            {
                Names.Add( PNames[i].Text.Trim());
                Costs.Add(Pcosts[i].Text.TrimStart(charsToTrim).Trim());
            }
            Assert.AreEqual(productNames,Names,"Both names of the product are not equal");
            Assert.AreEqual(ProductCosts, Costs, "Costs of the product are not equal");
        }
    }
}
