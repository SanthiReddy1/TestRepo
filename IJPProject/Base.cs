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
            driver = new ChromeDriver(@"D:\Users\nagasanthi.katreddy\Downloads\IJPProject (2)\IJPProject\IJPProject\IJPProject\Drivers");
            objectcontainer.RegisterInstanceAs<IWebDriver>(driver);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["site"]);
        }

        [AfterScenario]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
