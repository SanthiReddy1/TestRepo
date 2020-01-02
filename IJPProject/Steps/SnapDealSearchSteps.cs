using OpenQA.Selenium;
using System;
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
        SearchPage _searchpage;
        ProductsPage _productpage;
        public SnapDealSearchSteps(IWebDriver driver)
        {
            this._driver = driver;
            _loginpage = new LoginPage(_driver);
            _searchpage = new SearchPage(_driver);
            _productpage = new ProductsPage(_driver);
        }
        [Given(@"I login to the SnapDeal site")]
        public void GivenILoginToTheSnapDealSite()
        {
            _loginpage.LoginToTheApplication(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
        }

        [When(@"I search for '(.*)'")]
        public void WhenISearchForAProduct(string item)
        {
            _searchpage.SearchForAProduct(item);
        }

        [When(@"I apply '(.*)' filter")]
        public void WhenIApplyFilter(string filter)
        {
            _searchpage.ApplyFilter(filter);
        }

        [When(@"I add No of products from the search results to cart")]
        public void WhenIAddOfProductsFromTheSearchResultsToCart()
        {
            _productpage.AddProductsToCart(Int16.Parse(ConfigurationManager.AppSettings["Count"]));
        }

        [Then(@"I verify the products added to cart")]
        public void ThenIVerifyTheProductsAddedToCart()
        {
            _productpage.VerifyProductsIntheCart();
        }
    }
}
