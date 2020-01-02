using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IJPProject.Pages
{
    public class SearchPage
    {
        public IWebDriver _driver;
        public static By Search = By.Id("inputValEnter");
        public static By Sort = By.XPath("//div[@class='sort-drop clearfix']");
        Synchronization sync;
        public SearchPage(IWebDriver driver)
        {
            _driver = driver;
            sync = new Synchronization(_driver);
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
    }
}
