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
using System.Reflection;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using TechTalk.SpecFlow.Bindings;

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
        public static ExtentHtmlReporter htmlReporter;
        public static ExtentReports extent;
        public static ExtentTest feature;
        public ExtentTest scenario;
        public ExtentTest step;
        public static string featuretitle = string.Empty;
        public string Screenshotpath = string.Empty;
        string Error = string.Empty;

        public Base(IObjectContainer obj, TestContext TestContext, ScenarioContext ScenarioContext)
        {
            this.objectcontainer = obj;
            this.TC = TestContext;
            this.SC = ScenarioContext;
        }

        [BeforeTestRun]
        public static void ReportTestSetUp()
        {
            htmlReporter = new ExtentHtmlReporter(GetProjectLocation() + @"\\HtmlReport\\Reports.html");
            htmlReporter.Configuration().Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [BeforeFeature]
        public static void feautreinfo(FeatureContext FeatureContext)
        {
            featuretitle = FeatureContext.FeatureInfo.Title;
            feature = extent.CreateTest<Feature>(featuretitle);
        }

        [BeforeScenario]
        public void setup()
        {
            //driver = new ChromeDriver(@"D:\Users\nagasanthi.katreddy\Downloads\IJPProject\IJPProject\IJPProject\IJPProject\Drivers");
            driver = new ChromeDriver(GetProjectLocation() + @"\\Drivers");
            objectcontainer.RegisterInstanceAs<IWebDriver>(driver);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["site"]);
            scenario = feature.CreateNode<Scenario>(SC.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void TearDown()
        {
            if (SC.TestError != null)
            {
                scenario.Log(Status.Fail, SC.TestError);
                scenario.Fail(SC.TestError);
                scenario.Error(SC.TestError);
            }
            else
            {
                scenario.Log(Status.Pass,"Scenario Failed");
            }
            driver.Quit();
        }

        [AfterStep]
        public void StepInfo()
        {
            var stepDefinitionType = SC.StepContext.StepInfo.StepDefinitionType.ToString();
            
            if (SC.TestError != null)
            {
                Screenshot s = ((ITakesScreenshot)driver).GetScreenshot();
                string path = GetProjectLocation() + @"\Screenshots";
                Screenshotpath = Path.Combine(path, SC.ScenarioInfo.Title.Replace(" ", "") + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + "_ERROR" + ".png");
                s.SaveAsFile(Screenshotpath, ScreenshotImageFormat.Png);
                Error = SC.TestError.ToString();
                Console.WriteLine("error:" + Error);
                //step.Error(SC.TestError);
                //step.Fail(SC.TestError, MediaEntityBuilder.CreateScreenCaptureFromPath(Screenshotpath).Build());
                //step.Log(Status.Fail);

                if (stepDefinitionType.Equals("Given"))
                    scenario.CreateNode<Given>(SC.StepContext.StepInfo.Text).Fail(SC.TestError, MediaEntityBuilder.CreateScreenCaptureFromPath(Screenshotpath).Build()).Log(Status.Fail,"Step Failed");
                else if (stepDefinitionType.Equals("When"))
                    scenario.CreateNode<When>(SC.StepContext.StepInfo.Text).Fail(SC.TestError, MediaEntityBuilder.CreateScreenCaptureFromPath(Screenshotpath).Build()).Log(Status.Fail, "Step Failed");
                else if (stepDefinitionType.Equals("Then"))
                    scenario.CreateNode<Then>(SC.StepContext.StepInfo.Text).Fail(SC.TestError, MediaEntityBuilder.CreateScreenCaptureFromPath(Screenshotpath).Build()).Log(Status.Fail, "Step Failed");
                else if (stepDefinitionType.Equals("And"))
                    scenario.CreateNode<And>(SC.StepContext.StepInfo.Text).Fail(SC.TestError, MediaEntityBuilder.CreateScreenCaptureFromPath(Screenshotpath).Build()).Log(Status.Fail, "Step Failed");

            }
            else
            {
                if (stepDefinitionType.Equals("Given"))
                   scenario.CreateNode<Given>(SC.StepContext.StepInfo.Text);
                else if (stepDefinitionType.Equals("When"))
                   scenario.CreateNode<When>(SC.StepContext.StepInfo.Text);
                else if (stepDefinitionType.Equals("Then"))
                    scenario.CreateNode<Then>(SC.StepContext.StepInfo.Text);
                else if (stepDefinitionType.Equals("And"))
                   scenario.CreateNode<And>(SC.StepContext.StepInfo.Text);
            }
        }

        [AfterTestRun]
        public static void EndReport()
        {
            extent.Flush();
        }

        public static string GetProjectLocation()
        {
            string project = Assembly.GetCallingAssembly().GetName().Name;
            string sDirectory = AppDomain.CurrentDomain.BaseDirectory;
            sDirectory = sDirectory.Substring(0, sDirectory.LastIndexOf(project));
            sDirectory = Path.Combine(sDirectory, project);
            Console.WriteLine("path is:=>" + sDirectory);
            return sDirectory;
        }
    }
}
