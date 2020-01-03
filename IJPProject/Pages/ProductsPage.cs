using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IJPProject.Pages
{
    public class ProductsPage
    {
        public IWebDriver _driver;
        public Synchronization sync;
        IList<string> productNames = new List<string>();
        IList<string> ProductCosts = new List<string>();
        IList<string> Names = new List<string>();
        IList<string> Costs = new List<string>();
        public static By AddtoCart = By.XPath("//span[text()='add to cart']");
        public static By Cart = By.XPath("//div[contains(@class,'cartContainer ')]");
        public ProductsPage(IWebDriver driver)
        {
            _driver = driver;
            sync = new Synchronization(_driver);
        }
        public void AddProductsToCart(int count)
        {
            Thread.Sleep(2000);
            string parentWindow = _driver.CurrentWindowHandle;
            sync.WaitUntilVisible(By.XPath("//div[@id='products']/section[@data-dpwlbl='Product Grid']/div[1]"), 100);
            IList<IWebElement> products = _driver.FindElements(By.XPath("//div[@id='products']/section[@data-dpwlbl='Product Grid']/div"));

            for (int i = 0; i < count; i++) //get product names and costs
            {
                IList<string> window = null;      
                products[i].Click();
                window = _driver.WindowHandles;
                _driver.SwitchTo().Window(window[i+1]);
                sync.WaitUntilVisible(By.ClassName("containerBreadcrumb"),5);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView();", _driver.FindElement(By.ClassName("containerBreadcrumb")));
                IWebElement name = _driver.FindElement(By.XPath("//h1[@itemprop='name']"));
                IWebElement cost = _driver.FindElement(By.XPath("//span[@itemprop='price']"));
                //productNames[i - 1] = name.Text.Trim();
                productNames.Add(name.Text.Trim());
                //ProductCosts[i - 1] = cost.Text.Trim();
                ProductCosts.Add(cost.Text.Trim());
                _driver.FindElement(AddtoCart).Click();
                Thread.Sleep(500);
                _driver.SwitchTo().Window(parentWindow);
            }
        }

        public void VerifyProductsIntheCart()
        {
            char[] charsToTrim = { 'R', 's', '.', '4' };
            _driver.Navigate().Refresh();
            sync.WaitUntilClickable(Cart,5);
            _driver.FindElement(Cart).Click();
            Thread.Sleep(2000);
            IList<IWebElement> PNames = _driver.FindElements(By.XPath("//a[@class='item-name']"));
            IList<IWebElement> Pcosts = _driver.FindElements(By.XPath("//span[@class='item-price']"));
            int ProductsCount = PNames.Count;

            for (int i = ProductsCount; i >0; i--) //Get name and cost of products in cart
            {
                Names.Add(PNames[i - 1].Text.Trim());
                Costs.Add(Pcosts[i - 1].Text.TrimStart(charsToTrim).Trim());
            }
            Assert.AreEqual(productNames, Names, "Both names of the product are not equal");
            Assert.AreEqual(ProductCosts, Costs, "Costs of the product are not equal");
        }
    }
}
