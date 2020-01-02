using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IJPProject
{
    public class Synchronization
    {
        public IWebDriver _driver;
        public Synchronization(IWebDriver driver)
        {
            _driver = driver;
        }
        public void WaitUntilVisible(By locator,int waitInSeconds)
        {
            WebDriverWait w = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitInSeconds));
            w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        public void WaitUntilClickable(By locator, int waitInSeconds)
        {
            WebDriverWait w = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitInSeconds));
            w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }
    }
}
