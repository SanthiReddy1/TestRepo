using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using BoDi;
using NUnit.Framework;
using System.IO;

namespace IJPProject
{
    [Binding]
    public class Base
    {
        public IObjectContainer objectcontainer;
        public TestContext TC;
        public ScenarioContext SC;
        public FeatureContext FC;
        public IWebDriver driver;
        public Base(IObjectContainer obj, TestContext TestContext, ScenarioContext ScenarioContext, FeatureContext FeatureContext)
        {
            this.objectcontainer = obj;
            this.TC = TestContext;
            this.SC = ScenarioContext;
            this.FC = FeatureContext;
        }

        [BeforeScenario]
        public void setup()
        {
            driver = new ChromeDriver(@"D:\Users\nagasanthi.katreddy\Downloads\IJPProject\IJPProject\IJPProject\IJPProject\Drivers");
            objectcontainer.RegisterInstanceAs<IWebDriver>(driver);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["site"]);
        }

        [AfterScenario]
        public void TearDown()
        {
            string Error = string.Empty;
            try
            {
                if (SC.TestError != null)
                {
                    Screenshot s = ((ITakesScreenshot)driver).GetScreenshot();
                    string path = @"D:\Users\nagasanthi.katreddy\Desktop\IJPProject\IJPProject\IJPProject\IJPProject\Screenshots\";
                    string Screenshotpath = Path.Combine(path, SC.ScenarioInfo.Title.Replace(" ", "") + "_" + DateTime.Now.ToString("MM-dd-yyyy") + "_ERROR" + ".png");
                    s.SaveAsFile(Screenshotpath, ScreenshotImageFormat.Png);
                    Error = SC.TestError.ToString();
                    Console.WriteLine("error:" + Error);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            driver.Quit();
        }
    }
}
