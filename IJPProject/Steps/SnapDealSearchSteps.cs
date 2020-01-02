using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using IJPProject.Pages;
using System.Configuration;

namespace IJPProject.Steps
{
    [Binding]
    public class SnapDealSearchSteps
    {
        IWebDriver _driver;
        LoginPage _loginpage;
        public SnapDealSearchSteps(IWebDriver driver)
        {
            this._driver = driver;
            _loginpage = new LoginPage(_driver);
        }
        [Given(@"I login to the SnapDeal site")]
        public void GivenILoginToTheSnapDealSite()
        {
            _loginpage.LoginToTheApplication(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
        }

        [When(@"I search for '(.*)'")]
        public void WhenISearchForAProduct(string item)
        {
            _loginpage.SearchForAProduct(item);
        }

        [When(@"I apply '(.*)' filter")]
        public void WhenIApplyFilter(string filter)
        {
            _loginpage.ApplyFilter(filter);
        }

        [When(@"I add No of products from the search results to cart")]
        public void WhenIAddOfProductsFromTheSearchResultsToCart()
        {
            _loginpage.AddProductsToCart(Int16.Parse(ConfigurationManager.AppSettings["Count"]));
        }

        [Then(@"I verify the products added to cart")]
        public void ThenIVerifyTheProductsAddedToCart()
        {
            _loginpage.VerifyProductsIntheCart();
        }

    }
}
